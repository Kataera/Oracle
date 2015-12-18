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

using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;

using Clio.Utilities;

using ff14bot.Helpers;

using Oracle.Enumerations;

namespace Oracle.Settings
{
    internal sealed class OracleSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile OracleSettings instance;

        public List<uint> BlacklistedFates;
        public List<uint> BlacklistedMobs;
        public Dictionary<uint, Vector3> FateWaitLocations;
        public Dictionary<uint, uint> ZoneLevels;

        private int actionDelay;
        private bool bindHomePoint;
        private int bossEngagePercentage;
        private bool bossFatesEnabled;
        private int chainFateWaitTimeout;
        private bool changeZonesEnabled;
        private bool collectFatesEnabled;
        private int collectFateTurnInAtAmount;
        private bool debugEnabled;
        private bool defenceFatesEnabled;
        private bool escortFatesEnabled;
        private bool fateDelayMovement;
        private int fateDelayMovementMaximum;
        private int fateDelayMovementMinimum;
        private int fateMaxLevelsAbove;
        private int fateMaxLevelsBelow;
        private FateSelectMode fateSelectMode;
        private FateWaitMode fateWaitMode;
        private bool ignoreLowDurationUnstartedFates;
        private bool killFatesEnabled;
        private bool listHooksOnStart;
        private int lowRemainingDuration;
        private int megaBossEngagePercentage;
        private bool megaBossFatesEnabled;
        private int mobMaximumLevelAbove;
        private int mobMinimumLevelBelow;
        private OracleOperationMode oracleOperationMode;
        private bool runProblematicFates;
        private string specificFate;
        private bool teleportIfQuicker;
        private int teleportMinimumDistanceDelta;
        private bool waitAtFateForProgress;
        private bool waitForChainFates;

        private OracleSettings()
            : base(Path.Combine(SettingsPath, "OracleSettings.json"))
        {
            if (this.BlacklistedFates == null)
            {
                this.BlacklistedFates = new List<uint>();
                this.Save();
            }

            if (this.BlacklistedMobs == null)
            {
                this.BlacklistedMobs = new List<uint>();
                this.PopulateMobBlacklist();
                this.Save();
            }

            if (this.FateWaitLocations == null)
            {
                this.FateWaitLocations = new Dictionary<uint, Vector3>();
                this.Save();
            }

            if (this.ZoneLevels == null)
            {
                this.ZoneLevels = new Dictionary<uint, uint>();
                this.PopulateZoneLevels();
                this.Save();
            }
        }

        public static OracleSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new OracleSettings();
                        }
                    }
                }

                return instance;
            }
        }

        [DefaultValue(1500)]
        [Setting]
        public int ActionDelay
        {
            get { return this.actionDelay; }

            set
            {
                this.actionDelay = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool BindHomePoint
        {
            get { return this.bindHomePoint; }

            set
            {
                this.bindHomePoint = value;
                this.Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int BossEngagePercentage
        {
            get { return this.bossEngagePercentage; }

            set
            {
                this.bossEngagePercentage = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool BossFatesEnabled
        {
            get { return this.bossFatesEnabled; }

            set
            {
                this.bossFatesEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(60)]
        [Setting]
        public int ChainFateWaitTimeout
        {
            get { return this.chainFateWaitTimeout; }

            set
            {
                this.chainFateWaitTimeout = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ChangeZonesEnabled
        {
            get { return this.changeZonesEnabled; }

            set
            {
                this.changeZonesEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool CollectFatesEnabled
        {
            get { return this.collectFatesEnabled; }

            set
            {
                this.collectFatesEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(5)]
        [Setting]
        public int CollectFateTurnInAtAmount
        {
            get { return this.collectFateTurnInAtAmount; }

            set
            {
                this.collectFateTurnInAtAmount = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DebugEnabled
        {
            get { return this.debugEnabled; }

            set
            {
                this.debugEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DefenceFatesEnabled
        {
            get { return this.defenceFatesEnabled; }

            set
            {
                this.defenceFatesEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool EscortFatesEnabled
        {
            get { return this.escortFatesEnabled; }

            set
            {
                this.escortFatesEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool FateDelayMovement
        {
            get { return this.fateDelayMovement; }

            set
            {
                this.fateDelayMovement = value;
                this.Save();
            }
        }

        [DefaultValue(3)]
        [Setting]
        public int FateDelayMovementMaximum
        {
            get { return this.fateDelayMovementMaximum; }

            set
            {
                this.fateDelayMovementMaximum = value;
                this.Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int FateDelayMovementMinimum
        {
            get { return this.fateDelayMovementMinimum; }

            set
            {
                this.fateDelayMovementMinimum = value;
                this.Save();
            }
        }

        [DefaultValue(4)]
        [Setting]
        public int FateMaxLevelsAbove
        {
            get { return this.fateMaxLevelsAbove; }

            set
            {
                this.fateMaxLevelsAbove = value;
                this.Save();
            }
        }

        [DefaultValue(8)]
        [Setting]
        public int FateMaxLevelsBelow
        {
            get { return this.fateMaxLevelsBelow; }

            set
            {
                this.fateMaxLevelsBelow = value;
                this.Save();
            }
        }

        [DefaultValue(FateSelectMode.Closest)]
        [Setting]
        public FateSelectMode FateSelectMode
        {
            get { return this.fateSelectMode; }

            set
            {
                this.fateSelectMode = value;
                this.Save();
            }
        }

        [DefaultValue(FateWaitMode.GrindMobs)]
        [Setting]
        public FateWaitMode FateWaitMode
        {
            get { return this.fateWaitMode; }

            set
            {
                this.fateWaitMode = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool IgnoreLowDurationUnstartedFates
        {
            get { return this.ignoreLowDurationUnstartedFates; }

            set
            {
                this.ignoreLowDurationUnstartedFates = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool KillFatesEnabled
        {
            get { return this.killFatesEnabled; }

            set
            {
                this.killFatesEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool ListHooksOnStart
        {
            get { return this.listHooksOnStart; }

            set
            {
                this.listHooksOnStart = value;
                this.Save();
            }
        }

        [DefaultValue(180)]
        [Setting]
        public int LowRemainingDuration
        {
            get { return this.lowRemainingDuration; }

            set
            {
                this.lowRemainingDuration = value;
                this.Save();
            }
        }

        [DefaultValue(10)]
        [Setting]
        public int MegaBossEngagePercentage
        {
            get { return this.megaBossEngagePercentage; }

            set
            {
                this.megaBossEngagePercentage = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool MegaBossFatesEnabled
        {
            get { return this.megaBossFatesEnabled; }

            set
            {
                this.megaBossFatesEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(4)]
        [Setting]
        public int MobMaximumLevelAbove
        {
            get { return this.mobMaximumLevelAbove; }

            set
            {
                this.mobMaximumLevelAbove = value;
                this.Save();
            }
        }

        [DefaultValue(6)]
        [Setting]
        public int MobMinimumLevelBelow
        {
            get { return this.mobMinimumLevelBelow; }

            set
            {
                this.mobMinimumLevelBelow = value;
                this.Save();
            }
        }

        [DefaultValue(OracleOperationMode.FateGrind)]
        [Setting]
        public OracleOperationMode OracleOperationMode
        {
            get { return this.oracleOperationMode; }

            set
            {
                this.oracleOperationMode = value;
                this.Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool RunProblematicFates
        {
            get { return this.runProblematicFates; }

            set
            {
                this.runProblematicFates = value;
                this.Save();
            }
        }

        public string SpecificFate
        {
            get { return this.specificFate; }

            set
            {
                this.specificFate = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool TeleportIfQuicker
        {
            get { return this.teleportIfQuicker; }

            set
            {
                this.teleportIfQuicker = value;
                this.Save();
            }
        }

        [DefaultValue(400)]
        [Setting]
        public int TeleportMinimumDistanceDelta
        {
            get { return this.teleportMinimumDistanceDelta; }

            set
            {
                this.teleportMinimumDistanceDelta = value;
                this.Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool WaitAtFateForProgress
        {
            get { return this.waitAtFateForProgress; }

            set
            {
                this.waitAtFateForProgress = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool WaitForChainFates
        {
            get { return this.waitForChainFates; }

            set
            {
                this.waitForChainFates = value;
                this.Save();
            }
        }

        private void PopulateMobBlacklist()
        {
            // Heavensward S-Rank Hunts.
            const uint kaiserBehemoth = 4374;
            const uint senmurv = 4375;
            const uint thePaleRider = 4376;
            const uint gandarewa = 4377;
            const uint birdOfParadise = 4378;
            const uint leucrotta = 4380;
            this.BlacklistedMobs.Add(kaiserBehemoth);
            this.BlacklistedMobs.Add(senmurv);
            this.BlacklistedMobs.Add(thePaleRider);
            this.BlacklistedMobs.Add(gandarewa);
            this.BlacklistedMobs.Add(birdOfParadise);
            this.BlacklistedMobs.Add(leucrotta);

            // Heavensward A-Rank Hunts.
            const uint mirka = 4362;
            const uint lyuba = 4363;
            const uint pylraster = 4364;
            const uint lordOfTheWyverns = 4365;
            const uint slipkinxSteeljoints = 4366;
            const uint stolas = 4367;
            const uint bune = 4368;
            const uint agathos = 4369;
            const uint enkelados = 4370;
            const uint sisiutl = 4371;
            const uint campacti = 4372;
            const uint stenchBlossom = 4373;
            this.BlacklistedMobs.Add(mirka);
            this.BlacklistedMobs.Add(lyuba);
            this.BlacklistedMobs.Add(pylraster);
            this.BlacklistedMobs.Add(lordOfTheWyverns);
            this.BlacklistedMobs.Add(slipkinxSteeljoints);
            this.BlacklistedMobs.Add(stolas);
            this.BlacklistedMobs.Add(bune);
            this.BlacklistedMobs.Add(agathos);
            this.BlacklistedMobs.Add(enkelados);
            this.BlacklistedMobs.Add(sisiutl);
            this.BlacklistedMobs.Add(campacti);
            this.BlacklistedMobs.Add(stenchBlossom);

            // Heavensward B-Rank Hunts.
            const uint alteci = 4350;
            const uint kreutzet = 4351;
            const uint gnathCometdrone = 4352;
            const uint thextera = 4353;
            const uint pterygotus = 4354;
            const uint gigantopithecus = 4355;
            const uint scitalis = 4356;
            const uint theScarecrow = 4357;
            const uint squonk = 4358;
            const uint sanuValiOfDancingWings = 4359;
            const uint lycidas = 4360;
            const uint omni = 4361;
            this.BlacklistedMobs.Add(alteci);
            this.BlacklistedMobs.Add(kreutzet);
            this.BlacklistedMobs.Add(gnathCometdrone);
            this.BlacklistedMobs.Add(thextera);
            this.BlacklistedMobs.Add(pterygotus);
            this.BlacklistedMobs.Add(gigantopithecus);
            this.BlacklistedMobs.Add(scitalis);
            this.BlacklistedMobs.Add(theScarecrow);
            this.BlacklistedMobs.Add(squonk);
            this.BlacklistedMobs.Add(sanuValiOfDancingWings);
            this.BlacklistedMobs.Add(lycidas);
            this.BlacklistedMobs.Add(omni);

            // A Realm Reborn S-Rank Hunts.
            const uint agrippaTheMighty = 2969;
            const uint bonnacon = 2965;
            const uint brontes = 2958;
            const uint croakadile = 2963;
            const uint croqueMitaine = 2962;
            const uint garlok = 2964;
            const uint laideronnette = 2953;
            const uint lampalagua = 2959;
            const uint mahisha = 2967;
            const uint mindflayer = 2955;
            const uint minhocao = 2961;
            const uint nandi = 2966;
            const uint nunyunuwi = 2960;
            const uint safat = 2968;
            const uint thousandcastTheda = 2956;
            const uint wulgaru = 2954;
            const uint zonaSeeker = 2957;
            this.BlacklistedMobs.Add(agrippaTheMighty);
            this.BlacklistedMobs.Add(bonnacon);
            this.BlacklistedMobs.Add(brontes);
            this.BlacklistedMobs.Add(croakadile);
            this.BlacklistedMobs.Add(croqueMitaine);
            this.BlacklistedMobs.Add(garlok);
            this.BlacklistedMobs.Add(laideronnette);
            this.BlacklistedMobs.Add(lampalagua);
            this.BlacklistedMobs.Add(mahisha);
            this.BlacklistedMobs.Add(mindflayer);
            this.BlacklistedMobs.Add(minhocao);
            this.BlacklistedMobs.Add(nandi);
            this.BlacklistedMobs.Add(nunyunuwi);
            this.BlacklistedMobs.Add(safat);
            this.BlacklistedMobs.Add(thousandcastTheda);
            this.BlacklistedMobs.Add(wulgaru);
            this.BlacklistedMobs.Add(zonaSeeker);

            // A Realm Reborn A-Rank Hunts.
            const uint alectryon = 2940;
            const uint cornu = 2950;
            const uint dalvagsFinalFlame = 2944;
            const uint forneus = 2936;
            const uint ghedeTiMalice = 2938;
            const uint girtab = 2939;
            const uint hellsclaw = 2947;
            const uint kurrea = 2952;
            const uint maahes = 2942;
            const uint marberry = 2949;
            const uint marraco = 2951;
            const uint melt = 2937;
            const uint nahn = 2948;
            const uint unktehi = 2946;
            const uint vogaalJa = 2945;
            const uint sabotenderBailarina = 2941;
            const uint zanigoh = 2943;
            this.BlacklistedMobs.Add(alectryon);
            this.BlacklistedMobs.Add(cornu);
            this.BlacklistedMobs.Add(dalvagsFinalFlame);
            this.BlacklistedMobs.Add(forneus);
            this.BlacklistedMobs.Add(ghedeTiMalice);
            this.BlacklistedMobs.Add(girtab);
            this.BlacklistedMobs.Add(hellsclaw);
            this.BlacklistedMobs.Add(kurrea);
            this.BlacklistedMobs.Add(maahes);
            this.BlacklistedMobs.Add(marberry);
            this.BlacklistedMobs.Add(marraco);
            this.BlacklistedMobs.Add(melt);
            this.BlacklistedMobs.Add(nahn);
            this.BlacklistedMobs.Add(unktehi);
            this.BlacklistedMobs.Add(vogaalJa);
            this.BlacklistedMobs.Add(sabotenderBailarina);
            this.BlacklistedMobs.Add(zanigoh);

            // A Realm Reborn B-Rank Hunts.
            const uint albinTheAshen = 2926;
            const uint barbastelle = 2929;
            const uint bloodyMary = 2930;
            const uint darkHelmet = 2931;
            const uint flameSergeantDalvag = 2927;
            const uint gatling = 2925;
            const uint leechKing = 2935;
            const uint monarchOgrefly = 2921;
            const uint myradrosh = 2932;
            const uint naul = 2934;
            const uint ovjang = 2924;
            const uint phecda = 2922;
            const uint sewerSyrup = 2923;
            const uint skogsFru = 2928;
            const uint stingingSophie = 2920;
            const uint vuokho = 2933;
            const uint whiteJoker = 2919;
            this.BlacklistedMobs.Add(albinTheAshen);
            this.BlacklistedMobs.Add(barbastelle);
            this.BlacklistedMobs.Add(bloodyMary);
            this.BlacklistedMobs.Add(darkHelmet);
            this.BlacklistedMobs.Add(flameSergeantDalvag);
            this.BlacklistedMobs.Add(gatling);
            this.BlacklistedMobs.Add(leechKing);
            this.BlacklistedMobs.Add(monarchOgrefly);
            this.BlacklistedMobs.Add(myradrosh);
            this.BlacklistedMobs.Add(naul);
            this.BlacklistedMobs.Add(ovjang);
            this.BlacklistedMobs.Add(phecda);
            this.BlacklistedMobs.Add(sewerSyrup);
            this.BlacklistedMobs.Add(skogsFru);
            this.BlacklistedMobs.Add(stingingSophie);
            this.BlacklistedMobs.Add(vuokho);
            this.BlacklistedMobs.Add(whiteJoker);

            // Elite mobs.
            const uint scarredAntelope = 1992;
            this.BlacklistedMobs.Add(scarredAntelope);

            this.BlacklistedMobs.Sort();
        }

        private void PopulateZoneLevels()
        {
            const uint aleport = 14;
            const uint campDragonhead = 23;
            const uint ceruleumProcessingPlant = 22;
            const uint costaDelSol = 11;
            const uint falconsNest = 71;
            const uint hawthorneHut = 4;
            const uint horizon = 17;
            const uint idyllshire = 75;
            const uint moghome = 78;
            const uint quarrymill = 5;
            const uint tailfeather = 76;

            for (uint i = 1; i < 60; i++)
            {
                // 1-12: Western Thanalan (Horizon).
                if (i < 12)
                {
                    this.ZoneLevels.Add(i, horizon);
                }

                // 12-18: Western La Noscea (Aleport).
                else if (i < 18)
                {
                    this.ZoneLevels.Add(i, aleport);
                }

                // 18-24: East Shroud (The Hawthorne Hut).
                else if (i < 24)
                {
                    this.ZoneLevels.Add(i, hawthorneHut);
                }

                // 24-29: South Shroud (Quarrymill).
                else if (i < 29)
                {
                    this.ZoneLevels.Add(i, quarrymill);
                }

                // 29-36: Eastern La Noscea (Costa del Sol).
                else if (i < 36)
                {
                    this.ZoneLevels.Add(i, costaDelSol);
                }

                // 36-43: Coerthas Central Highlands (Camp Dragonhead).
                else if (i < 43)
                {
                    this.ZoneLevels.Add(i, campDragonhead);
                }

                // 43-46: Western La Noscea (Aleport).
                else if (i < 46)
                {
                    this.ZoneLevels.Add(i, aleport);
                }

                // 46-50: Northern Thanalan (Ceruleum Processing Plant).
                else if (i < 50)
                {
                    this.ZoneLevels.Add(i, ceruleumProcessingPlant);
                }

                // 50-52: Coerthas Western Highlands (Falcon's Nest).
                else if (i < 52)
                {
                    this.ZoneLevels.Add(i, falconsNest);
                }

                // 52-54: The Dravanian Forelands (Tailfeather).
                else if (i < 54)
                {
                    this.ZoneLevels.Add(i, tailfeather);
                }

                // 54-58: The Churning Mists (Moghome)
                else if (i < 58)
                {
                    this.ZoneLevels.Add(i, moghome);
                }

                // 58-60: Dravanian Hinterlands (Idyllshire).
                else
                {
                    this.ZoneLevels.Add(i, idyllshire);
                }
            }
        }
    }
}