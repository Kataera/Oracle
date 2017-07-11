using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;

using Clio.Utilities;

using ff14bot.Enums;
using ff14bot.Helpers;

using Oracle.Enumerations;

namespace Oracle.Settings
{
    internal sealed class OracleSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile OracleSettings instance;

        private int actionDelay;
        private List<uint> blacklistedFates;
        private List<uint> blacklistedMobs;
        private Dictionary<uint, ClassJobType> classGearsets;
        private IdleBehaviour idleBehaviour;
        private Dictionary<uint, Vector3> idleLocations;
        private bool levelArcanist;
        private bool levelArcher;
        private bool levelAstrologian;
        private bool levelConjurer;
        private bool levelDarkKnight;
        private bool levelGladiator;
        private bool levelLancer;
        private bool levelMachinist;
        private bool levelMarauder;
        private bool levelPugilist;
        private bool levelRedMage;
        private bool levelRogue;
        private bool levelSamurai;
        private bool levelThaumaturge;
        private int maxLevel;
        private int mobGrindMaxLevel;
        private int mobGrindMinLevel;
        private OracleFateMode oracleFateMode;
        private bool prioritiseForlornMaiden;
        private bool prioritiseTheForlorn;
        private bool showDebug;

        private OracleSettings() : base(Path.Combine(SettingsPath, @"Oracle\OracleSettings.json"))
        {
            if (BlacklistedFates == null)
            {
                BlacklistedFates = new List<uint>();
                Save();
            }

            if (BlacklistedMobs == null)
            {
                BlacklistedMobs = new List<uint>();
                PopulateMobBlacklist();
                Save();
            }

            if (ClassGearsets == null)
            {
                ClassGearsets = new Dictionary<uint, ClassJobType>();
                Save();
            }

            if (IdleLocations == null)
            {
                IdleLocations = new Dictionary<uint, Vector3>();
                Save();
            }
        }

        [DefaultValue(1500)]
        [Setting]
        internal int ActionDelay
        {
            get => actionDelay;

            set
            {
                actionDelay = value;
                Save();
            }
        }

        [Setting]
        internal List<uint> BlacklistedFates
        {
            get => blacklistedFates;

            set
            {
                blacklistedFates = value;
                Save();
            }
        }

        [Setting]
        internal List<uint> BlacklistedMobs
        {
            get => blacklistedMobs;

            set
            {
                blacklistedMobs = value;
                Save();
            }
        }

        [Setting]
        public Dictionary<uint, ClassJobType> ClassGearsets
        {
            get => classGearsets;

            set
            {
                classGearsets = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        internal IdleBehaviour IdleBehaviour
        {
            get => idleBehaviour;

            set
            {
                idleBehaviour = value;
                Save();
            }
        }

        [Setting]
        internal Dictionary<uint, Vector3> IdleLocations
        {
            get => idleLocations;

            set
            {
                idleLocations = value;
                Save();
            }
        }

        internal static OracleSettings Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new OracleSettings();
                    }
                }

                return instance;
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelArcanist
        {
            get => levelArcanist;
            set
            {
                levelArcanist = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelArcher
        {
            get => levelArcher;
            set
            {
                levelArcher = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelAstrologian
        {
            get => levelAstrologian;
            set
            {
                levelAstrologian = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelConjurer
        {
            get => levelConjurer;
            set
            {
                levelConjurer = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelDarkKnight
        {
            get => levelDarkKnight;
            set
            {
                levelDarkKnight = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelGladiator
        {
            get => levelGladiator;
            set
            {
                levelGladiator = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelLancer
        {
            get => levelLancer;
            set
            {
                levelLancer = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelMachinist
        {
            get => levelMachinist;
            set
            {
                levelMachinist = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelMarauder
        {
            get => levelMarauder;
            set
            {
                levelMarauder = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelPugilist
        {
            get => levelPugilist;
            set
            {
                levelPugilist = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelRedMage
        {
            get => levelRedMage;
            set
            {
                levelRedMage = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelRogue
        {
            get => levelRogue;
            set
            {
                levelRogue = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelSamurai
        {
            get => levelSamurai;
            set
            {
                levelSamurai = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool LevelThaumaturge
        {
            get => levelThaumaturge;
            set
            {
                levelThaumaturge = value;
                Save();
            }
        }

        [DefaultValue(60)]
        [Setting]
        public int MaxLevel
        {
            get => maxLevel;
            set
            {
                maxLevel = value;
                Save();
            }
        }

        [DefaultValue(4)]
        [Setting]
        internal int MobGrindMaxLevel
        {
            get => mobGrindMaxLevel;

            set
            {
                mobGrindMaxLevel = value;
                Save();
            }
        }

        [DefaultValue(6)]
        [Setting]
        internal int MobGrindMinLevel
        {
            get => mobGrindMinLevel;

            set
            {
                mobGrindMinLevel = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public OracleFateMode OracleFateMode
        {
            get => oracleFateMode;

            set
            {
                oracleFateMode = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool PrioritiseForlornMaiden
        {
            get => prioritiseForlornMaiden;

            set
            {
                prioritiseForlornMaiden = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool PrioritiseTheForlorn
        {
            get => prioritiseTheForlorn;

            set
            {
                prioritiseTheForlorn = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        internal bool ShowDebug
        {
            get => showDebug;

            set
            {
                showDebug = value;
                Save();
            }
        }

        internal static T GetDefaultValue<T>(string propertyName)
        {
            var property = typeof(OracleSettings).GetProperty(propertyName);
            var attribute = (DefaultValueAttribute) property.GetCustomAttribute(typeof(DefaultValueAttribute));

            return (T) attribute.Value;
        }

        internal void PopulateMobBlacklist()
        {
            /*
             * -------------------------
             * Heavensward S-Rank Hunts
             * -------------------------
             */

            const uint kaiserBehemoth = 4374;
            BlacklistedMobs.Add(kaiserBehemoth);

            const uint senmurv = 4375;
            BlacklistedMobs.Add(senmurv);

            const uint thePaleRider = 4376;
            BlacklistedMobs.Add(thePaleRider);

            const uint gandarewa = 4377;
            BlacklistedMobs.Add(gandarewa);

            const uint birdOfParadise = 4378;
            BlacklistedMobs.Add(birdOfParadise);

            const uint leucrotta = 4380;
            BlacklistedMobs.Add(leucrotta);

            /*
             * -------------------------
             * Heavensward A-Rank Hunts
             * -------------------------
             */

            const uint mirka = 4362;
            BlacklistedMobs.Add(mirka);

            const uint lyuba = 4363;
            BlacklistedMobs.Add(lyuba);

            const uint pylraster = 4364;
            BlacklistedMobs.Add(pylraster);

            const uint lordOfTheWyverns = 4365;
            BlacklistedMobs.Add(lordOfTheWyverns);

            const uint slipkinxSteeljoints = 4366;
            BlacklistedMobs.Add(slipkinxSteeljoints);

            const uint stolas = 4367;
            BlacklistedMobs.Add(stolas);

            const uint bune = 4368;
            BlacklistedMobs.Add(bune);

            const uint agathos = 4369;
            BlacklistedMobs.Add(agathos);

            const uint enkelados = 4370;
            BlacklistedMobs.Add(enkelados);

            const uint sisiutl = 4371;
            BlacklistedMobs.Add(sisiutl);

            const uint campacti = 4372;
            BlacklistedMobs.Add(campacti);

            const uint stenchBlossom = 4373;
            BlacklistedMobs.Add(stenchBlossom);

            /*
             * -------------------------
             * Heavensward B-Rank Hunts
             * -------------------------
             */

            const uint alteci = 4350;
            BlacklistedMobs.Add(alteci);

            const uint kreutzet = 4351;
            BlacklistedMobs.Add(kreutzet);

            const uint gnathCometdrone = 4352;
            BlacklistedMobs.Add(gnathCometdrone);

            const uint thextera = 4353;
            BlacklistedMobs.Add(thextera);

            const uint pterygotus = 4354;
            BlacklistedMobs.Add(pterygotus);

            const uint gigantopithecus = 4355;
            BlacklistedMobs.Add(gigantopithecus);

            const uint scitalis = 4356;
            BlacklistedMobs.Add(scitalis);

            const uint theScarecrow = 4357;
            BlacklistedMobs.Add(theScarecrow);

            const uint squonk = 4358;
            BlacklistedMobs.Add(squonk);

            const uint sanuValiOfDancingWings = 4359;
            BlacklistedMobs.Add(sanuValiOfDancingWings);

            const uint lycidas = 4360;
            BlacklistedMobs.Add(lycidas);

            const uint omni = 4361;
            BlacklistedMobs.Add(omni);

            /*
             * ---------------------------
             * A Realm Reborn S-Rank Hunts
             * ---------------------------
             */

            const uint agrippaTheMighty = 2969;
            BlacklistedMobs.Add(agrippaTheMighty);

            const uint bonnacon = 2965;
            BlacklistedMobs.Add(bonnacon);

            const uint brontes = 2958;
            BlacklistedMobs.Add(brontes);

            const uint croakadile = 2963;
            BlacklistedMobs.Add(croakadile);

            const uint croqueMitaine = 2962;
            BlacklistedMobs.Add(croqueMitaine);

            const uint garlok = 2964;
            BlacklistedMobs.Add(garlok);

            const uint laideronnette = 2953;
            BlacklistedMobs.Add(laideronnette);

            const uint lampalagua = 2959;
            BlacklistedMobs.Add(lampalagua);

            const uint mahisha = 2967;
            BlacklistedMobs.Add(mahisha);

            const uint mindflayer = 2955;
            BlacklistedMobs.Add(mindflayer);

            const uint minhocao = 2961;
            BlacklistedMobs.Add(minhocao);

            const uint nandi = 2966;
            BlacklistedMobs.Add(nandi);

            const uint nunyunuwi = 2960;
            BlacklistedMobs.Add(nunyunuwi);

            const uint safat = 2968;
            BlacklistedMobs.Add(safat);

            const uint thousandcastTheda = 2956;
            BlacklistedMobs.Add(thousandcastTheda);

            const uint wulgaru = 2954;
            BlacklistedMobs.Add(wulgaru);

            const uint zonaSeeker = 2957;
            BlacklistedMobs.Add(zonaSeeker);

            /*
             * ---------------------------
             * A Realm Reborn A-Rank Hunts
             * ---------------------------
             */

            const uint alectryon = 2940;
            BlacklistedMobs.Add(alectryon);

            const uint cornu = 2950;
            BlacklistedMobs.Add(cornu);

            const uint dalvagsFinalFlame = 2944;
            BlacklistedMobs.Add(dalvagsFinalFlame);

            const uint forneus = 2936;
            BlacklistedMobs.Add(forneus);

            const uint ghedeTiMalice = 2938;
            BlacklistedMobs.Add(ghedeTiMalice);

            const uint girtab = 2939;
            BlacklistedMobs.Add(girtab);

            const uint hellsclaw = 2947;
            BlacklistedMobs.Add(hellsclaw);

            const uint kurrea = 2952;
            BlacklistedMobs.Add(kurrea);

            const uint maahes = 2942;
            BlacklistedMobs.Add(maahes);

            const uint marberry = 2949;
            BlacklistedMobs.Add(marberry);

            const uint marraco = 2951;
            BlacklistedMobs.Add(marraco);

            const uint melt = 2937;
            BlacklistedMobs.Add(melt);

            const uint nahn = 2948;
            BlacklistedMobs.Add(nahn);

            const uint unktehi = 2946;
            BlacklistedMobs.Add(unktehi);

            const uint vogaalJa = 2945;
            BlacklistedMobs.Add(vogaalJa);

            const uint sabotenderBailarina = 2941;
            BlacklistedMobs.Add(sabotenderBailarina);

            const uint zanigoh = 2943;
            BlacklistedMobs.Add(zanigoh);

            /*
             * ---------------------------
             * A Realm Reborn B-Rank Hunts
             * ---------------------------
             */

            const uint albinTheAshen = 2926;
            BlacklistedMobs.Add(albinTheAshen);

            const uint barbastelle = 2929;
            BlacklistedMobs.Add(barbastelle);

            const uint bloodyMary = 2930;
            BlacklistedMobs.Add(bloodyMary);

            const uint darkHelmet = 2931;
            BlacklistedMobs.Add(darkHelmet);

            const uint flameSergeantDalvag = 2927;
            BlacklistedMobs.Add(flameSergeantDalvag);

            const uint gatling = 2925;
            BlacklistedMobs.Add(gatling);

            const uint leechKing = 2935;
            BlacklistedMobs.Add(leechKing);

            const uint monarchOgrefly = 2921;
            BlacklistedMobs.Add(monarchOgrefly);

            const uint myradrosh = 2932;
            BlacklistedMobs.Add(myradrosh);

            const uint naul = 2934;
            BlacklistedMobs.Add(naul);

            const uint ovjang = 2924;
            BlacklistedMobs.Add(ovjang);

            const uint phecda = 2922;
            BlacklistedMobs.Add(phecda);

            const uint sewerSyrup = 2923;
            BlacklistedMobs.Add(sewerSyrup);

            const uint skogsFru = 2928;
            BlacklistedMobs.Add(skogsFru);

            const uint stingingSophie = 2920;
            BlacklistedMobs.Add(stingingSophie);

            const uint vuokho = 2933;
            BlacklistedMobs.Add(vuokho);

            const uint whiteJoker = 2919;
            BlacklistedMobs.Add(whiteJoker);

            /*
             * ----------------------
             * Difficult / Elite Mobs
             * ----------------------
             */

            const uint scarredAntelope = 1992;
            BlacklistedMobs.Add(scarredAntelope);

            const uint bygoneBombard = 4964;
            BlacklistedMobs.Add(bygoneBombard);

            /*
			 * ---------------
			 * Unkillable Mobs
			 *----------------
			 */

            const uint trainingDummy = 541;
            BlacklistedMobs.Add(trainingDummy);

            BlacklistedMobs.Sort();
        }
    }
}