﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Clio.Utilities;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Hooks.WaitSelect
{
    internal static class ReturnToAetheryte
    {
        private static async Task<bool> BlacklistUnnavigableAetherytes()
        {
            if (WorldManager.CanFly)
            {
                return true;
            }

            var aetherytes = OracleFateManager.GetAetheryteIdsForZone(WorldManager.ZoneId);
            var navRequest = aetherytes.Select(target => new CanFullyNavigateTarget
            {
                Id = target.Item1,
                Position = target.Item2
            });
            var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

            foreach (var navResult in navResults.Where(result => result.CanNavigate == 0))
            {
                var aetheryte = aetherytes.FirstOrDefault(result => result.Item1 == navResult.Id);
                if (aetheryte != null)
                {
                    Blacklist.Add(aetheryte.Item1, BlacklistFlags.Node, TimeSpan.FromMinutes(10), "Cannot navigate to aetheryte crystal.");
                }
            }

            return true;
        }

        private static Vector3 GetClosestAetheryteLocation()
        {
            var aetherytes = OracleFateManager.GetAetheryteIdsForZone(WorldManager.ZoneId);
            var playerLocation = Core.Player.Location;
            var location = Vector3.Zero;

            // Vectors are non-nullable, so return zeroed location and handle in task.
            if (aetherytes == null || aetherytes.Length == 0)
            {
                return location;
            }

            foreach (var aetheryte in aetherytes)
            {
                if (!Blacklist.Contains(aetheryte.Item1)
                    && (location == Vector3.Zero || playerLocation.Distance2D(location) > playerLocation.Distance2D(aetheryte.Item2)))
                {
                    location = aetheryte.Item2;
                }
            }

            return location;
        }

        public static async Task<bool> Main()
        {
            await BlacklistUnnavigableAetherytes();
            var aetheryteLocation = GetClosestAetheryteLocation();

            // If there's no viable aetheryte, wait where we are.
            if (aetheryteLocation == Vector3.Zero)
            {
                Logger.SendLog("There's no aetheryte crystal in this zone that is reachable, waiting at current location.");
                Poi.Current = new Poi(Core.Player.Location, PoiType.Wait);
            }
            else
            {
                Logger.SendLog("Moving to closest reachable aetheryte crystal.");
                Poi.Current = new Poi(aetheryteLocation, PoiType.Wait);
            }

            return true;
        }
    }
}