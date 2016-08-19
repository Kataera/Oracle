using Pathfinding;

namespace Oracle.Data.Meshes
{
    internal class FlightMesh
    {
        internal FlightMesh(uint zoneId, Graph graph)
        {
            ZoneId = zoneId;
            Graph = graph;
        }

        internal Graph Graph { get; set; }

        internal uint ZoneId { get; set; }
    }
}