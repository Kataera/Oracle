using System;
using System.Collections.Generic;

using Oracle.Enumerations;

namespace Oracle.Data
{
    [Serializable]
    internal struct Fate
    {
        public uint ChainId { get; set; }
        public uint Id { get; set; }
        public uint ItemId { get; set; }
        public float LandingRadius { get; set; }
        public uint Level { get; set; }
        public string Name { get; set; }
        public uint NpcId { get; set; }
        public List<uint> PreferredTargetId { get; set; }
        public FateSupportLevel SupportLevel { get; set; }
        public FateType Type { get; set; }
    }
}