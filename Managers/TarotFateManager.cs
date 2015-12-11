﻿/*
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

using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Tarot.Enumerations;
using Tarot.Helpers;
using Tarot.Settings;

namespace Tarot.Managers
{
    internal class TarotFateManager : FateManager
    {
        public static async Task<bool> AnyViableFates()
        {
            if (ActiveFates.Any(FateFilter))
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

        private static async Task BlacklistBadFates()
        {
            if (!WorldManager.CanFly || !PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                var navRequest = ActiveFates.Select(fate => new CanFullyNavigateTarget {Id = fate.Id, Position = fate.Location});
                var navResults =
                    await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate == 0))
                {
                    var fate = ActiveFates.FirstOrDefault(result => result.Id == navResult.Id);
                    if (fate == null)
                    {
                        continue;
                    }

                    Logger.SendDebugLog("'" + fate.Name + "' cannot be navigated to, blacklisting for its remaining duration.");
                    Blacklist.Add(fate.ObjectId, BlacklistFlags.Node, fate.TimeLeft, "Cannot navigate to FATE.");
                }
            }
        }

        private static bool FateFilter(FateData fate)
        {
            var tarotFateData = Tarot.FateDatabase.GetFateWithId(fate.Id);

            if (Blacklist.Contains(fate.ObjectId, BlacklistFlags.Node))
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

            return true;
        }

        private static bool FateProgressionMet(FateData fate)
        {
            if (TarotSettings.Instance.WaitAtFateForProgress)
            {
                return true;
            }

            if (Tarot.FateDatabase.GetFateWithId(fate.Id).Type != FateType.Boss
                && Tarot.FateDatabase.GetFateWithId(fate.Id).Type != FateType.MegaBoss)
            {
                return true;
            }

            if (Tarot.FateDatabase.GetFateWithId(fate.Id).Type == FateType.Boss
                && fate.Progress >= TarotSettings.Instance.BossEngagePercentage)
            {
                return true;
            }

            if (Tarot.FateDatabase.GetFateWithId(fate.Id).Type == FateType.MegaBoss
                && fate.Progress >= TarotSettings.Instance.MegaBossEngagePercentage)
            {
                return true;
            }

            return false;
        }
    }
}