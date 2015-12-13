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
            var teleportMinDistance = TarotSettings.Instance.TeleportMinimumDistanceDelta;

            Logger.SendDebugLog("Distance to navigate to '" + fate.Name + "' from player location is " + Math.Round(distanceFromPlayer, 0)
                                + " yalms.");
            Logger.SendDebugLog("Distance to navigate to '" + fate.Name + "' from aetheryte location is "
                                + Math.Round(aetheryte.Distance, 0) + " yalms.");
            Logger.SendDebugLog("Minimum distance needed to be saved to teleport is " + teleportMinDistance + " yalms.");

            if (distanceFromPlayer - aetheryte.Distance <= 0)
            {
                Logger.SendDebugLog("No distance is saved by teleporting.");
            }
            else
            {
                Logger.SendDebugLog("The distance saved by teleporting is " + Math.Round(distanceFromPlayer - aetheryte.Distance, 0)
                                    + " yalms.");
            }

            if (distanceFromPlayer - aetheryte.Distance > teleportMinDistance)
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
                Distance = float.MaxValue,
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
            for (var i = 0; i < tuples.Length; i++)
            {
                results[i] = TupleToAetheryte(tuples[i], location);
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
            Aetheryte[] viableAetherytes;

            var allAetherytes = AllTuplesToAetherytes(WorldManager.AetheryteIdsForZone(WorldManager.ZoneId), location);
            var viableAetheryteList = new List<Aetheryte>();

            if (!WorldManager.CanFly || !PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                var navRequest = allAetherytes.Select(target => new CanFullyNavigateTarget {Id = target.Id, Position = target.Location});
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate != 0))
                {
                    var aetheryte = allAetherytes.FirstOrDefault(result => result.Id == navResult.Id);
                    aetheryte.Distance = navResult.PathLength;
                    viableAetheryteList.Add(aetheryte);
                    await Coroutine.Yield();
                }

                viableAetherytes = viableAetheryteList.ToArray();
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