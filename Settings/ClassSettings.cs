using System.Collections.Generic;
using System.ComponentModel;
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

        [DefaultValue(false)]
        [Setting]
        public bool LevelArcanist
        {
            get
            {
                return levelArcanist;
            }
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
            get
            {
                return levelArcher;
            }
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
            get
            {
                return levelAstrologian;
            }
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
            get
            {
                return levelConjurer;
            }
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
            get
            {
                return levelDarkKnight;
            }
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
            get
            {
                return levelGladiator;
            }
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
            get
            {
                return levelLancer;
            }
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
            get
            {
                return levelMachinist;
            }
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
            get
            {
                return levelMarauder;
            }
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
            get
            {
                return levelPugilist;
            }
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
            get
            {
                return levelRogue;
            }
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
            get
            {
                return levelThaumaturge;
            }
            set
            {
                levelThaumaturge = value;
                Save();
            }
        }
    }
}