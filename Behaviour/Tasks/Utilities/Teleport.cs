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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Tarot.Helpers;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.Utilities
{
    internal static class Teleport
    {
        public static async Task<bool> FasterToTeleport(FateData fate)
        {
            var aetheryte = await GetClosestAetheryte(fate.Location);
            var distanceFromPlayer = await GetDistanceFromPlayer(fate);

            Logger.SendDebugLog("Distance to navigate to '" + fate.Name + "' from player location is " + distanceFromPlayer + " yalms.");
            Logger.SendDebugLog("Distance to navigate to '" + fate.Name + "' from Aetheryte location is " + aetheryte.Distance + " yalms.");

            if (distanceFromPlayer - aetheryte.Distance <= 0)
            {
                Logger.SendDebugLog("Distance saved by teleporting is 0 yalms.");
            }
            else
            {
                Logger.SendDebugLog("Distance saved by teleporting is " + (distanceFromPlayer - aetheryte.Distance) + " yalms.");
            }

            Logger.SendDebugLog("Minimum distance difference to teleport is " + TarotSettings.Instance.TeleportMinimumDistanceDelta + " yalms.");

            if (distanceFromPlayer - aetheryte.Distance > TarotSettings.Instance.TeleportMinimumDistanceDelta)
            {
                return true;
            }

            return false;
        }

        public static async Task<Aetheryte> GetClosestAetheryte(Vector3 location)
        {
            var aetherytes = await GetNavigableAetherytes(location);
            var closestToFate = new Aetheryte
            {
                Distance = 0,
                Id = 0,
                Location = Vector3.Zero
            };

            foreach (var aetheryte in aetherytes)
            {
                if (closestToFate.Id == 0)
                {
                    closestToFate = aetheryte;
                }
                else if (closestToFate.Distance > aetheryte.Distance)
                {
                    closestToFate = aetheryte;
                }

                await Coroutine.Yield();
            }

            return closestToFate;
        }

        public static async Task<bool> TeleportToAetheryte(uint aetheryteId)
        {
            await
                CommonBehaviors.CreateTeleportBehavior(vr => aetheryteId, vr => WorldManager.GetZoneForAetheryteId(aetheryteId))
                               .ExecuteCoroutine();
            await Coroutine.Wait(TimeSpan.FromSeconds(10), () => !Core.Player.IsCasting);
            await Coroutine.Sleep(TimeSpan.FromSeconds(5));
            await Coroutine.Wait(TimeSpan.MaxValue, () => !CommonBehaviors.IsLoading);

            return true;
        }

        public static async Task<bool> TeleportToClosestAetheryte(Vector3 location)
        {
            var aetheryte = await GetClosestAetheryte(location);
            await TeleportToAetheryte(aetheryte.Id);

            return true;
        }

        public static async Task<bool> TeleportToClosestAetheryte(FateData fate)
        {
            var aetheryte = await GetClosestAetheryte(fate.Location);
            await TeleportToAetheryte(aetheryte.Id);

            return true;
        }

        private static Aetheryte[] AllTuplesToAetherytes(Tuple<uint, Vector3>[] tuples, Vector3 location)
        {
            var results = new Aetheryte[tuples.Length];
            foreach (var tuple in tuples)
            {
                results[results.Length - 1] = TupleToAetheryte(tuple, location);
            }

            return results;
        }

        private static async Task<float> GetDistanceFromPlayer(FateData fate)
        {
            float distanceFromPlayer = 0;

            if (!WorldManager.CanFly || !PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                var navRequest = new List<CanFullyNavigateTarget>
                {
                    new CanFullyNavigateTarget {Id = fate.ObjectId, Position = fate.Location}
                };
                var navResults =
                    await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);
                var navResult = navResults.FirstOrDefault();

                if (navResult != null)
                {
                    distanceFromPlayer = navResult.PathLength;
                }
            }
            else
            {
                distanceFromPlayer = fate.Location.Distance(Core.Player.Location);
            }

            return distanceFromPlayer;
        }

        private static async Task<Aetheryte[]> GetNavigableAetherytes(Vector3 location)
        {
            var allAetherytes = AllTuplesToAetherytes(WorldManager.AetheryteIdsForZone(WorldManager.ZoneId), location);
            var viableAetherytes = new Aetheryte[allAetherytes.Length];

            if (!WorldManager.CanFly || !PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                var navRequest = allAetherytes.Select(target => new CanFullyNavigateTarget {Id = target.Id, Position = target.Location});
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate == 1))
                {
                    var aetheryte = allAetherytes.FirstOrDefault(result => result.Id == navResult.Id);
                    aetheryte.Distance = navResult.PathLength;
                    viableAetherytes[viableAetherytes.Length - 1] = aetheryte;

                    await Coroutine.Yield();
                }
            }
            else
            {
                viableAetherytes = allAetherytes;
            }

            return viableAetherytes;
        }

        private static Aetheryte TupleToAetheryte(Tuple<uint, Vector3> tuple, float distance)
        {
            return new Aetheryte
            {
                Distance = distance,
                Id = tuple.Item1,
                Location = tuple.Item2
            };
        }

        private static Aetheryte TupleToAetheryte(Tuple<uint, Vector3> tuple, Vector3 location)
        {
            return new Aetheryte
            {
                Distance = tuple.Item2.Distance(location),
                Id = tuple.Item1,
                Location = tuple.Item2
            };
        }

        public struct Aetheryte
        {
            public float Distance;
            public uint Id;
            public Vector3 Location;
        }
    }
}