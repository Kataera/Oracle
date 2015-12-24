using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Clio.Utilities;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Interfaces;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Oracle.Helpers;

using Pathfinding;

namespace Oracle.Providers
{
    public class OracleFlightNavigationProvider : INavigationProvider
    {
        private static AStarz aStar;
        private static List<Vector3> currentPath;
        private static Dictionary<uint, Graph> graphDictionary;
        private static INavigationProvider groundNavigationProvider;
        private static Vector3 targetLocation;

        public OracleFlightNavigationProvider()
        {
            groundNavigationProvider = new GaiaNavigator();
            this.LoadZoneGraph(WorldManager.ZoneId);
        }

        public float PathPrecision { get; set; }

        public List<CanFullyNavigateResult> CanFullyNavigateTo(IEnumerable<CanFullyNavigateTarget> targets)
        {
            return this.CanFullyNavigateToAsync(targets, Core.Player.Location, WorldManager.ZoneId).Result;
        }

        public async Task<List<CanFullyNavigateResult>> CanFullyNavigateToAsync(IEnumerable<CanFullyNavigateTarget> targets)
        {
            return await this.CanFullyNavigateToAsync(targets, Core.Player.Location, WorldManager.ZoneId);
        }

        public Task<List<CanFullyNavigateResult>> CanFullyNavigateToAsync(
            IEnumerable<CanFullyNavigateTarget> targets,
            Vector3 start,
            ushort zoneid)
        {
            return groundNavigationProvider.CanFullyNavigateToAsync(targets, start, zoneid);
        }

        [Obsolete("CanNavigateFully is deprecated, please use CanFullyNavigateToAsync instead.")]
        public bool CanNavigateFully(Vector3 @from, Vector3 to, float strictDistance)
        {
            return groundNavigationProvider.CanNavigateFully(@from, to, strictDistance);
        }

        public bool Clear()
        {
            return false;
        }

        public Vector3 GenerateRandomSpotWithinRadius(Vector3 location, float radius)
        {
            var radiusSquared = radius * radius;
            var xOffset = Convert.ToSingle(((2 * new Random().NextDouble()) - 1.0) * radius);
            var zOffset = Convert.ToSingle(((2 * new Random().NextDouble()) - 1.0) * Math.Sqrt(radiusSquared - (xOffset * xOffset)));

            return new Vector3(location.X + xOffset, location.Y, location.Z + zOffset);
        }

        public void LoadZoneGraph(uint zoneId)
        {
            aStar = new AStarz(this.ZoneGraph(zoneId)) {ChoosenHeuristic = AStarz.ManhattanHeuristic};
        }

        public MoveResult MoveTo(Vector3 location, string destination = null)
        {
            if (aStar.SearchStarted && !aStar.SearchEnded)
            {
                return MoveResult.GeneratingPath;
            }

            if (!WorldManager.CanFly)
            {
                return groundNavigationProvider.MoveTo(location, destination);
            }

            if (location.Equals(Vector3.Zero))
            {
                return MoveResult.Failed;
            }

            if (!Core.Player.IsMounted)
            {
                if (Actionmanager.CanMount == 0)
                {
                    Actionmanager.Mount();
                }

                return MoveResult.Failure;
            }

            var currentLocation = Core.Player.Location;
            if (targetLocation == Vector3.Zero)
            {
                targetLocation = location;
            }

            if (currentLocation.Distance(targetLocation) > this.PathPrecision)
            {
                currentPath = this.GeneratePath(currentLocation, targetLocation).ToList();

                if (currentPath == null)
                {
                    return MoveResult.GeneratingPath;
                }

                if (!currentPath.Any())
                {
                    return MoveResult.ReachedDestination;
                }

                return this.MoveToNextHop();
            }

            Navigator.PlayerMover.MoveStop();
            currentPath = null;
            targetLocation = Vector3.Zero;

            return MoveResult.Done;
        }

        public MoveResult MoveToRandomSpotWithin(Vector3 location, float radius, string destination = null)
        {
            if (aStar.SearchStarted && !aStar.SearchEnded)
            {
                return MoveResult.GeneratingPath;
            }

            if (!WorldManager.CanFly)
            {
                return groundNavigationProvider.MoveToRandomSpotWithin(location, radius, destination);
            }

            if (location.Equals(Vector3.Zero))
            {
                return MoveResult.Failed;
            }

            if (!Core.Player.IsMounted)
            {
                if (Actionmanager.CanMount == 0)
                {
                    Actionmanager.Mount();
                }

                return MoveResult.Failure;
            }

            var currentLocation = Core.Player.Location;
            if (targetLocation == Vector3.Zero)
            {
                targetLocation = this.GenerateRandomSpotWithinRadius(location, radius);
            }

            if (currentLocation.Distance(targetLocation) > this.PathPrecision)
            {
                currentPath = this.GeneratePath(currentLocation, targetLocation).ToList();

                if (currentPath == null)
                {
                    return MoveResult.GeneratingPath;
                }

                if (!currentPath.Any())
                {
                    return MoveResult.ReachedDestination;
                }

                return this.MoveToNextHop();
            }

            Navigator.PlayerMover.MoveStop();
            currentPath = null;
            targetLocation = Vector3.Zero;

            return MoveResult.Done;
        }

        public Graph ZoneGraph(uint zoneId)
        {
            Graph graph;
            graphDictionary.TryGetValue(zoneId, out graph);

            return graph;
        }

        private IEnumerable<Vector3> GeneratePath(Vector3 from, Vector3 to)
        {
            var path = new List<Vector3>();

            var currentNode = this.ZoneGraph(WorldManager.ZoneId).CloestNode(from);
            var targetNode = this.ZoneGraph(WorldManager.ZoneId).CloestNode(to);

            aStar.Initialize(currentNode, targetNode);
            if (!aStar.SearchEnded)
            {
                Logger.SendLog("Generating flight path.");
                return null;
            }

            path.Add(currentNode.Position);
            path.AddRange(aStar.PathByCoordinates);
            path.Add(to);

            return path;
        }

        private MoveResult MoveToNextHop()
        {
            var currentLocation = Core.Player.Location;
            var currentStep = currentPath.FirstOrDefault();

            if (currentStep.Distance(currentLocation) > this.PathPrecision)
            {
                Navigator.PlayerMover.MoveTowards(currentPath.FirstOrDefault());
            }
            else if (currentPath.Count > 1)
            {
                currentPath.Remove(currentStep);
            }
            else
            {
                return MoveResult.ReachedDestination;
            }

            return MoveResult.Moved;
        }
    }
}