using Pathfinding;

namespace Oracle.Data
{
    internal class OracleFlightMesh
    {
        public OracleFlightMesh(uint zoneId, Graph graph)
        {
            this.ZoneId = zoneId;
            this.Graph = graph;
        }

        public Graph Graph { get; set; }

        public uint ZoneId { get; set; }
    }
}