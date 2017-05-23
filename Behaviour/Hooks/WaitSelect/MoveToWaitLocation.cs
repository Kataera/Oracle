using System;
using System.Linq;
using System.Threading.Tasks;

using Clio.Utilities;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Hooks.WaitSelect
{
    internal static class MoveToWaitLocation
    {
        private static async Task<bool> BlacklistUnnavigableLocation()
        {
            if (!WorldManager.CanFly)
            {
                if (!WaitSettings.Instance.FateWaitLocations.ContainsKey(WorldManager.ZoneId))
                {
                    Logger.SendErrorLog("No wait location has been set. Check the 'downtime' section under 'General Settings' and set the location you wish.");
                    return false;
                }

                var locations = WaitSettings.Instance.FateWaitLocations;
                var navRequest =
                    locations.Select(target => new CanFullyNavigateTarget
                             {
                                 Id = target.Key,
                                 Position = target.Value
                             })
                             .Where(target => target.Id == WorldManager.ZoneId);
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate == 0))
                {
                    var val = locations.FirstOrDefault(result => result.Key == navResult.Id);
                    Blacklist.Add(val.Key, BlacklistFlags.Node, TimeSpan.FromMinutes(10), "Cannot navigate to wait location.");
                }
            }

            return true;
        }

        private static Vector3 GetWaitLocation()
        {
            var location = Vector3.Zero;

            // Vectors are non-nullable, so return zeroed location and handle in task.
            if (!WaitSettings.Instance.FateWaitLocations.ContainsKey(WorldManager.ZoneId))
            {
                return location;
            }

            if (!Blacklist.Contains(WorldManager.ZoneId))
            {
                location = WaitSettings.Instance.FateWaitLocations.FirstOrDefault(result => result.Key == WorldManager.ZoneId).Value;
            }

            return location;
        }

        internal static async Task<bool> Main()
        {
            await BlacklistUnnavigableLocation();
            var waitLocation = GetWaitLocation();

            // If there's no wait location, wait where we are.
            if (waitLocation == Vector3.Zero)
            {
                Logger.SendLog("There's no available wait location, either because you haven't set it or it's unreachable, waiting at current location.");
                Poi.Current = new Poi(Core.Player.Location, PoiType.Wait);
            }
            else
            {
                Logger.SendLog("Moving to designated waiting location.");
                Poi.Current = new Poi(waitLocation, PoiType.Wait);
            }

            return true;
        }
    }
}