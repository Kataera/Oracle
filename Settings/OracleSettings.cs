using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;

using Clio.Utilities;

using ff14bot.Helpers;

using Oracle.Enumerations;

namespace Oracle.Settings
{
    internal sealed class OracleSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile OracleSettings instance;

        private int actionDelay;
        private bool bindHomePoint;
        private List<uint> blacklistedFates;
        private List<uint> blacklistedMobs;
        private int bossEngagePercentage;
        private bool bossFatesEnabled;
        private int chainFateWaitTimeout;
        private bool collectFatesEnabled;
        private int collectFateTurnInAtAmount;
        private bool debugEnabled;
        private bool defenceFatesEnabled;
        private bool escortFatesEnabled;
        private bool fateDelayMovement;
        private int fateDelayMovementMaximum;
        private int fateDelayMovementMinimum;
        private int fateMaximumLevelAbove;
        private int fateMinimumLevelBelow;
        private FateSelectMode fateSelectMode;
        private Dictionary<uint, Vector3> fateWaitLocations;
        private FateWaitMode fateWaitMode;
        private bool flightAzysLlaEnabled;
        private bool flightChurningMistsEnabled;
        private bool flightCoerthasWesternHighlandsEnabled;
        private bool flightDravanianForelandsEnabled;
        private bool flightDravanianHinterlandsEnabled;
        private bool flightPathPostProcessingEnabled;
        private bool flightSeaOfCloudsEnabled;
        private bool ignoreLowDurationUnstartedFates;
        private bool killFatesEnabled;
        private double landingTimeOut;
        private bool listHooksOnStart;
        private int lowRemainingFateDuration;
        private int megaBossEngagePercentage;
        private bool megaBossFatesEnabled;
        private int mobMaximumLevelAbove;
        private int mobMinimumLevelBelow;
        private OracleOperationMode oracleOperationMode;
        private bool runProblematicFates;
        private string specificFateName;
        private bool teleportIfQuicker;
        private int teleportMinimumDistanceDelta;
        private bool waitAtBossFateForProgress;
        private bool waitAtMegaBossFateForProgress;
        private bool waitForChainFates;
        private bool zoneChangingEnabled;
        private Dictionary<uint, uint> zoneLevels;

        private OracleSettings() : base(Path.Combine(SettingsPath, @"/Oracle/OracleSettings.json"))
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

            if (FateWaitLocations == null)
            {
                FateWaitLocations = new Dictionary<uint, Vector3>();
                Save();
            }

            if (ZoneLevels == null)
            {
                ZoneLevels = new Dictionary<uint, uint>();
                PopulateZoneLevels();
                Save();
            }
        }

        [DefaultValue(1500)]
        [Setting]
        public int ActionDelay
        {
            get
            {
                return actionDelay;
            }

            set
            {
                actionDelay = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool BindHomePoint
        {
            get
            {
                return bindHomePoint;
            }

            set
            {
                bindHomePoint = value;
                Save();
            }
        }

        [Setting]
        public List<uint> BlacklistedFates
        {
            get
            {
                return blacklistedFates;
            }

            set
            {
                blacklistedFates = value;
                Save();
            }
        }

        [Setting]
        public List<uint> BlacklistedMobs
        {
            get
            {
                return blacklistedMobs;
            }

            set
            {
                blacklistedMobs = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int BossEngagePercentage
        {
            get
            {
                return bossEngagePercentage;
            }

            set
            {
                bossEngagePercentage = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool BossFatesEnabled
        {
            get
            {
                return bossFatesEnabled;
            }

            set
            {
                bossFatesEnabled = value;
                Save();
            }
        }

        [DefaultValue(60)]
        [Setting]
        public int ChainFateWaitTimeout
        {
            get
            {
                return chainFateWaitTimeout;
            }

            set
            {
                chainFateWaitTimeout = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool CollectFatesEnabled
        {
            get
            {
                return collectFatesEnabled;
            }

            set
            {
                collectFatesEnabled = value;
                Save();
            }
        }

        [DefaultValue(5)]
        [Setting]
        public int CollectFateTurnInAtAmount
        {
            get
            {
                return collectFateTurnInAtAmount;
            }

            set
            {
                collectFateTurnInAtAmount = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DebugEnabled
        {
            get
            {
                return debugEnabled;
            }

            set
            {
                debugEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DefenceFatesEnabled
        {
            get
            {
                return defenceFatesEnabled;
            }

            set
            {
                defenceFatesEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool EscortFatesEnabled
        {
            get
            {
                return escortFatesEnabled;
            }

            set
            {
                escortFatesEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FateDelayMovement
        {
            get
            {
                return fateDelayMovement;
            }

            set
            {
                fateDelayMovement = value;
                Save();
            }
        }

        [DefaultValue(3)]
        [Setting]
        public int FateDelayMovementMaximum
        {
            get
            {
                return fateDelayMovementMaximum;
            }

            set
            {
                fateDelayMovementMaximum = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int FateDelayMovementMinimum
        {
            get
            {
                return fateDelayMovementMinimum;
            }

            set
            {
                fateDelayMovementMinimum = value;
                Save();
            }
        }

        [DefaultValue(4)]
        [Setting]
        public int FateMaximumLevelAbove
        {
            get
            {
                return fateMaximumLevelAbove;
            }

            set
            {
                fateMaximumLevelAbove = value;
                Save();
            }
        }

        [DefaultValue(8)]
        [Setting]
        public int FateMinimumLevelBelow
        {
            get
            {
                return fateMinimumLevelBelow;
            }

            set
            {
                fateMinimumLevelBelow = value;
                Save();
            }
        }

        [DefaultValue(FateSelectMode.Closest)]
        [Setting]
        public FateSelectMode FateSelectMode
        {
            get
            {
                return fateSelectMode;
            }

            set
            {
                fateSelectMode = value;
                Save();
            }
        }

        [Setting]
        public Dictionary<uint, Vector3> FateWaitLocations
        {
            get
            {
                return fateWaitLocations;
            }

            set
            {
                fateWaitLocations = value;
                Save();
            }
        }

        [DefaultValue(FateWaitMode.GrindMobs)]
        [Setting]
        public FateWaitMode FateWaitMode
        {
            get
            {
                return fateWaitMode;
            }

            set
            {
                fateWaitMode = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FlightAzysLlaEnabled
        {
            get
            {
                return flightAzysLlaEnabled;
            }

            set
            {
                flightAzysLlaEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FlightChurningMistsEnabled
        {
            get
            {
                return flightChurningMistsEnabled;
            }

            set
            {
                flightChurningMistsEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FlightCoerthasWesternHighlandsEnabled
        {
            get
            {
                return flightCoerthasWesternHighlandsEnabled;
            }

            set
            {
                flightCoerthasWesternHighlandsEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FlightDravanianForelandsEnabled
        {
            get
            {
                return flightDravanianForelandsEnabled;
            }

            set
            {
                flightDravanianForelandsEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FlightDravanianHinterlandsEnabled
        {
            get
            {
                return flightDravanianHinterlandsEnabled;
            }

            set
            {
                flightDravanianHinterlandsEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FlightPathPostProcessingEnabled
        {
            get
            {
                return flightPathPostProcessingEnabled;
            }

            set
            {
                flightPathPostProcessingEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FlightSeaOfCloudsEnabled
        {
            get
            {
                return flightSeaOfCloudsEnabled;
            }

            set
            {
                flightSeaOfCloudsEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool IgnoreLowDurationUnstartedFates
        {
            get
            {
                return ignoreLowDurationUnstartedFates;
            }

            set
            {
                ignoreLowDurationUnstartedFates = value;
                Save();
            }
        }

        public static OracleSettings Instance
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

        [DefaultValue(true)]
        [Setting]
        public bool KillFatesEnabled
        {
            get
            {
                return killFatesEnabled;
            }

            set
            {
                killFatesEnabled = value;
                Save();
            }
        }

        [DefaultValue(10)]
        [Setting]
        public double LandingTimeOut
        {
            get
            {
                return landingTimeOut;
            }

            set
            {
                landingTimeOut = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool ListHooksOnStart
        {
            get
            {
                return listHooksOnStart;
            }

            set
            {
                listHooksOnStart = value;
                Save();
            }
        }

        [DefaultValue(180)]
        [Setting]
        public int LowRemainingFateDuration
        {
            get
            {
                return lowRemainingFateDuration;
            }

            set
            {
                lowRemainingFateDuration = value;
                Save();
            }
        }

        [DefaultValue(10)]
        [Setting]
        public int MegaBossEngagePercentage
        {
            get
            {
                return megaBossEngagePercentage;
            }

            set
            {
                megaBossEngagePercentage = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool MegaBossFatesEnabled
        {
            get
            {
                return megaBossFatesEnabled;
            }

            set
            {
                megaBossFatesEnabled = value;
                Save();
            }
        }

        [DefaultValue(4)]
        [Setting]
        public int MobMaximumLevelAbove
        {
            get
            {
                return mobMaximumLevelAbove;
            }

            set
            {
                mobMaximumLevelAbove = value;
                Save();
            }
        }

        [DefaultValue(6)]
        [Setting]
        public int MobMinimumLevelBelow
        {
            get
            {
                return mobMinimumLevelBelow;
            }

            set
            {
                mobMinimumLevelBelow = value;
                Save();
            }
        }

        [DefaultValue(OracleOperationMode.FateGrind)]
        [Setting]
        public OracleOperationMode OracleOperationMode
        {
            get
            {
                return oracleOperationMode;
            }

            set
            {
                oracleOperationMode = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool RunProblematicFates
        {
            get
            {
                return runProblematicFates;
            }

            set
            {
                runProblematicFates = value;
                Save();
            }
        }

        [DefaultValue("")]
        [Setting]
        public string SpecificFateName
        {
            get
            {
                return specificFateName;
            }

            set
            {
                specificFateName = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool TeleportIfQuicker
        {
            get
            {
                return teleportIfQuicker;
            }

            set
            {
                teleportIfQuicker = value;
                Save();
            }
        }

        [DefaultValue(400)]
        [Setting]
        public int TeleportMinimumDistanceDelta
        {
            get
            {
                return teleportMinimumDistanceDelta;
            }

            set
            {
                teleportMinimumDistanceDelta = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool WaitAtBossFateForProgress
        {
            get
            {
                return waitAtBossFateForProgress;
            }

            set
            {
                waitAtBossFateForProgress = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool WaitAtMegaBossFateForProgress
        {
            get
            {
                return waitAtMegaBossFateForProgress;
            }

            set
            {
                waitAtMegaBossFateForProgress = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool WaitForChainFates
        {
            get
            {
                return waitForChainFates;
            }

            set
            {
                waitForChainFates = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ZoneChangingEnabled
        {
            get
            {
                return zoneChangingEnabled;
            }

            set
            {
                zoneChangingEnabled = value;
                Save();
            }
        }

        [Setting]
        public Dictionary<uint, uint> ZoneLevels
        {
            get
            {
                return zoneLevels;
            }

            set
            {
                zoneLevels = value;
                Save();
            }
        }

        public static T GetDefaultValue<T>(string propertyName)
        {
            var property = typeof(OracleSettings).GetProperty(propertyName);
            var attribute = (DefaultValueAttribute) property.GetCustomAttribute(typeof(DefaultValueAttribute));

            return (T) attribute.Value;
        }

        public void PopulateMobBlacklist()
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
             * ----------
             * Elite Mobs
             * ----------
             */

            const uint scarredAntelope = 1992;
            BlacklistedMobs.Add(scarredAntelope);

            BlacklistedMobs.Sort();
        }

        public void PopulateZoneLevels()
        {
            const ushort aleport = 14;
            const ushort campCloudtop = 72;
            const ushort campDragonhead = 23;
            const ushort ceruleumProcessingPlant = 22;
            const ushort costaDelSol = 11;
            const ushort hawthorneHut = 4;
            const ushort horizon = 17;
            const ushort idyllshire = 75;
            const ushort moghome = 78;
            const ushort quarrymill = 5;
            const ushort tailfeather = 76;

            for (ushort i = 1; i < 60; i++)
            {
                // 1-12: Western Thanalan (Horizon).
                if (i < 12)
                {
                    ZoneLevels.Add(i, horizon);
                }

                // 12-18: Western La Noscea (Aleport).
                else if (i < 18)
                {
                    ZoneLevels.Add(i, aleport);
                }

                // 18-24: East Shroud (The Hawthorne Hut).
                else if (i < 24)
                {
                    ZoneLevels.Add(i, hawthorneHut);
                }

                // 24-29: South Shroud (Quarrymill).
                else if (i < 29)
                {
                    ZoneLevels.Add(i, quarrymill);
                }

                // 29-36: Eastern La Noscea (Costa del Sol).
                else if (i < 36)
                {
                    ZoneLevels.Add(i, costaDelSol);
                }

                // 36-45: Coerthas Central Highlands (Camp Dragonhead).
                else if (i < 45)
                {
                    ZoneLevels.Add(i, campDragonhead);
                }

                // 45-50: Northern Thanalan (Ceruleum Processing Plant).
                else if (i < 50)
                {
                    ZoneLevels.Add(i, ceruleumProcessingPlant);
                }

                // 50-52: The Sea of Clouds (Camp Cloudtop).
                else if (i < 52)
                {
                    ZoneLevels.Add(i, campCloudtop);
                }

                // 52-54: The Dravanian Forelands (Tailfeather).
                else if (i < 54)
                {
                    ZoneLevels.Add(i, tailfeather);
                }

                // 54-58: The Churning Mists (Moghome)
                else if (i < 58)
                {
                    ZoneLevels.Add(i, moghome);
                }

                // 58-60: Dravanian Hinterlands (Idyllshire).
                else
                {
                    ZoneLevels.Add(i, idyllshire);
                }
            }
        }
    }
}