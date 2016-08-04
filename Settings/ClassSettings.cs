using System.Collections.Generic;
using System.Configuration;
using System.IO;

using ff14bot.Enums;
using ff14bot.Helpers;

namespace Oracle.Settings
{
    internal sealed class ClassSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile ClassSettings instance;

        private Dictionary<uint, ClassJobType> classGearsets;

        public ClassSettings() : base(Path.Combine(SettingsPath, @"Oracle\ClassSettings.json"))
        {
            if (ClassGearsets == null)
            {
                ClassGearsets = new Dictionary<uint, ClassJobType>();
                Save();
            }
        }

        [Setting]
        public Dictionary<uint, ClassJobType> ClassGearsets
        {
            get
            {
                return classGearsets;
            }

            set
            {
                classGearsets = value;
                Save();
            }
        }

        public static ClassSettings Instance
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
                        instance = new ClassSettings();
                    }
                }

                return instance;
            }
        }
    }
}