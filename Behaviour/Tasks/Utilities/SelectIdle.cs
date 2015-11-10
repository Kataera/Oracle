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

namespace Tarot.Behaviour.Tasks.Utilities
{
    using System.Threading.Tasks;

    using Buddy.Coroutines;

    using Clio.Utilities;

    using ff14bot;
    using ff14bot.Behavior;
    using ff14bot.Enums;
    using ff14bot.Managers;
    using ff14bot.Navigation;

    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class SelectIdle
    {
        public static async Task<bool> Task()
        {
            // Check if the current FATE isn't null.
            if (Tarot.CurrentFate != null)
            {
                // If this occurs something is really wrong. Stop the bot.
                Logger.SendErrorLog("Entered idle selector with an active FATE assigned, stopping the bot.");
                TreeRoot.Stop("Continuing could lead to undefined behaviour.");
            }

            // Determine FATE selection strategy.
            switch (TarotSettings.Instance.FateIdleMode)
            {
                case (int) FateIdleMode.ReturnToAetheryte:
                    await ReturnToAetheryte();
                    break;

                case (int) FateIdleMode.MoveToWaitLocation:
                    await ReturnToWaitLocation();
                    break;

                case (int) FateIdleMode.GrindMobs:
                    await GrindMobs();
                    break;

                case (int) FateIdleMode.Nothing:
                    await WaitForFate();
                    break;

                default:
                    await ReturnToAetheryte();
                    Logger.SendDebugLog(
                        "Cannot determine idle selection strategy, defaulting to 'Return to Aetheryte'.");
                    break;
            }

            // We should always return false so we can reprocess the Poi.
            return false;
        }

        private static async Task<bool> ReturnToAetheryte()
        {
            var aetheryte = GetClosestAetheryteLocation();

            // Check if there were no Aetherytes in the zone.
            if (aetheryte == Vector3.Zero)
            {
                // TODO: Add check for a wait location of the zone and try that.
                // Default to no action for now.
                Logger.SendLog("There are no Aetheryte crystals in this zone, defaulting to nothing.");
                await WaitForFate();
                return true;
            }

            // Mount up if we're not already mounted.
            // TODO: Check that CreateMountBehavior handles combat.
            if (!Core.Player.IsMounted)
            {
                await CommonBehaviors.CreateMountBehavior().ExecuteCoroutine();
            }

            // Move to the Aetheryte crystal.
            Logger.SendLog("Moving to the closest Aetheryte crystal to wait for the next FATE.");
            var result = Navigator.MoveToPointWithin(aetheryte, 15f, "Moving to Aetheryte");

            // Keep polling the result until we're done.
            while (result != MoveResult.Done)
            {
                // Check if a FATE popped while we're moving.
                if (IsFateActive())
                {
                    Logger.SendLog("Found a FATE, exiting idle coroutine.");
                    Navigator.Stop();
                    Navigator.Clear();

                    return true;
                }

                // Continue moving.
                result = Navigator.MoveToPointWithin(aetheryte, 15f, "Moving to Aetheryte");

                // Make sure we yield control so we don't lock the bot.
                await Coroutine.Yield();
            }

            // Make sure we stop moving now we're there.
            Navigator.Stop();
            Navigator.Clear();

            // Wait for FATE once we're there.
            await WaitForFate();
            return true;
        }

        private static async Task<bool> ReturnToWaitLocation()
        {
            // TODO: Implement.
            Logger.SendLog("'Return to wait location' is not yet implemented, defaulting to 'Return to Aetheryte'.");
            await ReturnToAetheryte();
            return true;
        }

        private static async Task<bool> GrindMobs()
        {
            // TODO: Implement.
            Logger.SendLog("'Grind mobs' is not yet implemented, defaulting to 'Return to Aetheryte'.");
            await ReturnToAetheryte();
            return true;
        }

        private static async Task<bool> WaitForFate()
        {
            // Continually poll the FATE manager until we get a FATE.
            while (true)
            {
                if (IsFateActive())
                {
                    Logger.SendLog("Found a FATE, exiting idle coroutine.");
                    return true;
                }

                // Make sure we yield control so we don't lock the bot.
                await Coroutine.Yield();
            }
        }

        private static Vector3 GetClosestAetheryteLocation()
        {
            var aetherytes = WorldManager.AetheryteIdsForZone(WorldManager.ZoneId);
            var playerLocation = Core.Player.Location;
            var location = Vector3.Zero;

            // If there's no Aetheryte, return zeroed location and handle in task.
            if (aetherytes == null)
            {
                return location;
            }

            if (aetherytes.Length == 0)
            {
                return location;
            }

            foreach (var aetheryte in aetherytes)
            {
                // If this is the first Aetheryte we're processing, just set as closest.
                if (location == Vector3.Zero)
                {
                    location = aetheryte.Item2;
                }

                // If distance to current Aetheryte is less than current closest, set it as closest.
                if (playerLocation.Distance2D(location) > playerLocation.Distance2D(aetheryte.Item2))
                {
                    location = aetheryte.Item2;
                }
            }

            return location;
        }

        private static bool IsFateActive()
        {
            var activeFates = FateManager.ActiveFates as FateData[];
            if (activeFates != null)
            {
                if (activeFates.Length == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}