using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;

using ff14bot.Enums;
using ff14bot.Helpers;

using Oracle.Enumerations;

namespace Oracle.Settings
{
    internal sealed class ClassSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile ClassSettings instance;

        private Dictionary<uint, ClassJobType> classGearsets;
        private ClassLevelMode classLevelMode;
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
        private bool levelRogue;
        private bool levelThaumaturge;
        private int maxLevel;

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
            get => classGearsets;

            set
            {
                classGearsets = value;
                Save();
            }
        }

        // Use literal value rather than enum for obfuscator.
        // 0 = ClassLevelMode.Concurrent
        [DefaultValue(0)]
        [Setting]
        public ClassLevelMode ClassLevelMode
        {
            get => classLevelMode;
            set
            {
                classLevelMode = value;
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
    }
}