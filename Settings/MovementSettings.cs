using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;

using ff14bot.Helpers;

namespace Oracle.Settings
{
    internal sealed class MovementSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile MovementSettings instance;

        private bool azysLlaFlight;
        private bool bindHomePoint;
        private bool churningMistsFlight;
        private bool coerthasWesternHighlandsFlight;
        private bool considerAetheryteFateDistances;
        private bool delayFateMovement;
        private int delayFateMovementMax;
        private int delayFateMovementMin;
        private bool dravanianForelandsFlight;
        private bool dravanianHinterlandsFlight;
        private float fateProgressTeleportFateMaxProgress;
        private double landingTimeOut;
        private int minDistanceToTeleport;
        private bool processFlightPath;
        private bool seaOfCloudsFlight;
        private bool teleportIfQuicker;
        private Dictionary<uint, uint> zoneLevels;

        public MovementSettings() : base(Path.Combine(SettingsPath, @"Oracle\MovementSettings.json"))
        {
            if (ZoneLevels == null)
            {
                ZoneLevels = new Dictionary<uint, uint>();
                PopulateZoneLevels();
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool AzysLlaFlight
        {
            get
            {
                return azysLlaFlight;
            }

            set
            {
                azysLlaFlight = value;
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

        [DefaultValue(true)]
        [Setting]
        public bool ChurningMistsFlight
        {
            get
            {
                return churningMistsFlight;
            }

            set
            {
                churningMistsFlight = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool CoerthasWesternHighlandsFlight
        {
            get
            {
                return coerthasWesternHighlandsFlight;
            }

            set
            {
                coerthasWesternHighlandsFlight = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ConsiderAetheryteFateDistances
        {
            get
            {
                return considerAetheryteFateDistances;
            }

            set
            {
                considerAetheryteFateDistances = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DelayFateMovement
        {
            get
            {
                return delayFateMovement;
            }

            set
            {
                delayFateMovement = value;
                Save();
            }
        }

        [DefaultValue(3)]
        [Setting]
        public int DelayFateMovementMax
        {
            get
            {
                return delayFateMovementMax;
            }

            set
            {
                delayFateMovementMax = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int DelayFateMovementMin
        {
            get
            {
                return delayFateMovementMin;
            }

            set
            {
                delayFateMovementMin = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DravanianForelandsFlight
        {
            get
            {
                return dravanianForelandsFlight;
            }

            set
            {
                dravanianForelandsFlight = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DravanianHinterlandsFlight
        {
            get
            {
                return dravanianHinterlandsFlight;
            }

            set
            {
                dravanianHinterlandsFlight = value;
                Save();
            }
        }

        [DefaultValue(50f)]
        [Setting]
        public float FateProgressTeleportLimit
        {
            get
            {
                return fateProgressTeleportFateMaxProgress;
            }

            set
            {
                fateProgressTeleportFateMaxProgress = value;
                Save();
            }
        }

        public static MovementSettings Instance
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
                        instance = new MovementSettings();
                    }
                }

                return instance;
            }
        }

        [DefaultValue(10000)]
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

        [DefaultValue(400)]
        [Setting]
        public int MinDistanceToTeleport
        {
            get
            {
                return minDistanceToTeleport;
            }

            set
            {
                minDistanceToTeleport = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ProcessFlightPath
        {
            get
            {
                return processFlightPath;
            }

            set
            {
                processFlightPath = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool SeaOfCloudsFlight
        {
            get
            {
                return seaOfCloudsFlight;
            }

            set
            {
                seaOfCloudsFlight = value;
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

                // 18-23: East Shroud (The Hawthorne Hut).
                else if (i < 23)
                {
                    ZoneLevels.Add(i, hawthorneHut);
                }

                // 23-29: South Shroud (Quarrymill).
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