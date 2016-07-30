using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Common;
using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Settings;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Data;
using Oracle.Helpers;
using Oracle.Settings;

using Pathfinding;

namespace Oracle.Managers
{
    internal static class OracleMovementManager
    {
        internal static OracleFlightMesh ZoneFlightMesh { get; set; }

        private static async Task ClearExpiredFate()
        {
            OracleFateManager.DoNotWaitBeforeMovingFlag = true;
            await OracleFateManager.ClearCurrentFate("FATE ended before we got there.", false);
            Navigator.Stop();
        }

        public static async Task<bool> FlyToCurrentFate()
        {
            OracleFateManager.ReachedCurrentFate = false;
            var currentFate = OracleFateManager.GetCurrentFateData();
            if (currentFate == null || !currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
            {
                await ClearExpiredFate();
                return true;
            }

            var originalFateLocation = currentFate.Location;
            var path = await GenerateFlightPathToFate(currentFate);
            if (path == null)
            {
                return false;
            }

            if (!MovementManager.IsFlying)
            {
                await CommonTasks.TakeOff();
            }

            var enumerablePath = path as IList<Vector3> ?? path.ToList();
            foreach (var step in enumerablePath)
            {
                var processedStep = !enumerablePath.Last().Equals(step) ? ProcessFlightStep(step) : step;
                if (Core.Player.Location.Distance2D(currentFate.Location) < Core.Player.Location.Distance2D(processedStep))
                {
                    Logger.SendDebugLog("FATE centre is closer than next hop. Ending navigation early.");
                    break;
                }

                while (Core.Player.Distance(processedStep) > 2f)
                {
                    if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                    {
                        await ClearExpiredFate();
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
                        await FlyToCurrentFate();
                        return true;
                    }

                    Logger.SendLog("Flying to hop: " + processedStep);
                    Navigator.PlayerMover.MoveTowards(processedStep);
                    await Coroutine.Yield();
                }
            }

            await LandInFateArea();

            OracleFateManager.ReachedCurrentFate = true;
            return true;
        }

        public static async Task<bool> FlyToLocation(Vector3 location, float precision, bool land, bool stopOnFateSpawn)
        {
            if (!IsFlightMeshLoaded())
            {
                await NavigateToLocation(location, precision, stopOnFateSpawn);
                return true;
            }

            if (!Core.Player.IsMounted)
            {
                if (Core.Player.InCombat)
                {
                    return false;
                }

                await Mount.MountUp();
            }

            // Ensure precision isn't too strong, else we get flip-flopping.
            if (precision < 2f)
            {
                precision = 2f;
            }

            var path = await GenerateFlightPathToLocation(location);
            if (path == null)
            {
                return false;
            }

            if (!MovementManager.IsFlying)
            {
                await CommonTasks.TakeOff();
            }

            var enumerablePath = path as IList<Vector3> ?? path.ToList();
            foreach (var step in enumerablePath)
            {
                var processedStep = !enumerablePath.Last().Equals(step) ? ProcessFlightStep(step) : step;
                if (Core.Player.Location.Distance(location) < Core.Player.Location.Distance(processedStep))
                {
                    Logger.SendDebugLog("Destination is closer than next hop. Ending navigation early.");
                    break;
                }

                while (Core.Player.Location.Distance(processedStep) > 2f)
                {
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

                    if (stopOnFateSpawn && await OracleFateManager.AnyViableFates())
                    {
                        Navigator.PlayerMover.MoveStop();
                        OracleFateManager.ClearPoi("FATE found.");
                        return true;
                    }

                    Logger.SendLog("Flying to hop: " + processedStep);
                    Navigator.PlayerMover.MoveTowards(processedStep);
                    await Coroutine.Yield();
                }
            }

            // Move to destination.
            while (Core.Player.Location.Distance(location) > precision)
            {
                Navigator.PlayerMover.MoveTowards(location);
                await Coroutine.Yield();
            }

            Navigator.PlayerMover.MoveStop();

            if (land && MovementManager.IsFlying && await CommonTasks.CanLand(Core.Player.Location) == CanLandResult.Yes)
            {
                await CommonTasks.Land();
            }

            return true;
        }

        private static async Task<IEnumerable<Vector3>> GenerateFlightPathToFate(FateData currentFate)
        {
            Logger.SendLog("Generating new flight path to FATE.");
            var flightPathTimer = Stopwatch.StartNew();

            var aStar = new AStarNavigator(ZoneFlightMesh.Graph);
            var startingNode = GetClosestNodeToLocation(Core.Player.Location);
            var endingNode = GetClosestNodeToFate(currentFate);

            if (startingNode == null || endingNode == null)
            {
                Logger.SendErrorLog("Couldn't generate a flight path to the FATE, blacklisting it and selecting another.");
                Blacklist.Add(currentFate.Id, BlacklistFlags.Node, currentFate.TimeLeft, "Could not generate flight path.");
                await OracleFateManager.ClearCurrentFate("Couldn't generate a flight path to the FATE", false);
                return null;
            }

            var path = aStar.GeneratePath(startingNode.Position, endingNode.Position).ToList();

            if (!path.Any())
            {
                Logger.SendErrorLog("Couldn't generate a flight path to the FATE, blacklisting it and selecting another.");
                Blacklist.Add(currentFate.Id, BlacklistFlags.Node, currentFate.TimeLeft, "Could not generate flight path.");
                await OracleFateManager.ClearCurrentFate("Couldn't generate a flight path to the FATE", false);
                return null;
            }

            // Skip first node if we can to prevent bot-like mid-air direction change.
            if (path.Count > 1)
            {
                Vector3 collision;
                if (!WorldManager.Raycast(Core.Player.Location, path[1], out collision))
                {
                    Logger.SendDebugLog("Skipping first node, no collision detected.");
                    path.Remove(path.First());
                }
                else
                {
                    Logger.SendDebugLog("Not skipping first node, collision detected at: " + collision);
                }
            }

            flightPathTimer.Stop();
            Logger.SendDebugLog("Flight path generated in " + flightPathTimer.ElapsedMilliseconds + " ms.");

            return path;
        }

        private static async Task<IEnumerable<Vector3>> GenerateFlightPathToLocation(Vector3 destination)
        {
            Logger.SendLog("Generating new flight path to " + destination);
            var flightPathTimer = Stopwatch.StartNew();

            var aStar = new AStarNavigator(ZoneFlightMesh.Graph);
            var startingNode = GetClosestNodeToLocation(Core.Player.Location);
            var endingNode = GetClosestNodeToLocation(destination);

            if (startingNode == null || endingNode == null)
            {
                Logger.SendErrorLog("Couldn't generate a flight path to " + destination);
                return null;
            }

            var path = aStar.GeneratePath(startingNode.Position, endingNode.Position).ToList();

            if (!path.Any())
            {
                Logger.SendErrorLog("Couldn't generate a flight path to " + destination);
                return null;
            }

            // Skip first node if we can to prevent bot-like mid-air direction change.
            if (path.Count > 1)
            {
                Vector3 collision;
                if (!WorldManager.Raycast(Core.Player.Location, path[1], out collision))
                {
                    Logger.SendDebugLog("Skipping first node, no collision detected.");
                    path.Remove(path.First());
                }
                else
                {
                    Logger.SendDebugLog("Not skipping first node, collision detected at: " + collision);
                }
            }

            flightPathTimer.Stop();
            Logger.SendDebugLog("Flight path generated in " + flightPathTimer.ElapsedMilliseconds + " ms.");

            return path;
        }

        private static Vector3 GenerateRandomLocationInRadius(Vector3 location, float radius)
        {
            // Pick a location that is in the general direction we're facing.
            return MathEx.GetPointAt(location,
                                     radius * Convert.ToSingle(MathEx.Random(0.5, 0.9)),
                                     Core.Player.Heading + Convert.ToSingle(MathEx.Random(-0.4 * Math.PI, 0.4 * Math.PI)));
        }

        private static Node GetClosestNodeToFate(FateData fate)
        {
            var potentialNodes =
                ZoneFlightMesh.Graph.Nodes.OrderBy(kvp => kvp.Value.Position.Distance(fate.Location)).Where(kvp => kvp.Value.Position.Y > fate.Location.Y + 10);

            return potentialNodes.FirstOrDefault().Value;
        }

        private static Node GetClosestNodeToLocation(Vector3 location)
        {
            var potentialNodes =
                ZoneFlightMesh.Graph.Nodes.OrderBy(kvp => kvp.Value.Position.Distance(location)).Where(kvp => kvp.Value.Position.Y > location.Y + 10);

            return potentialNodes.FirstOrDefault().Value;
        }

        private static async Task<Vector3> GetFateLandingLocation()
        {
            var oracleFate = OracleFateManager.GetCurrentOracleFate();
            var currentFate = OracleFateManager.GetCurrentFateData();
            var elevatedFateLocation = currentFate.Location;
            elevatedFateLocation.Y = Core.Player.Location.Y;

            Logger.SendDebugLog("Generating a landing spot.");
            var potentialLandingLocation = GenerateRandomLocationInRadius(elevatedFateLocation, currentFate.Radius * oracleFate.LandingRadius);

            while (await CommonTasks.CanLand(potentialLandingLocation) == CanLandResult.No)
            {
                potentialLandingLocation = GenerateRandomLocationInRadius(elevatedFateLocation, currentFate.Radius * oracleFate.LandingRadius);
                await Coroutine.Yield();
            }

            // Raycast from generated location to ground to get position closer to ground.
            Vector3 collision;
            var groundVector = new Vector3(potentialLandingLocation.X, potentialLandingLocation.Y - 100, potentialLandingLocation.Z);
            if (WorldManager.Raycast(potentialLandingLocation, groundVector, out collision))
            {
                potentialLandingLocation = new Vector3(collision.X, collision.Y + Convert.ToSingle(MathEx.Random(7, 13)), collision.Z);
            }

            // Raycast to generated location from current location to check we can move there.
            if (WorldManager.Raycast(Core.Player.Location, potentialLandingLocation, out collision))
            {
                Logger.SendDebugLog("Landing spot generation failed: there's obstacles in the way.");
                return Core.Player.Location;
            }

            Logger.SendDebugLog("Landing spot generation succeeded.");
            return potentialLandingLocation;
        }

        public static bool IsFlightMeshLoaded()
        {
            return ZoneFlightMesh != null && ZoneFlightMesh.ZoneId == WorldManager.ZoneId;
        }

        public static bool IsMountNeeded(float distance)
        {
            return distance > CharacterSettings.Instance.MountDistance;
        }

        private static async Task<bool> LandInFateArea()
        {
            Navigator.PlayerMover.MoveStop();

            var landingLocation = await GetFateLandingLocation();
            if (!landingLocation.Equals(Core.Player.Location))
            {
                Logger.SendLog("Flying to " + landingLocation + " in order to land.");
                while (Core.Player.Location.Distance2D(landingLocation) > 2f)
                {
                    Navigator.PlayerMover.MoveTowards(landingLocation);
                    await Coroutine.Yield();
                }

                Navigator.PlayerMover.MoveStop();
            }

            Logger.SendLog("Attempting to land.");
            await Coroutine.Wait(TimeSpan.FromMilliseconds(MovementSettings.Instance.LandingTimeOut), () => CommonTasks.Land().IsCompleted);

            if (MovementManager.IsFlying)
            {
                Logger.SendErrorLog("Landing failed, trying another location.");
                await LandInFateArea();
                return true;
            }

            Logger.SendLog("Landing successful.");
            return true;
        }

        public static async Task<bool> LoadFlightMeshIfAvailable()
        {
            if (ZoneFlightMesh != null && ZoneFlightMesh.ZoneId == WorldManager.ZoneId)
            {
                return true;
            }

            // TODO: Place these in an enum.
            const ushort coerthasWesternHighlands = 397;
            const ushort dravanianForelands = 398;
            const ushort dravanianHinterlands = 399;
            const ushort churningMists = 400;
            const ushort seaOfClouds = 401;
            const ushort azysLla = 402;

            switch (WorldManager.ZoneId)
            {
                case coerthasWesternHighlands:
                    if (!MovementSettings.Instance.CoerthasWesternHighlandsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for Coerthas Western Highlands.");
                        return true;
                    }

                    Logger.SendLog("Loading the Coerthas Western Highlands flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;

                case dravanianForelands:
                    if (!MovementSettings.Instance.DravanianForelandsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for The Dravanian Forelands.");
                        return true;
                    }

                    Logger.SendLog("Loading The Dravanian Forelands flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                case dravanianHinterlands:
                    if (!MovementSettings.Instance.DravanianForelandsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for The Dravanian Hinterlands.");
                        return true;
                    }

                    Logger.SendLog("Loading The Dravanian Hinterlands flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                case churningMists:
                    if (!MovementSettings.Instance.ChurningMistsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for The Churning Mists.");
                        return true;
                    }

                    Logger.SendLog("Loading The Churning Mists flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                case seaOfClouds:
                    if (!MovementSettings.Instance.SeaOfCloudsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for The Sea of Clouds.");
                        return true;
                    }

                    Logger.SendLog("Loading The Sea of Clouds flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                case azysLla:
                    if (!MovementSettings.Instance.AzysLlaFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for Azys Lla.");
                        return true;
                    }

                    Logger.SendLog("Loading the Azys Lla flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                default:
                    Logger.SendDebugLog("No flight mesh available for current zone (ID: " + WorldManager.ZoneId + ").");
                    return true;
            }
        }

        public static async Task<bool> NavigateToCurrentFate(bool ignoreCombat)
        {
            OracleFateManager.ReachedCurrentFate = false;
            var currentFate = OracleFateManager.GetCurrentFateData();
            if (currentFate == null || !currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
            {
                await ClearExpiredFate();
                return true;
            }

            if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
            {
                await ClearExpiredFate();
                return true;
            }

            var cachedFateLocation = currentFate.Location;
            var currentFateRadius = currentFate.Radius;

            while (Core.Player.Distance(cachedFateLocation) > currentFateRadius * 0.6f)
            {
                if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                {
                    await ClearExpiredFate();
                    Navigator.Stop();
                    return true;
                }

                var distanceToFateBoundary = Core.Player.Location.Distance2D(cachedFateLocation) - currentFateRadius;
                if (!Core.Player.IsMounted && IsMountNeeded(distanceToFateBoundary) && Actionmanager.AvailableMounts.Any())
                {
                    Navigator.Stop();
                    if (!ignoreCombat && Core.Player.InCombat)
                    {
                        return true;
                    }

                    await Mount.MountUp();
                }

                // Throttle navigator path generation requests.
                if (cachedFateLocation.Distance2D(currentFate.Location) > 25)
                {
                    cachedFateLocation = currentFate.Location;
                }

                Navigator.MoveToPointWithin(cachedFateLocation, currentFateRadius * 0.5f, currentFate.Name);
                await Coroutine.Yield();
            }

            Navigator.Stop();
            OracleFateManager.ReachedCurrentFate = true;
            return true;
        }

        public static async Task<bool> NavigateToLocation(Vector3 location, float precision, bool stopOnFateSpawn)
        {
            while (Core.Player.Location.Distance(location) > precision)
            {
                if (!Core.Player.IsMounted && IsMountNeeded(Core.Player.Location.Distance(location)) && Actionmanager.AvailableMounts.Any())
                {
                    Navigator.Stop();
                    if (Core.Player.InCombat)
                    {
                        return true;
                    }

                    await Mount.MountUp();
                }

                if (stopOnFateSpawn && await OracleFateManager.AnyViableFates())
                {
                    Navigator.Stop();
                    OracleFateManager.ClearPoi("FATE found.");
                    return true;
                }

                Navigator.MoveTo(location);
                await Coroutine.Yield();
            }

            Navigator.Clear();
            return true;
        }

        private static Vector3 ProcessFlightStep(Vector3 step)
        {
            if (!MovementSettings.Instance.ProcessFlightPath)
            {
                return step;
            }

            var processedStep = step;
            processedStep.X += Convert.ToSingle(MathEx.Random(-2.5, 2.5));
            processedStep.Y += Convert.ToSingle(MathEx.Random(-2.5, 2.5));
            processedStep.Z += Convert.ToSingle(MathEx.Random(-2.5, 2.5));

            Vector3 collision;
            return WorldManager.Raycast(Core.Player.Location, processedStep, out collision) ? step : processedStep;
        }
    }
}