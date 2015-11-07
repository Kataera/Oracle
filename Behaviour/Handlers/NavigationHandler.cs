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

namespace Tarot.Behaviour.Handlers
{
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

    using global::Tarot.Helpers;

    using TreeSharp;

    internal static class NavigationHandler
    {
        private static bool isIdleNavigationComplete;

        public static Composite Behaviour
        {
            get
            {
                return CreateBehaviour();
            }
        }

        private static async Task<bool> HandleCustomWaypoints()
        {
            // TODO: Handle waypoints. For now always return false.
            return true;
        }

        private static async Task<bool> MoveToFate()
        {
            var fate = MainBehaviour.Instance.CurrentRbFate;

            // Sanity check before we request a path.
            if (fate.Within2D(Core.Player.Location))
            {
                return true;
            }

            // Make sure we're mounted.
            while (!Core.Player.IsMounted)
            {
                // Break behaviour if in combat.
                if (Core.Player.InCombat)
                {
                    return false;
                }

                Actionmanager.Mount();
                await Coroutine.Wait(5000, () => Core.Player.IsMounted);
            }

            // TODO: Implement specific waiting spots for certain FATEs.
            // Continually poll the navigator until it returns done or we're in the fate area.
            while (Navigator.MoveToPointWithin(fate.Location, fate.Radius, "Moving to '" + fate.Name + "'.")
                   != MoveResult.Done)
            {
                // If the FATE becomes invalid/complete while traveling, stop and break loop.
                if (!fate.IsValid || fate.Status == FateStatus.COMPLETE)
                {
                    // TODO: Check if clear stops by default.
                    Navigator.Stop();
                    Navigator.Clear();

                    return false;
                }

                // If we reached the FATE boundary.
                if (fate.Within2D(Core.Player.Location))
                {
                    Navigator.Stop();
                    Navigator.Clear();

                    return true;
                }

                Navigator.MoveToPointWithin(fate.Location, fate.Radius, "Moving to '" + fate.Name + "'.");
                await Coroutine.Yield();
            }

            return true;
        }

        private static async Task<bool> MoveToIdle()
        {
            Logger.SendDebugLog("Entered MoveToIdle coroutine.");
            var aetheryteLocation = GetClosestAetheryteLocation();
            var navResult = Navigator.MoveToPointWithin(aetheryteLocation, 20f, "Returning to Aetheryte Crystal.");

            // Continually poll the navigator until it returns done.
            while (navResult != MoveResult.Done)
            {
                navResult = Navigator.MoveToPointWithin(aetheryteLocation, 20f, "Returning to Aetheryte Crystal.");
                await Coroutine.Yield();
            }

            return true;
        }

        private static Vector3 GetClosestAetheryteLocation()
        {
            var aetherytes = WorldManager.AetheryteIdsForZone(WorldManager.ZoneId);
            var location = Vector3.Zero;

            // If there's no aetheryte, return zeroed location and handle in behaviour.
            if (aetherytes == null || aetherytes.Length == 0)
            {
                return location;
            }

            foreach (var aetheryte in aetherytes)
            {
                if (location == Vector3.Zero)
                {
                    location = aetheryte.Item2;
                }

                if (Core.Player.Location.Distance2D(location) > Core.Player.Location.Distance2D(aetheryte.Item2))
                {
                    location = aetheryte.Item2;
                }
            }

            return location;
        }

        private static Composite CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new DecoratorContinue(check => Core.Player.InCombat, CombatHandler.Behaviour),
                new ActionRunCoroutine(coroutine => HandleCustomWaypoints()),
                new DecoratorContinue(
                    check => Poi.Current.Type == PoiType.Fate,
                    new ActionRunCoroutine(coroutine => MoveToFate())),
                new DecoratorContinue(
                    check => Poi.Current.Type == PoiType.None && !isIdleNavigationComplete,
                    new ActionRunCoroutine(coroutine => MoveToIdle()))
            };

            return new Sequence(behaviours);
        }
    }
}