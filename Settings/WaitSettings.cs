using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;

using Clio.Utilities;

using ff14bot.Helpers;

using Oracle.Enumerations;

namespace Oracle.Settings
{
    internal sealed class WaitSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile WaitSettings instance;

        private Dictionary<uint, Vector3> fateWaitLocations;
        private FateWaitMode fateWaitMode;
        private int mobGrindMaxLevelAbove;
        private int mobGrindMinLevelBelow;

        public WaitSettings() : base(Path.Combine(SettingsPath, @"Oracle\WaitSettings.json"))
        {
            if (FateWaitLocations == null)
            {
                FateWaitLocations = new Dictionary<uint, Vector3>();
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

        public static WaitSettings Instance
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
                        instance = new WaitSettings();
                    }
                }

                return instance;
            }
        }

        [DefaultValue(4)]
        [Setting]
        public int MobGrindMaxLevelAbove
        {
            get
            {
                return mobGrindMaxLevelAbove;
            }

            set
            {
                mobGrindMaxLevelAbove = value;
                Save();
            }
        }

        [DefaultValue(6)]
        [Setting]
        public int MobGrindMinLevelBelow
        {
            get
            {
                return mobGrindMinLevelBelow;
            }

            set
            {
                mobGrindMinLevelBelow = value;
                Save();
            }
        }
    }
}