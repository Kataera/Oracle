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
    using ff14bot.Navigation;

    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    using NeoGaia.ConnectionHandler;

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

            // Check if there are FATEs we can't navigate to.
            if (!WorldManager.CanFly)
            {
                var navRequest =
                    activeFates.Select(
                        target => new CanFullyNavigateTarget { Id = target.Id, Position = target.Location });
                var navResults =
                    await
                    Navigator.NavigationProvider.CanFullyNavigateToAsync(
                        navRequest,
                        playerLocation,
                        WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate == 0))
                {
                    var val = activeFates.FirstOrDefault(r => r.Id == navResult.Id);
                    if (val != null)
                    {
                        Logger.SendDebugLog(
                            "'" + val.Name + "' cannot be navigated to, blacklisting for its remaining duration.");
                        Blacklist.Add(val.Id, BlacklistFlags.Node, val.TimeLeft, "Cannot navigate to FATE.");
                    }
                }
            }

            // Select closest FATE.
            Logger.SendLog("Selecting closest active FATE.");
            foreach (var fate in activeFates)
            {
                var tarotFate = Tarot.FateDatabase.GetFateWithId(fate.Id);
                Logger.SendDebugLog("Found FATE: '" + fate.Name + "'.");

                // TODO: Convert below to switch statement.
                if (Blacklist.Contains(fate.Id))
                {
                    Logger.SendDebugLog("'" + fate.Name + "' has been blacklisted by Tarot, ignoring.");
                }

                else if (tarotFate.SupportLevel == FateSupportLevel.Unsupported)
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

                else if (closestFate == null)
                {
                    closestFate = fate;
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