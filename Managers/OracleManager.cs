/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

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

using Oracle.Data;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleManager
    {
        internal static uint CurrentFateId { get; set; }
        internal static bool DeathFlag { get; set; }
        internal static bool DoNotWaitBeforeMovingFlag { get; set; }
        internal static OracleDatabase OracleDatabase { get; set; }
        internal static uint PreviousFateId { get; set; }
        internal static OracleFlightMesh ZoneFlightMesh { get; set; }

        public static async Task<bool> AnyViableFates()
        {
            if (!FateManager.ActiveFates.Any(FateFilter))
            {
                return false;
            }

            await BlacklistBadFates();
            if (FateManager.ActiveFates.Any(FateFilter))
            {
                return true;
            }

            return false;
        }

        public static async Task BlacklistBadFates()
        {
            if (!WorldManager.CanFly)
            {
                var navRequest = FateManager.ActiveFates.Select(fate => new CanFullyNavigateTarget {Id = fate.Id, Position = fate.Location});
                var navResults =
                    await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate == 0))
                {
                    var fate = FateManager.ActiveFates.FirstOrDefault(result => result.Id == navResult.Id);
                    if (fate == null || Blacklist.Contains(fate.Id))
                    {
                        continue;
                    }

                    Logger.SendDebugLog("'" + fate.Name + "' cannot be navigated to, blacklisting for its remaining duration.");
                    Blacklist.Add(fate.Id, BlacklistFlags.Node, fate.TimeLeft, "Cannot navigate to FATE.");
                }
            }
        }

        public static async Task ClearCurrentFate(string reason)
        {
            Logger.SendLog(reason);
            PreviousFateId = CurrentFateId;
            CurrentFateId = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                ClearPoi(reason, false);
            }
        }

        public static async Task ClearCurrentFate(string reason, bool setAsPrevious)
        {
            Logger.SendLog(reason);
            PreviousFateId = setAsPrevious ? CurrentFateId : 0;
            CurrentFateId = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                ClearPoi(reason, false);
            }
        }

        public static void ClearPoi(string reason)
        {
            Logger.SendLog(reason);
            Poi.Clear(reason);
        }

        public static void ClearPoi(string reason, bool sendLog)
        {
            if (sendLog)
            {
                Logger.SendLog(reason);
            }

            Poi.Clear(reason);
        }

        public static bool CurrentFateHasChain()
        {
            var oracleFate = OracleDatabase.GetFateFromId(CurrentFateId);

            if (oracleFate.ChainIdSuccess != 0)
            {
                return true;
            }

            return false;
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

            if (OracleSettings.Instance.OracleOperationMode == OracleOperationMode.SpecificFate
                && !fate.Name.Equals(OracleSettings.Instance.SpecificFate))
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

            if (fate.Level > Core.Player.ClassLevel + OracleSettings.Instance.FateMaximumLevelAbove)
            {
                return false;
            }

            if (fate.Level < Core.Player.ClassLevel - OracleSettings.Instance.FateMinimumLevelBelow)
            {
                return false;
            }

            return true;
        }

        public static bool FateProgressionMet(FateData fate)
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

        public static async Task<Dictionary<FateData, float>> GetActiveFateDistances()
        {
            var activeFates = new Dictionary<FateData, float>();

            if (!WorldManager.CanFly)
            {
                var navRequest = FateManager.ActiveFates.Select(fate => new CanFullyNavigateTarget {Id = fate.Id, Position = fate.Location});
                var navResults =
                    await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

                foreach (var result in navResults.Where(result => result.CanNavigate != 0))
                {
                    activeFates.Add(FateManager.GetFateById(result.Id), result.PathLength);
                    await Coroutine.Yield();
                }
            }
            else
            {
                foreach (var fate in FateManager.ActiveFates)
                {
                    activeFates.Add(fate, fate.Location.Distance(Core.Player.Location));
                }
            }

            return activeFates;
        }

        public static FateData GetCurrentFateData()
        {
            return FateManager.GetFateById(CurrentFateId);
        }

        public static FateData GetPreviousFateData()
        {
            return FateManager.GetFateById(PreviousFateId);
        }

        public static bool IsPlayerBeingAttacked()
        {
            return
                GameObjectManager.Attackers.Any(
                    mob => mob.IsValid && mob.HasTarget && mob.CurrentTargetId == Core.Player.ObjectId && !mob.IsFateGone);
        }

        public static void SetDoNotWaitFlag(bool flag)
        {
            DoNotWaitBeforeMovingFlag = flag;
        }

        public static void UpdateGameCache()
        {
            FateManager.Update();
            GameObjectManager.Clear();
            GameObjectManager.Update();
        }

        public static bool ZoneChangeNeeded()
        {
            const ushort dravanianHinterlands = 399;

            if (Core.Player.IsLevelSynced || Core.Player.IsDead)
            {
                return false;
            }

            if (Poi.Current.Type == PoiType.Kill || Poi.Current.Type == PoiType.Fate || CurrentFateId != 0)
            {
                return false;
            }

            uint aetheryteId;
            OracleSettings.Instance.ZoneLevels.TryGetValue(Core.Player.ClassLevel, out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                return false;
            }

            if (WorldManager.GetZoneForAetheryteId(aetheryteId) == WorldManager.ZoneId)
            {
                return false;
            }

            // Handle Idyllshire.
            if (aetheryteId == 75 && WorldManager.ZoneId == dravanianHinterlands)
            {
                return false;
            }

            return true;
        }
    }
}