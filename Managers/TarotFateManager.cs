/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Tarot.Behaviour;
using Tarot.Data;
using Tarot.Enumerations;
using Tarot.Helpers;
using Tarot.Settings;

namespace Tarot.Managers
{
    internal class TarotFateManager : FateManager
    {
        internal static FateData CurrentFate { get; set; }
        internal static bool DoNotWaitBeforeMovingFlag { get; set; }
        internal static FateDatabase FateDatabase { get; set; }
        internal static FateData PreviousFate { get; set; }

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
            var tarotFateData = FateDatabase.GetFateWithId(CurrentFate.Id);
            var wasFateAChain = tarotFateData.ChainIdFailure != 0 || tarotFateData.ChainIdSuccess != 0;

            PreviousFate = wasFateAChain ? CurrentFate : null;
            CurrentFate = null;

            if (Poi.Current.Type == PoiType.Fate)
            {
                TarotBehaviour.ClearPoi(reason);
            }
        }

        public static void ClearCurrentFate(string reason, bool setAsPrevious)
        {
            PreviousFate = setAsPrevious ? CurrentFate : null;
            CurrentFate = null;

            if (Poi.Current.Type == PoiType.Fate)
            {
                TarotBehaviour.ClearPoi(reason);
            }
        }

        public static bool FateFilter(FateData fate)
        {
            var tarotFateData = FateDatabase.GetFateWithId(fate.Id);

            if (Blacklist.Contains(fate.Id, BlacklistFlags.Node))
            {
                return false;
            }

            if (tarotFateData.SupportLevel == FateSupportLevel.Unsupported)
            {
                return false;
            }

            if (tarotFateData.SupportLevel == FateSupportLevel.Problematic && !TarotSettings.Instance.RunProblematicFates)
            {
                return false;
            }

            if (tarotFateData.SupportLevel == FateSupportLevel.NotInGame)
            {
                return false;
            }

            if (!FateProgressionMet(fate))
            {
                return false;
            }

            if (fate.Level > Core.Player.ClassLevel + TarotSettings.Instance.FateMaxLevelsAbove)
            {
                return false;
            }

            if (fate.Level < Core.Player.ClassLevel - TarotSettings.Instance.FateMaxLevelsBelow)
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

        public static void SetDoNotWaitFlag(bool flag)
        {
            DoNotWaitBeforeMovingFlag = flag;
        }

        private static bool FateProgressionMet(FateData fate)
        {
            if (TarotSettings.Instance.WaitAtFateForProgress)
            {
                return true;
            }

            if (FateDatabase.GetFateWithId(fate.Id).Type != FateType.Boss
                && FateDatabase.GetFateWithId(fate.Id).Type != FateType.MegaBoss)
            {
                return true;
            }

            if (FateDatabase.GetFateWithId(fate.Id).Type == FateType.Boss
                && fate.Progress >= TarotSettings.Instance.BossEngagePercentage)
            {
                return true;
            }

            if (FateDatabase.GetFateWithId(fate.Id).Type == FateType.MegaBoss
                && fate.Progress >= TarotSettings.Instance.MegaBossEngagePercentage)
            {
                return true;
            }

            return false;
        }
    }
}