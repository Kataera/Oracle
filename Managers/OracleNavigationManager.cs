using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Pathing;
using ff14bot.ServiceClient;

namespace Oracle.Managers
{
    internal static class OracleNavigationManager
    {
        private static List<Tuple<uint, Vector3>> aetheryteList;

        internal static List<Tuple<uint, Vector3>> AetheryteList
        {
            get
            {
                if (aetheryteList != null)
                {
                    return aetheryteList;
                }

                // Scan through all zone ids for aetherytes. Icky, but only real solution.
                var results = new List<Tuple<uint, Vector3>>();
                for (uint i = 0; i < 700; i++)
                {
                    results.AddRange(GetAetherytesForZone(i));
                }

                results.Sort((x, y) => x.Item1.CompareTo(y.Item1));
                aetheryteList = results;
                return aetheryteList;
            }
        }

        internal static async Task<List<CanFullyNavigateResult>> AreLocationsNavigable(List<CanFullyNavigateTarget> targets)
        {
            var navTask = Navigator.NavigationProvider.CanFullyNavigateTo(targets, Core.Player.Location, WorldManager.ZoneId);
            return await Coroutine.ExternalTask(navTask);
        }

        internal static Tuple<uint, Vector3> GetAetheryteById(uint id)
        {
            return AetheryteList.FirstOrDefault(a => a.Item1 == id);
        }

        internal static Tuple<uint, Vector3> GetAetheryteByLocation(Vector3 location)
        {
            return AetheryteList.FirstOrDefault(a => a.Item2 == location);
        }

        internal static List<Tuple<uint, Vector3>> GetAetherytesForZone(uint zoneId)
        {
            var baseResults = WorldManager.AetheryteIdsForZone(zoneId).ToList();
            foreach (var result in WorldManager.AetheryteIdsForZone(zoneId))
            {
                // TODO: Test if this safety check for Little Ala Mhigo is still needed.
                if (result.Item1 == 19)
                {
                    baseResults.Remove(result);
                    baseResults.Add(new Tuple<uint, Vector3>(19, new Vector3(-168.7496f, 26.1383f, -418.8336f)));
                }
            }

            baseResults.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            return baseResults.ToList();
        }

        internal static async Task<Tuple<uint, Vector3>> GetClosestAetheryte(uint zoneId)
        {
            var aetherytes = GetAetherytesForZone(zoneId);
            var navRequest = aetherytes.Select(aetheryte => new CanFullyNavigateTarget
            {
                Id = aetheryte.Item1,
                Position = aetheryte.Item2
            }).ToList();

            var navResults = await AreLocationsNavigable(navRequest);
            var navigableAetherytes = navResults.Where(result => result.CanNavigate != 0).Select(result => GetAetheryteById(result.Id)).ToList();

            navigableAetherytes.Sort((x, y) => Core.Player.Location.Distance2D(x.Item2).CompareTo(Core.Player.Location.Distance2D(y.Item2)));
            return navigableAetherytes.FirstOrDefault();
        }

        internal static bool InterruptNavigation(bool interruptOnFateInvalid)
        {
            // Handle combat.
            if (!Core.Player.IsMounted && Core.Player.InCombat)
            {
                var attacker = OracleTargetManager.GetAttacker();
                if (attacker != null)
                {
                    Poi.Current = new Poi(attacker, PoiType.Kill);
                    return true;
                }
            }

            // Handle death.
            if (Core.Player.IsDead)
            {
                Poi.Clear("Died while moving.");
                return true;
            }

            if (interruptOnFateInvalid && !OracleFateManager.IsCurrentFateValid())
            {
                OracleFateManager.ClearFate();
                return true;
            }

            return false;
        }

        internal static async Task<bool> NavigateToFate()
        {
            if (!OracleFateManager.IsCurrentFateValid())
            {
                OracleFateManager.ClearFate();
            }

            var movementParams = new MoveToParameters
            {
                Location = OracleFateManager.GameFateData.Location,
                MoveImmediately = true
            };

            while (Core.Player.Location.Distance2D(OracleFateManager.GameFateData.Location) > OracleFateManager.GameFateData.Radius * 0.8)
            {
                Navigator.MoveTo(movementParams);

                if (InterruptNavigation(true))
                {
                    Navigator.Stop();
                    return true;
                }

                await Coroutine.Yield();
            }

            await CommonTasks.StopAndDismount();
            return true;
        }

        internal static async Task<bool> NavigateToLocation(Vector3 location, float range)
        {
            var movementParams = new MoveToParameters
            {
                Location = location,
                MoveImmediately = true
            };

            while (Core.Player.Location.Distance2D(location) > range)
            {
                Navigator.MoveTo(movementParams);

                if (InterruptNavigation(false))
                {
                    Navigator.Stop();
                    return true;
                }

                await Coroutine.Yield();
            }

            await CommonTasks.StopAndDismount();
            return true;
        }
    }
}