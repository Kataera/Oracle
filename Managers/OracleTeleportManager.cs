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

namespace Oracle.Managers
{
    internal static class OracleTeleportManager
    {
        private static Aetheryte[] AllTuplesToAetherytes(IReadOnlyList<Tuple<uint, Vector3>> tuples, Vector3 location)
        {
            var results = new Aetheryte[tuples.Count];
            for (var i = 0; i < tuples.Count; i++)
            {
                results[i] = TupleToAetheryte(tuples[i], location);
            }

            return results;
        }

        public static async Task<bool> FasterToTeleport(FateData fate)
        {
            if (WorldManager.CanFly)
            {
                return false;
            }

            var aetheryte = await GetClosestAetheryte(fate);

            if (aetheryte.Id == 0)
            {
                Logger.SendDebugLog("No viable aetheryte crystals in this zone.");
                return false;
            }

            var distanceFromPlayer = await GetDistanceFromPlayer(fate);
            Logger.SendDebugLog("Distance to navigate to FATE from player location is " + Math.Round(distanceFromPlayer, 2) + " yalms.");
            Logger.SendDebugLog("Distance to navigate to FATE from closest aetheryte location is " + Math.Round(aetheryte.Distance, 2) + " yalms.");
            Logger.SendDebugLog("Minimum reduction in distance to use teleport is " + MovementSettings.Instance.MinDistanceToTeleport + " yalms.");

            if (distanceFromPlayer - aetheryte.Distance <= 0)
            {
                Logger.SendDebugLog("No reduction in distance by teleporting.");
            }
            else
            {
                Logger.SendDebugLog("The distance reduction from teleporting is " + Math.Round(distanceFromPlayer - aetheryte.Distance, 2) + " yalms.");
            }

            if (distanceFromPlayer - aetheryte.Distance > MovementSettings.Instance.MinDistanceToTeleport)
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

        private static async Task<float> GetDistanceFromPlayer(FateData fate)
        {
            float distanceFromPlayer = 0;

            if (!WorldManager.CanFly)
            {
                var navRequest = new List<CanFullyNavigateTarget>
                {
                    new CanFullyNavigateTarget
                    {
                        Id = fate.ObjectId,
                        Position = fate.Location
                    }
                };
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);
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

        private static async Task<Aetheryte[]> GetNavigableAetherytes(FateData fate)
        {
            Aetheryte[] viableAetherytes;

            var allAetherytes = AllTuplesToAetherytes(OracleFateManager.GetAetheryteIdsForZone(WorldManager.ZoneId), fate.Location);
            var viableAetheryteList = new List<Aetheryte>();

            if (!WorldManager.CanFly)
            {
                var navRequest = allAetherytes.Select(target => new CanFullyNavigateTarget
                {
                    Id = target.Id,
                    Position = target.Location
                });
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, fate.Location, WorldManager.ZoneId);

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

        public static async Task<bool> TeleportToAetheryte(uint aetheryteId)
        {
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));
            await CommonBehaviors.CreateTeleportBehavior(vr => aetheryteId, vr => WorldManager.GetZoneForAetheryteId(aetheryteId)).ExecuteCoroutine();
            await Coroutine.Wait(TimeSpan.FromSeconds(10), () => !Core.Player.IsCasting || Core.Player.InCombat);
            await Coroutine.Wait(TimeSpan.FromSeconds(2), () => Core.Player.InCombat);
            await Coroutine.Wait(TimeSpan.MaxValue, () => !CommonBehaviors.IsLoading || Core.Player.InCombat);
            await Coroutine.Wait(TimeSpan.FromSeconds(2), () => Core.Player.InCombat);

            return true;
        }

        public static async Task<bool> TeleportToClosestAetheryte(FateData fate)
        {
            var aetheryte = await GetClosestAetheryte(fate);

            if (aetheryte.Id == 0)
            {
                Logger.SendDebugLog("No viable aetheryte crystals in this zone.");
                return false;
            }

            await TeleportToAetheryte(aetheryte.Id);

            if (Core.Player.InCombat)
            {
                OracleFateManager.ClearPoi("We're in combat and need to teleport.");
            }

            return true;
        }

        public static async Task TeleportToClosestCity()
        {
            var cityList = new List<WorldManager.TeleportLocation>
            {
                WorldManager.AvailableLocations.FirstOrDefault(loc => loc.AetheryteId == 8),
                WorldManager.AvailableLocations.FirstOrDefault(loc => loc.AetheryteId == 2),
                WorldManager.AvailableLocations.FirstOrDefault(loc => loc.AetheryteId == 9),
                WorldManager.AvailableLocations.FirstOrDefault(loc => loc.AetheryteId == 70),
                WorldManager.AvailableLocations.FirstOrDefault(loc => loc.AetheryteId == 75)
            };

            if (cityList.Any(city => city.ZoneId == WorldManager.ZoneId))
            {
                return;
            }

            Logger.SendLog("Teleporting to closest city.");
            await Coroutine.Wait(TimeSpan.FromSeconds(15), WorldManager.CanTeleport);

            if (!WorldManager.CanTeleport())
            {
                return;
            }

            cityList = cityList.OrderBy(loc => loc.GilCost).Where(loc => WorldManager.HasAetheryteId(loc.AetheryteId)).ToList();
            await TeleportToAetheryte(cityList.FirstOrDefault().AetheryteId);
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