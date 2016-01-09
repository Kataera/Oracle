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

using System.Collections.Generic;

using Oracle.Enumerations;

namespace Oracle.Data
{
    internal struct Fate
    {
        public uint ChainId { get; set; }
        public uint Id { get; set; }
        public uint ItemId { get; set; }
        public uint Level { get; set; }
        public string Name { get; set; }
        public uint NpcId { get; set; }
        public List<uint> PreferredTargetId { get; set; }
        public FateSupportLevel SupportLevel { get; set; }
        public FateType Type { get; set; }
    }
}