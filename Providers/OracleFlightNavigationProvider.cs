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
        private readonly AStarz aStar;
        private readonly Graph graph;
        private readonly INavigationProvider groundNavigationProvider;
        private List<Vector3> currentPath;
        private Vector3 targetLocation;

        public OracleFlightNavigationProvider(Graph graph)
        {
            this.groundNavigationProvider = new GaiaNavigator();
            this.graph = graph;
            this.aStar = new AStarz(this.graph) {ChoosenHeuristic = AStarz.ManhattanHeuristic};
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
            return this.groundNavigationProvider.CanFullyNavigateToAsync(targets, start, zoneid);
        }

        [Obsolete("CanNavigateFully is deprecated, please use CanFullyNavigateToAsync instead.")]
        public bool CanNavigateFully(Vector3 @from, Vector3 to, float strictDistance)
        {
            return this.groundNavigationProvider.CanNavigateFully(@from, to, strictDistance);
        }

        public bool Clear()
        {
            this.targetLocation = Vector3.Zero;
            this.currentPath = null;

            return this.groundNavigationProvider.Clear();
        }

        public Vector3 GenerateRandomSpotWithinRadius(Vector3 location, float radius)
        {
            var rng = new Random();
            var radiusSquared = radius * radius;
            var xOffset = Convert.ToSingle(((2 * rng.NextDouble()) - 1.0) * radius);
            var zOffset = Convert.ToSingle(((2 * rng.NextDouble()) - 1.0) * Math.Sqrt(radiusSquared - (xOffset * xOffset)));

            return new Vector3(location.X + xOffset, location.Y, location.Z + zOffset);
        }

        public MoveResult MoveTo(Vector3 location, string destination = null)
        {
            if (this.aStar.SearchStarted && !this.aStar.SearchEnded)
            {
                return MoveResult.GeneratingPath;
            }

            if (!WorldManager.CanFly)
            {
                return this.groundNavigationProvider.MoveTo(location, destination);
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
            if (this.targetLocation == Vector3.Zero)
            {
                this.targetLocation = location;
            }

            if (currentLocation.Distance(this.targetLocation) > this.PathPrecision)
            {
                this.currentPath = this.GeneratePath(currentLocation, this.targetLocation).ToList();

                if (this.currentPath == null)
                {
                    return MoveResult.GeneratingPath;
                }

                if (!this.currentPath.Any())
                {
                    return MoveResult.ReachedDestination;
                }

                return this.MoveToNextHop();
            }

            Navigator.PlayerMover.MoveStop();
            this.currentPath = null;
            this.targetLocation = Vector3.Zero;

            return MoveResult.Done;
        }

        public MoveResult MoveToRandomSpotWithin(Vector3 location, float radius, string destination = null)
        {
            if (this.aStar.SearchStarted && !this.aStar.SearchEnded)
            {
                return MoveResult.GeneratingPath;
            }

            if (!WorldManager.CanFly)
            {
                return this.groundNavigationProvider.MoveToRandomSpotWithin(location, radius, destination);
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
            if (this.targetLocation == Vector3.Zero)
            {
                this.targetLocation = this.GenerateRandomSpotWithinRadius(location, radius);
            }

            if (currentLocation.Distance(this.targetLocation) > this.PathPrecision)
            {
                this.currentPath = this.GeneratePath(currentLocation, this.targetLocation).ToList();

                if (this.currentPath == null)
                {
                    return MoveResult.GeneratingPath;
                }

                if (!this.currentPath.Any())
                {
                    return MoveResult.ReachedDestination;
                }

                return this.MoveToNextHop();
            }

            Navigator.PlayerMover.MoveStop();
            this.currentPath = null;
            this.targetLocation = Vector3.Zero;

            return MoveResult.Done;
        }

        private IEnumerable<Vector3> GeneratePath(Vector3 from, Vector3 to)
        {
            var path = new List<Vector3>();

            var currentNode = this.graph.CloestNode(from);
            var targetNode =
                this.graph.Nodes.Where(kvp => kvp.Value.Position.Y > to.Y)
                    .OrderBy(kvp => kvp.Value.Position.Distance(to))
                    .FirstOrDefault()
                    .Value;

            this.aStar.Initialize(currentNode, targetNode);
            if (!this.aStar.SearchEnded)
            {
                Logger.SendLog("Generating flight path.");
                return null;
            }

            path.Add(currentNode.Position);
            path.AddRange(this.aStar.PathByCoordinates);
            path.Add(to);

            return path;
        }

        private MoveResult MoveToNextHop()
        {
            var currentLocation = Core.Player.Location;
            var currentStep = this.currentPath.FirstOrDefault();

            if (currentStep.Distance(currentLocation) > this.PathPrecision)
            {
                Navigator.PlayerMover.MoveTowards(this.currentPath.FirstOrDefault());
            }
            else if (this.currentPath.Count > 1)
            {
                this.currentPath.Remove(currentStep);
            }
            else
            {
                return MoveResult.ReachedDestination;
            }

            return MoveResult.Moved;
        }
    }
}