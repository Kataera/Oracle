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

using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using Tarot.Helpers;

namespace Tarot.Behaviour.Tasks.Idles
{
    internal static class ReturnToAetheryte
    {
        public static async Task<bool> Main()
        {
            var aetheryte = GetClosestAetheryteLocation();

            // Check if there were no Aetherytes in the zone.
            if (aetheryte == Vector3.Zero)
            {
                // TODO: Add check for a wait location of the zone and try that.
                Logger.SendLog("There are no Aetheryte crystals in this zone, defaulting to nothing.");
                await WaitForFates.Main();
                return true;
            }

            if (!IsFateActive())
            {
                Logger.SendLog("Moving to the closest Aetheryte crystal to wait for the next FATE.");

                if (!Core.Player.IsMounted)
                {
                    // TODO: Check that CreateMountBehavior handles combat.
                    await CommonBehaviors.CreateMountBehavior().ExecuteCoroutine();
                }
            }

            // Set Poi and start moving.
            Poi.Current = new Poi(aetheryte, PoiType.Wait);
            Tarot.CurrentPoi = Poi.Current;

            var result = Navigator.MoveToPointWithin(aetheryte, 15f, "Moving to Aetheryte");
            while (result != MoveResult.Done)
            {
                // Check if a FATE popped while we're moving.
                if (IsFateActive())
                {
                    Navigator.Stop();
                    Poi.Clear("Found a FATE.");

                    return true;
                }

                // This needs to be continually reassigned, no other way to check status.
                result = Navigator.MoveToPointWithin(aetheryte, 15f, "Moving to Aetheryte");
                await Coroutine.Yield();
            }

            Navigator.PlayerMover.MoveStop();

            return true;
        }

        private static Vector3 GetClosestAetheryteLocation()
        {
            var aetherytes = WorldManager.AetheryteIdsForZone(WorldManager.ZoneId);
            var playerLocation = Core.Player.Location;
            var location = Vector3.Zero;

            // Vectors are non-nullable, so return zeroed location and handle in task.
            if (aetherytes == null || aetherytes.Length == 0)
            {
                return location;
            }

            foreach (var aetheryte in aetherytes)
            {
                // TODO: Check the Aetheryte is navigable first.
                if (location == Vector3.Zero
                    || playerLocation.Distance2D(location) > playerLocation.Distance2D(aetheryte.Item2))
                {
                    location = aetheryte.Item2;
                }
            }

            return location;
        }

        private static bool IsFateActive()
        {
            var activeFates = FateManager.ActiveFates;
            if (activeFates != null && activeFates.Any())
            {
                return true;
            }

            return false;
        }
    }
}