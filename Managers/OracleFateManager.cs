﻿/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Oracle.Behaviour;
using Oracle.Data;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal class OracleFateManager : FateManager
    {
        internal static uint CurrentFateId { get; set; }
        internal static bool DoNotWaitBeforeMovingFlag { get; set; }
        internal static uint PreviousFateId { get; set; }
        internal static OracleDatabase OracleDatabase { get; set; }

        public static async Task<bool> AnyViableFates()
        {
            if (!ActiveFates.Any(FateFilter))
            {
                return false;
            }

            await BlacklistBadFates();
            if (ActiveFates.Any(FateFilter))
            {
                return true;
            }

            return false;
        }

        public static async Task BlacklistBadFates()
        {
            if (!WorldManager.CanFly || !PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                var navRequest = ActiveFates.Select(fate => new CanFullyNavigateTarget {Id = fate.Id, Position = fate.Location});
                var navResults =
                    await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate == 0))
                {
                    var fate = ActiveFates.FirstOrDefault(result => result.Id == navResult.Id);
                    if (fate == null || Blacklist.Contains(fate.Id))
                    {
                        continue;
                    }

                    Logger.SendDebugLog("'" + fate.Name + "' cannot be navigated to, blacklisting for its remaining duration.");
                    Blacklist.Add(fate.Id, BlacklistFlags.Node, fate.TimeLeft, "Cannot navigate to FATE.");
                }
            }
        }

        public static void ClearCurrentFate(string reason)
        {
            PreviousFateId = CurrentFateId;
            CurrentFateId = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                OracleBehaviour.ClearPoi(reason);
            }
        }

        public static void ClearCurrentFate(string reason, bool setAsPrevious)
        {
            PreviousFateId = setAsPrevious ? CurrentFateId : 0;
            CurrentFateId = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                OracleBehaviour.ClearPoi(reason);
            }
        }

        public static bool FateFilter(FateData fate)
        {
            var oracleFateData = OracleDatabase.GetFateFromFateData(fate);

            if (oracleFateData.Type == FateType.Boss && !OracleSettings.Instance.BossFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Collect && !OracleSettings.Instance.CollectFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Defence && !OracleSettings.Instance.DefenceFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Escort && !OracleSettings.Instance.EscortFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Kill && !OracleSettings.Instance.KillFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.MegaBoss && !OracleSettings.Instance.MegaBossFatesEnabled)
            {
                return false;
            }

            if (Blacklist.Contains(fate.Id, BlacklistFlags.Node))
            {
                return false;
            }

            if (OracleSettings.Instance.BlacklistedFates.Contains(fate.Id))
            {
                return false;
            }

            if (OracleSettings.Instance.IgnoreLowDurationUnstartedFates)
            {
                if (Math.Abs(fate.Progress) < 0.5f && fate.TimeLeft < TimeSpan.FromSeconds(OracleSettings.Instance.LowRemainingDuration))
                {
                    return false;
                }
            }

            if (oracleFateData.SupportLevel == FateSupportLevel.Unsupported)
            {
                return false;
            }

            if (oracleFateData.SupportLevel == FateSupportLevel.Problematic && !OracleSettings.Instance.RunProblematicFates)
            {
                return false;
            }

            if (oracleFateData.SupportLevel == FateSupportLevel.NotInGame)
            {
                return false;
            }

            if (!FateProgressionMet(fate))
            {
                return false;
            }

            if (fate.Level > Core.Player.ClassLevel + OracleSettings.Instance.FateMaxLevelsAbove)
            {
                return false;
            }

            if (fate.Level < Core.Player.ClassLevel - OracleSettings.Instance.FateMaxLevelsBelow)
            {
                return false;
            }

            return true;
        }

        public static async Task<Dictionary<FateData, float>> GetActiveFateDistances()
        {
            var navRequest = ActiveFates.Select(fate => new CanFullyNavigateTarget {Id = fate.Id, Position = fate.Location});
            var navResults =
                await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

            var activeFates = new Dictionary<FateData, float>();
            foreach (var result in navResults)
            {
                activeFates.Add(GetFateById(result.Id), result.PathLength);
                await Coroutine.Yield();
            }

            return activeFates;
        }

        public static FateData GetCurrentFateData()
        {
            return GetFateById(CurrentFateId);
        }

        public static FateData GetPreviousFateData()
        {
            return GetFateById(PreviousFateId);
        }

        public static void SetDoNotWaitFlag(bool flag)
        {
            DoNotWaitBeforeMovingFlag = flag;
        }

        private static bool FateProgressionMet(FateData fate)
        {
            if (OracleSettings.Instance.WaitAtFateForProgress)
            {
                return true;
            }

            if (OracleDatabase.GetFateFromFateData(fate).Type != FateType.Boss
                && OracleDatabase.GetFateFromFateData(fate).Type != FateType.MegaBoss)
            {
                return true;
            }

            if (OracleDatabase.GetFateFromFateData(fate).Type == FateType.Boss
                && fate.Progress >= OracleSettings.Instance.BossEngagePercentage)
            {
                return true;
            }

            if (OracleDatabase.GetFateFromFateData(fate).Type == FateType.MegaBoss
                && fate.Progress >= OracleSettings.Instance.MegaBossEngagePercentage)
            {
                return true;
            }

            return false;
        }
    }
}