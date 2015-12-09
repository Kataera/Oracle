/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

using Tarot.Enumerations;

namespace Tarot.Data.FateTypes
{
    internal abstract class Fate
    {
        protected Fate()
        {
            ChainIdFailure = 0;
            ChainIdSuccess = 0;
            Id = 0;
            ItemId = 0;
            Level = 0;
            Name = string.Empty;
            NpcId = 0;
            SupportLevel = FateSupportLevel.Unsupported;
            Type = FateType.Null;
        }

        public uint ChainIdFailure { get; set; }

        public uint ChainIdSuccess { get; set; }

        public uint Id { get; set; }

        public uint ItemId { get; set; }

        public uint Level { get; set; }

        public string Name { get; set; }

        public uint NpcId { get; set; }

        public FateSupportLevel SupportLevel { get; set; }

        public FateType Type { get; set; }
    }
}