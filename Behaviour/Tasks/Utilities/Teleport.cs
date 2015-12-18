/*
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

using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class Teleport
    {
        public static async Task<bool> FasterToTeleport(FateData fate)
        {
            var aetheryte = await GetClosestAetheryte(fate);
            var distanceFromPlayer = await GetDistanceFromPlayer(fate);
            var teleportMinDistance = OracleSettings.Instance.TeleportMinimumDistanceDelta;

            Logger.SendDebugLog("Distance to navigate to FATE from player location is ~" + Math.Round(distanceFromPlayer, 0) + " yalms.");
            Logger.SendDebugLog("Distance to navigate to FATE from closest aetheryte location is ~" + Math.Round(aetheryte.Distance, 0)
                                + " yalms.");
            Logger.SendDebugLog("Minimum reduction in distance to use teleport is " + teleportMinDistance + " yalms.");

            if (distanceFromPlayer - aetheryte.Distance <= 0)
            {
                Logger.SendDebugLog("No reduction in distance by teleporting.");
            }
            else
            {
                Logger.SendDebugLog("The distance reduction from teleporting is ~" + Math.Round(distanceFromPlayer - aetheryte.Distance, 0)
                                    + " yalms.");
            }

            if (distanceFromPlayer - aetheryte.Distance > teleportMinDistance)
            {
                return true;
            }

            return false;
        }

        public static async Task<Aetheryte> GetClosestAetheryte(FateData fate)
        {
            var aetherytes = await GetNavigableAetherytes(fate);
            var closestAetheryte = aetherytes.OrderBy(node => node.Distance).FirstOrDefault();

            return closestAetheryte;
        }

        public static async Task<bool> TeleportToAetheryte(uint aetheryteId)
        {
            await
                CommonBehaviors.CreateTeleportBehavior(vr => aetheryteId, vr => WorldManager.GetZoneForAetheryteId(aetheryteId))
                               .ExecuteCoroutine();
            await Coroutine.Wait(TimeSpan.FromSeconds(10), () => !Core.Player.IsCasting || Core.Player.InCombat);
            await Coroutine.Wait(TimeSpan.FromSeconds(2), () => Core.Player.InCombat);
            await Coroutine.Wait(TimeSpan.MaxValue, () => !CommonBehaviors.IsLoading || Core.Player.InCombat);
            await Coroutine.Wait(TimeSpan.FromSeconds(2), () => Core.Player.InCombat);

            return true;
        }

        public static async Task<bool> TeleportToClosestAetheryte(FateData fate)
        {
            var aetheryte = await GetClosestAetheryte(fate);
            await TeleportToAetheryte(aetheryte.Id);

            if (Core.Player.InCombat)
            {
                OracleBehaviour.ClearPoi("We're in combat and need to teleport.");
            }

            return true;
        }

        private static Aetheryte[] AllTuplesToAetherytes(IReadOnlyList<Tuple<uint, Vector3>> tuples, Vector3 location)
        {
            var results = new Aetheryte[tuples.Count];
            for (var i = 0; i < tuples.Count; i++)
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
                    distanceFromPlayer = navResult.PathLength - (fate.Radius * 0.75f);
                }
            }
            else
            {
                distanceFromPlayer = fate.Location.Distance(Core.Player.Location) - (fate.Radius * 0.75f);
            }

            return distanceFromPlayer;
        }

        private static async Task<Aetheryte[]> GetNavigableAetherytes(FateData fate)
        {
            Aetheryte[] viableAetherytes;

            var allAetherytes = AllTuplesToAetherytes(WorldManager.AetheryteIdsForZone(WorldManager.ZoneId), fate.Location);
            var viableAetheryteList = new List<Aetheryte>();

            if (!WorldManager.CanFly || !PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                var navRequest = allAetherytes.Select(target => new CanFullyNavigateTarget {Id = target.Id, Position = target.Location});
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, fate.Location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate != 0))
                {
                    var aetheryte = allAetherytes.FirstOrDefault(result => result.Id == navResult.Id);
                    aetheryte.Distance = navResult.PathLength - (fate.Radius * 0.75f);
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