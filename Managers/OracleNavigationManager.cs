using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Managers;
using ff14bot.Navigation;
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

        public static async Task<bool[]> AreLocationsNavigable(Vector3[] locations)
        {
            var navRequest = new List<CanFullyNavigateTarget>();
            uint index = 0;
            foreach (var location in locations)
            {
                // Don't send too many requests.
                if (index > 7)
                {
                    continue;
                }

                navRequest.Add(new CanFullyNavigateTarget
                {
                    Id = index,
                    Position = location
                });
                index++;
            }

            var navTask = Navigator.NavigationProvider.CanFullyNavigateTo(navRequest, Core.Player.Location, WorldManager.ZoneId);
            var navResults = await Coroutine.ExternalTask(navTask);

            return navResults.Select(navResult => navResult.CanNavigate != 0).ToArray();
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
            var locations = aetherytes.Select(aetheryte => aetheryte.Item2).ToArray();

            var navigableResults = await AreLocationsNavigable(locations);

            var navigableAetherytes = new List<Tuple<uint, Vector3>>();
            for (var i = 0; i < navigableResults.Length; i++)
            {
                if (navigableResults[i])
                {
                    navigableAetherytes.Add(GetAetheryteByLocation(locations[i]));
                }
            }

            navigableAetherytes.Sort((x, y) => Core.Player.Location.Distance2D(x.Item2).CompareTo(Core.Player.Location.Distance2D(y.Item2)));
            return navigableAetherytes.FirstOrDefault();
        }

        public static async Task<bool> IsLocationNavigable(Vector3 location)
        {
            var navRequest = new List<CanFullyNavigateTarget>
            {
                new CanFullyNavigateTarget
                {
                    Id = 0,
                    Position = location
                }
            };

            var navTask = Navigator.NavigationProvider.CanFullyNavigateTo(navRequest, Core.Player.Location, WorldManager.ZoneId);
            var navResults = await Coroutine.ExternalTask(navTask);

            var result = navResults.FirstOrDefault();
            if (result == null)
            {
                return false;
            }

            return result.CanNavigate != 0;
        }
    }
}