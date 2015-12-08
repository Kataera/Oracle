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

namespace Tarot.Behaviour.Tasks.Selectors.Fates
{
    using System.Linq;
    using System.Threading.Tasks;

    using ff14bot;
    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class Closest
    {
        public static async Task<bool> Task()
        {
            var activeFates = FateManager.ActiveFates;

            var playerLocation = Core.Player.Location;
            FateData closestFate = null;

            if (activeFates == null || !activeFates.Any())
            {
                return false;
            }

            Logger.SendLog("Selecting closest active FATE.");
            foreach (var fate in activeFates)
            {
                var tarotFate = Tarot.FateDatabase.GetFateWithId(fate.Id);
                Logger.SendDebugLog("Found FATE: '" + fate.Name + "'.");
                if (closestFate == null)
                {
                    closestFate = fate;
                }

                // TODO: Convert below to switch statement.
                if (tarotFate.SupportLevel == FateSupportLevel.Unsupported)
                {
                    Logger.SendDebugLog("'" + fate.Name + "' has been flagged as unsupported, ignoring.");
                }

                else if (tarotFate.SupportLevel == FateSupportLevel.Problematic
                         && !TarotSettings.Instance.RunProblematicFates)
                {
                    Logger.SendDebugLog(
                        "'" + fate.Name + "' has been flagged as problematic, ignoring due to user settings.");
                }

                // Sanity check.
                else if (tarotFate.SupportLevel == FateSupportLevel.NotInGame)
                {
                    Logger.SendErrorLog(
                        "'" + fate.Name
                        + "' has been flagged as not in game, yet Tarot has found it. Please inform Kataera on the RebornBuddy forums and include this log.");
                    Logger.SendErrorLog("Fate Name: " + fate.Name);
                    Logger.SendErrorLog("Fate ID: " + fate.Id);
                    Logger.SendErrorLog("Fate Location: " + fate.Location);
                    Logger.SendErrorLog("Fate Zone: " + WorldManager.ZoneId);
                }

                else if (playerLocation.Distance2D(closestFate.Location) > playerLocation.Distance2D(fate.Location))
                {
                    closestFate = fate;
                }
            }

            // ReSharper disable once PossibleNullReferenceException
            Logger.SendLog("Selected FATE: '" + closestFate.Name + "'.");

            // Set FATE in Tarot and the Poi.
            Tarot.CurrentFate = closestFate;
            Poi.Current = new Poi(closestFate, PoiType.Fate);

            return true;
        }
    }
}