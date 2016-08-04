using Pathfinding;

namespace Oracle.Data.Meshes
{
    internal class FlightMesh
    {
        public FlightMesh(uint zoneId, Graph graph)
        {
            ZoneId = zoneId;
            Graph = graph;
        }

        public Graph Graph { get; set; }

        public uint ZoneId { get; set; }
    }
}