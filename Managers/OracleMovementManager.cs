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
using Oracle.Data.Meshes;
using Oracle.Helpers;
using Oracle.Settings;

using Pathfinding;

namespace Oracle.Managers
{
    internal static class OracleMovementManager
    {
        private const ushort CoerthasWesternHighlandsZoneId = 397;
        private const ushort DravanianForelandsZoneId = 398;
        private const ushort DravanianHinterlandsZoneId = 399;
        private const ushort ChurningMistsZoneId = 400;
        private const ushort SeaOfCloudsZoneId = 401;
        private const ushort AzysLlaZoneId = 402;

        internal static FlightMesh ZoneFlightMesh { get; set; }

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

                        await MountUp();
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

                await MountUp();
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

                        await MountUp();
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

        private static async Task<Vector3> GenerateRandomLocationInRadius(Vector3 location, float radius)
        {
            var potentialSpots = new List<Vector3>();

            // Generate locations in the direction we're facing.
            for (var i = 0; i < 10; i++)
            {
                potentialSpots.Add(MathEx.GetPointAt(location,
                                                     radius * Convert.ToSingle(MathEx.Random(0.5, 0.9)),
                                                     Core.Player.Heading + Convert.ToSingle(MathEx.Random(-0.4 * Math.PI, 0.4 * Math.PI))));
            }

            var bestSpot = Vector3.Zero;
            float bestScore = 0;
            foreach (var spot in potentialSpots)
            {
                var closestEnemies =
                    GameObjectManager.GameObjects.OrderBy(enemy => enemy.Distance2D(spot)).Where(enemy => enemy.Type == GameObjectType.BattleNpc).Take(10);

                var currentScore = closestEnemies.Sum(enemy => enemy.Distance2D(spot));
                if (!(currentScore > bestScore))
                {
                    continue;
                }

                bestSpot = spot;
                bestScore = currentScore;
            }

            return bestSpot;
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

            var potentialLandingLocation = await GenerateRandomLocationInRadius(elevatedFateLocation, currentFate.Radius * oracleFate.LandingRadius);
            if (await CommonTasks.CanLand(potentialLandingLocation) == CanLandResult.No)
            {
                Logger.SendDebugLog("Landing spot generation failed: we can't land at the proposed spot.");
                return Core.Player.Location;
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

            switch (WorldManager.ZoneId)
            {
                case CoerthasWesternHighlandsZoneId:
                    if (!MovementSettings.Instance.CoerthasWesternHighlandsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for Coerthas Western Highlands.");
                        return true;
                    }

                    Logger.SendLog("Loading the Coerthas Western Highlands flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;

                case DravanianForelandsZoneId:
                    if (!MovementSettings.Instance.DravanianForelandsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for The Dravanian Forelands.");
                        return true;
                    }

                    Logger.SendLog("Loading The Dravanian Forelands flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                case DravanianHinterlandsZoneId:
                    if (!MovementSettings.Instance.DravanianForelandsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for The Dravanian Hinterlands.");
                        return true;
                    }

                    Logger.SendLog("Loading The Dravanian Hinterlands flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                case ChurningMistsZoneId:
                    if (!MovementSettings.Instance.ChurningMistsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for The Churning Mists.");
                        return true;
                    }

                    Logger.SendLog("Loading The Churning Mists flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                case SeaOfCloudsZoneId:
                    if (!MovementSettings.Instance.SeaOfCloudsFlight)
                    {
                        Logger.SendDebugLog("Flight mesh is available, but disabled for The Sea of Clouds.");
                        return true;
                    }

                    Logger.SendLog("Loading The Sea of Clouds flight mesh.");
                    await LoadFlightMesh.Main();
                    return true;
                case AzysLlaZoneId:
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

        public static async Task<bool> MountUp()
        {
            if (!Actionmanager.AvailableMounts.Any())
            {
                Logger.SendDebugLog("Character does not have any mount available, skipping mount task.");
                return true;
            }

            if (MovementManager.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }

            while (!Core.Player.IsMounted)
            {
                if (GameObjectManager.Attackers.Any())
                {
                    return false;
                }

                if (Actionmanager.CanMount == 0)
                {
                    Actionmanager.Mount();
                }

                await Coroutine.Yield();
            }

            return true;
        }

        public static async Task<bool> MoveOutOfIdyllshire()
        {
            Logger.SendLog("We're in Idyllshire, moving to The Dravanian Hinterlands.");
            await MountUp();

            var location = new Vector3(142.6006f, 207f, 114.136f);
            while (Core.Player.Distance(location) > 5f)
            {
                Navigator.MoveTo(location, "Leaving Idyllshire");
                await Coroutine.Yield();
            }

            Navigator.Stop();
            Core.Player.SetFacing(0.9709215f);
            MovementManager.MoveForwardStart();

            await Coroutine.Sleep(TimeSpan.FromSeconds(2));
            await Coroutine.Wait(TimeSpan.MaxValue, () => !CommonBehaviors.IsLoading);
            await Coroutine.Sleep(TimeSpan.FromSeconds(2));

            return true;
        }

        public static async Task<bool> MoveToCurrentFate(bool ignoreCombat)
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (!ignoreCombat && GameObjectManager.Attackers.Any(attacker => attacker.IsValid) && !Core.Player.IsMounted)
            {
                return false;
            }

            await LoadFlightMeshIfAvailable();

            if (!ignoreCombat && MovementSettings.Instance.TeleportIfQuicker && currentFate.IsValid)
            {
                if (await OracleTeleportManager.FasterToTeleport(currentFate))
                {
                    if (GameObjectManager.Attackers.Any(attacker => attacker.IsValid))
                    {
                        OracleFateManager.ClearPoi("We're under attack and can't teleport.");
                        return false;
                    }

                    await Coroutine.Wait(TimeSpan.FromSeconds(10), WorldManager.CanTeleport);
                    if (WorldManager.CanTeleport())
                    {
                        Logger.SendLog("Teleporting to the closest aetheryte crystal to the FATE.");
                        await OracleTeleportManager.TeleportToClosestAetheryte(currentFate);

                        if (GameObjectManager.Attackers.Any(attacker => attacker.IsValid))
                        {
                            OracleFateManager.ClearPoi("We're under attack and can't teleport.");
                            return false;
                        }
                    }
                    else
                    {
                        Logger.SendErrorLog("Timed out trying to teleport, running to FATE instead.");
                    }
                }
            }

            var distanceToFateBoundary = Core.Player.Location.Distance2D(currentFate.Location) - currentFate.Radius;
            if (!ignoreCombat && IsMountNeeded(distanceToFateBoundary) && !Core.Player.IsMounted && currentFate.IsValid)
            {
                await MountUp();
            }

            if (!ignoreCombat && WorldManager.CanFly && ZoneFlightMesh != null)
            {
                await FlyToCurrentFate();
            }
            else
            {
                await NavigateToCurrentFate(ignoreCombat);
            }

            return true;
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

            while (Core.Player.Distance(cachedFateLocation) > currentFateRadius * 0.65f)
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

                    await MountUp();
                }

                // Throttle navigator path generation requests.
                if (cachedFateLocation.Distance2D(currentFate.Location) > 10)
                {
                    cachedFateLocation = currentFate.Location;
                }

                Navigator.MoveToPointWithin(cachedFateLocation, currentFateRadius * 0.3f, currentFate.Name);
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

                    await MountUp();
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