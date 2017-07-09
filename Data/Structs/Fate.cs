using System;

using Oracle.Enumerations;

namespace Oracle.Data.Structs
{
    [Serializable]
    internal struct Fate
    {
        public uint ChainId { get; set; }

        public uint Id { get; set; }

        public uint ItemId { get; set; }

        public uint Level { get; set; }

        public string Name { get; set; }

        public uint PreferredTargetId { get; set; }

        public SupportLevel SupportLevel { get; set; }

        public FateType Type { get; set; }
    }
}