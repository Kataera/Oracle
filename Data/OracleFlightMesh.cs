/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

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

using Pathfinding;

namespace Oracle.Data
{
    internal class OracleFlightMesh
    {
        public OracleFlightMesh(uint zoneId, Graph nodeGraph)
        {
            this.ZoneId = zoneId;
            this.NodeGraph = nodeGraph;
        }

        public Graph NodeGraph { get; set; }

        public uint ZoneId { get; set; }
    }
}