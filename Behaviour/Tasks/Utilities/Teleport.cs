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

using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.Utilities
{
    internal static class Teleport
    {
        public static async Task<bool> FasterToTeleport(Vector3 location)
        {
            var aetherytes = await GetNavigableAetherytes(location);
            var distanceFromPlayer = location.Distance(Core.Player.Location) - TarotSettings.Instance.TeleportMinimumDistanceDelta;

            if (aetherytes.Any(aetheryte => location.Distance(aetheryte.Item2) < distanceFromPlayer))
            {
                return true;
            }

            return false;
        }

        public static async Task<Tuple<uint, Vector3>> GetClosestAetheryte(Vector3 location)
        {
            var aetherytes = await GetNavigableAetherytes(location);
            var closestToFate = new Tuple<uint, Vector3>(0, Vector3.Zero);

            foreach (var aetheryte in aetherytes)
            {
                if (closestToFate.Item1 == 0 && closestToFate.Item2 == Vector3.Zero)
                {
                    closestToFate = aetheryte;
                }
                else if (location.Distance(closestToFate.Item2) > location.Distance(aetheryte.Item2))
                {
                    closestToFate = aetheryte;
                }

                await Coroutine.Yield();
            }

            return closestToFate;
        }

        public static async Task<bool> TeleportToAetheryte(uint aetheryteId)
        {
            if (!WorldManager.CanTeleport())
            {
                return false;
            }

            while (!Core.Player.IsDead && !Core.Player.InCombat && WorldManager.ZoneId != WorldManager.GetZoneForAetheryteId(aetheryteId))
            {
                if (Core.Me.IsMounted)
                {
                    await CommonTasks.StopAndDismount();
                }

                if (!Core.Me.IsCasting && !CommonBehaviors.IsLoading)
                {
                    WorldManager.TeleportById(aetheryteId);
                }

                await Coroutine.Yield();
            }

            await Coroutine.Wait(TimeSpan.MaxValue, () => CommonBehaviors.IsLoading);
            return true;
        }

        public static async Task<bool> TeleportToClosestAetheryte(Vector3 location)
        {
            var aetheryte = await GetClosestAetheryte(location);
            await TeleportToAetheryte(aetheryte.Item1);

            return true;
        }

        private static async Task<Tuple<uint, Vector3>[]> GetNavigableAetherytes(Vector3 location)
        {
            var allAetherytes = WorldManager.AetheryteIdsForZone(WorldManager.ZoneId);
            var viableAetherytes = new Tuple<uint, Vector3>[allAetherytes.Length];

            if (!WorldManager.CanFly || !PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                var navRequest = allAetherytes.Select(target => new CanFullyNavigateTarget {Id = target.Item1, Position = target.Item2});
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate == 1))
                {
                    var aetheryte = allAetherytes.FirstOrDefault(result => result.Item1 == navResult.Id);
                    viableAetherytes[viableAetherytes.Length] = aetheryte;

                    await Coroutine.Yield();
                }
            }
            else
            {
                viableAetherytes = allAetherytes;
            }

            return viableAetherytes;
        }
    }
}