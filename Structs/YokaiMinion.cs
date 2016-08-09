using Oracle.Enumerations;

namespace Oracle.Structs
{
    internal struct YokaiMinion
    {
        public YokaiMinionId MinionId { get; set; }

        public string EnglishName { get; set; }

        public uint MedalItemId { get; set; }

        public uint MedalZoneOne { get; set; }

        public uint MedalZoneTwo { get; set; }

        public uint MedalZoneThree { get; set; }

        public bool Ignored { get; set; }
    }
}