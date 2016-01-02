/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

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
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Common;
using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Settings;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

using Pathfinding;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class MoveToFate
    {
        public static async Task<bool> Main(bool ignoreCombat)
        {
            var currentFate = OracleManager.GetCurrentFateData();

            if (!ignoreCombat && GameObjectManager.Attackers.Any(attacker => attacker.IsValid) && !Core.Player.IsMounted)
            {
                return false;
            }

            if (!ignoreCombat && OracleSettings.Instance.TeleportIfQuicker && currentFate.IsValid)
            {
                if (await Teleport.FasterToTeleport(currentFate))
                {
                    await Coroutine.Wait(TimeSpan.FromSeconds(10), WorldManager.CanTeleport);
                    if (WorldManager.CanTeleport())
                    {
                        Logger.SendLog("Teleporting to the closest aetheryte crystal to the FATE.");
                        await Teleport.TeleportToClosestAetheryte(currentFate);

                        if (GameObjectManager.Attackers.Any(attacker => attacker.IsValid))
                        {
                            OracleManager.ClearPoi("We're under attack and can't teleport.");
                            return false;
                        }
                    }
                    else
                    {
                        Logger.SendLog("Timed out trying to teleport, running to FATE instead.");
                    }
                }
            }

            if (!ignoreCombat && IsMountNeeded() && !Core.Player.IsMounted && currentFate.IsValid)
            {
                await Mount.MountUp();
            }

            if (!WorldManager.CanFly || OracleManager.ZoneFlightMesh == null)
            {
                await MoveWithNavigator(ignoreCombat);
            }
            else
            {
                await MoveWithFlightMesh();
            }

            return true;
        }

        private static async Task ClearFate()
        {
            OracleManager.SetDoNotWaitFlag(true);
            await OracleManager.ClearCurrentFate("FATE ended before we got there.", false);
            Navigator.Stop();
        }

        private static Vector3 GenerateLandingSpot()
        {
            var currentFate = OracleManager.GetCurrentFateData();
            return MathEx.GetPointAt(currentFate.Location, currentFate.Radius * Convert.ToSingle(MathEx.Random(0.5, 1)),
                Core.Player.Heading + Convert.ToSingle(MathEx.Random(-0.25 * Math.PI, 0.25 * Math.PI)));
        }

        private static Node GetClosestNodeToLocation(Vector3 location)
        {
            return OracleManager.ZoneFlightMesh.Graph.Nodes
                                .OrderBy(kvp => kvp.Value.Position.Distance(location))
                                .FirstOrDefault(kvp => kvp.Value.Position.Y > location.Y).Value;
        }

        private static bool IsMountNeeded()
        {
            var currentFate = OracleManager.GetCurrentFateData();

            if (currentFate == null || !currentFate.IsValid)
            {
                return false;
            }

            var distanceToFateBoundary = Core.Player.Distance(currentFate.Location) - currentFate.Radius;
            return distanceToFateBoundary > CharacterSettings.Instance.MountDistance;
        }

        private static async Task<bool> Land()
        {
            var landingSpot = GenerateLandingSpot();
            while (await CommonTasks.CanLand(landingSpot) != CanLandResult.Yes)
            {
                landingSpot = GenerateLandingSpot();
                await Coroutine.Yield();
            }

            Logger.SendLog("Attempting to land at: " + landingSpot);
            while (Core.Player.Distance2D(landingSpot) > 2f)
            {
                Navigator.PlayerMover.MoveTowards(landingSpot);
                await Coroutine.Yield();
            }

            Navigator.PlayerMover.MoveStop();
            await CommonTasks.Land();

            if (MovementManager.IsFlying)
            {
                return false;
            }

            Logger.SendLog("Landing successful.");
            return true;
        }

        private static async Task<bool> MoveWithFlightMesh()
        {
            var currentFate = OracleManager.GetCurrentFateData();
            if (currentFate == null || !currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE
                || currentFate.Status == FateStatus.NOTACTIVE)
            {
                await ClearFate();
                return true;
            }

            Logger.SendLog("Generating new flight path to FATE.");

            if (!MovementManager.IsFlying)
            {
                await CommonTasks.TakeOff();
            }

            var originalFateLocation = currentFate.Location;
            var aStar = new AStarNavigator(OracleManager.ZoneFlightMesh.Graph);
            var closestNode = GetClosestNodeToLocation(originalFateLocation);
            var path = aStar.GeneratePath(Core.Player.Location, closestNode.Position);

            foreach (var step in path)
            {
                while (Core.Player.Distance(step) > 1f)
                {
                    if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                    {
                        await ClearFate();
                        Navigator.PlayerMover.MoveStop();
                        return true;
                    }

                    if (!Core.Player.IsMounted && Actionmanager.AvailableMounts.Any())
                    {
                        Navigator.PlayerMover.MoveStop();
                        if (Core.Player.InCombat)
                        {
                            return true;
                        }

                        await Mount.MountUp();
                    }

                    if (!MovementManager.IsFlying)
                    {
                        Navigator.PlayerMover.MoveStop();
                        await CommonTasks.TakeOff();
                    }

                    // Did FATE move?
                    if (currentFate.Location.Distance(originalFateLocation) > 50f)
                    {
                        Logger.SendDebugLog("FATE has moved significantly, recalculating flight path.");
                        await MoveWithFlightMesh();
                        return true;
                    }

                    Logger.SendLog("Flying to hop: " + step);
                    Navigator.PlayerMover.MoveTowards(step);
                    await Coroutine.Yield();
                }
            }

            while (MovementManager.IsFlying)
            {
                await Land();
            }

            return true;
        }

        private static async Task<bool> MoveWithNavigator(bool ignoreCombat)
        {
            var currentFate = OracleManager.GetCurrentFateData();
            if (currentFate == null || !currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE
                || currentFate.Status == FateStatus.NOTACTIVE)
            {
                await ClearFate();
                return true;
            }

            if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
            {
                await ClearFate();
                return true;
            }

            var currentFateLocation = currentFate.Location;
            var currentFateRadius = currentFate.Radius;

            while (Core.Player.Distance(currentFateLocation) > currentFateRadius * 0.75f)
            {
                if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                {
                    await ClearFate();
                    Navigator.Stop();
                    return true;
                }

                if (!Core.Player.IsMounted && IsMountNeeded() && Actionmanager.AvailableMounts.Any())
                {
                    Navigator.Stop();
                    if (!ignoreCombat && Core.Player.InCombat)
                    {
                        return true;
                    }

                    await Mount.MountUp();
                }

                currentFateLocation = currentFate.Location;
                Navigator.MoveToPointWithin(currentFateLocation, currentFateRadius * 0.5f, currentFate.Name);
                await Coroutine.Yield();
            }

            Navigator.Stop();
            return true;
        }
    }
}