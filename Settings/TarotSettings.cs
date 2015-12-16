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

using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;

using Clio.Utilities;

using ff14bot.Helpers;

using Tarot.Enumerations;

namespace Tarot.Settings
{
    internal sealed class TarotSettings : JsonSettings
    {
        private const uint Aleport = 14;
        private const uint CampDragonhead = 23;
        private const uint CeruleumProcessingPlant = 22;
        private const uint CostaDelSol = 11;
        private const uint FalconsNest = 71;
        private const uint HawthorneHut = 4;
        private const uint Horizon = 17;
        private const uint Idyllshire = 75;
        private const uint Moghome = 78;
        private const uint Quarrymill = 5;
        private const uint Tailfeather = 76;

        private static readonly object SyncRoot = new object();
        private static volatile TarotSettings instance;

        public Dictionary<uint, Vector3> FateWaitLocations;
        public Dictionary<uint, uint> ZoneLevels;

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
        private int grindMobRadius;
        private bool killFatesEnabled;
        private bool listHooksOnStart;
        private int megaBossEngagePercentage;
        private bool megaBossFatesEnabled;
        private int mobMaximumLevelAbove;
        private int mobMinimumLevelBelow;
        private bool runProblematicFates;
        private TarotOperationMode tarotOperationMode;
        private bool teleportIfQuicker;
        private int teleportMinimumDistanceDelta;
        private int turnInActionDelay;
        private bool waitAtFateForProgress;
        private bool waitForChainFates;

        private TarotSettings()
            : base(Path.Combine(SettingsPath, "TarotSettings.json"))
        {
            if (this.FateWaitLocations == null)
            {
                this.FateWaitLocations = new Dictionary<uint, Vector3>();
            }

            if (this.ZoneLevels == null)
            {
                this.ZoneLevels = new Dictionary<uint, uint>();
                this.PopulateZoneLevels();
            }
        }

        public static TarotSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TarotSettings();
                        }
                    }
                }

                return instance;
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

        [DefaultValue(200)]
        [Setting]
        public int GrindMobRadius
        {
            get { return this.grindMobRadius; }

            set
            {
                this.grindMobRadius = value;
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

        [DefaultValue(6)]
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

        [DefaultValue(TarotOperationMode.FateGrind)]
        [Setting]
        public TarotOperationMode TarotOperationMode
        {
            get { return this.tarotOperationMode; }

            set
            {
                this.tarotOperationMode = value;
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

        [DefaultValue(1500)]
        [Setting]
        public int TurnInActionDelay
        {
            get { return this.turnInActionDelay; }

            set
            {
                this.turnInActionDelay = value;
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

        private void PopulateZoneLevels()
        {
            for (uint i = 1; i < 60; i++)
            {
                // 1-12: Western Thanalan (Horizon).
                if (i < 12)
                {
                    this.ZoneLevels.Add(i, Horizon);
                }

                // 12-18: Western La Noscea (Aleport).
                else if (i < 18)
                {
                    this.ZoneLevels.Add(i, Aleport);
                }

                // 18-24: East Shroud (The Hawthorne Hut).
                else if (i < 24)
                {
                    this.ZoneLevels.Add(i, HawthorneHut);
                }

                // 24-29: South Shroud (Quarrymill).
                else if (i < 29)
                {
                    this.ZoneLevels.Add(i, Quarrymill);
                }

                // 29-36: Eastern La Noscea (Costa del Sol).
                else if (i < 36)
                {
                    this.ZoneLevels.Add(i, CostaDelSol);
                }

                // 36-43: Coerthas Central Highlands (Camp Dragonhead).
                else if (i < 43)
                {
                    this.ZoneLevels.Add(i, CampDragonhead);
                }

                // 43-46: Western La Noscea (Aleport).
                else if (i < 46)
                {
                    this.ZoneLevels.Add(i, Aleport);
                }

                // 46-50: Northern Thanalan (Ceruleum Processing Plant).
                else if (i < 50)
                {
                    this.ZoneLevels.Add(i, CeruleumProcessingPlant);
                }

                // 50-52: Coerthas Western Highlands (Falcon's Nest).
                else if (i < 52)
                {
                    this.ZoneLevels.Add(i, FalconsNest);
                }

                // 52-54: The Dravanian Forelands (Tailfeather).
                else if (i < 54)
                {
                    this.ZoneLevels.Add(i, Tailfeather);
                }

                // 54-58: The Churning Mists (Moghome)
                else if (i < 58)
                {
                    this.ZoneLevels.Add(i, Moghome);
                }

                // 58-60: Dravanian Hinterlands (Idyllshire).
                else
                {
                    this.ZoneLevels.Add(i, Idyllshire);
                }
            }
        }
    }
}