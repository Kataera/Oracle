using System.ComponentModel;
using System.Configuration;
using System.IO;

using ff14bot.Helpers;

namespace Oracle.Settings
{
    internal sealed class YokaiSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile YokaiSettings instance;

        private int blizzariaMedalsToFarm;
        private int blizzariaZoneChoice;
        private int hovernyanMedalsToFarm;
        private int hovernyanZoneChoice;
        private int jibanyanMedalsToFarm;
        private int jibanyanZoneChoice;
        private int komajiroMedalsToFarm;
        private int komajiroZoneChoice;
        private int komasanMedalsToFarm;
        private int komasanZoneChoice;
        private int kyubiMedalsToFarm;
        private int kyubiZoneChoice;
        private int manjimuttMedalsToFarm;
        private int manjimuttZoneChoice;
        private int nokoMedalsToFarm;
        private int nokoZoneChoice;
        private int robonyanMedalsToFarm;
        private int robonyanZoneChoice;
        private int shogunyanMedalsToFarm;
        private int shogunyanZoneChoice;
        private int usapyonMedalsToFarm;
        private int usapyonZoneChoice;
        private int venoctMedalsToFarm;
        private int venoctZoneChoice;
        private int whisperMedalsToFarm;
        private int whisperZoneChoice;

        public YokaiSettings() : base(Path.Combine(SettingsPath, @"Oracle\YokaiSettings.json"))
        {
        }

        [DefaultValue(0)]
        [Setting]
        public int BlizzariaMedalsToFarm
        {
            get
            {
                return blizzariaMedalsToFarm;
            }

            set
            {
                blizzariaMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int BlizzariaZoneChoice
        {
            get
            {
                return blizzariaZoneChoice;
            }

            set
            {
                blizzariaZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int HovernyanMedalsToFarm
        {
            get
            {
                return hovernyanMedalsToFarm;
            }

            set
            {
                hovernyanMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int HovernyanZoneChoice
        {
            get
            {
                return hovernyanZoneChoice;
            }

            set
            {
                hovernyanZoneChoice = value;
                Save();
            }
        }

        public static YokaiSettings Instance
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
                        instance = new YokaiSettings();
                    }
                }

                return instance;
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int JibanyanMedalsToFarm
        {
            get
            {
                return jibanyanMedalsToFarm;
            }

            set
            {
                jibanyanMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int JibanyanZoneChoice
        {
            get
            {
                return jibanyanZoneChoice;
            }

            set
            {
                jibanyanZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int KomajiroMedalsToFarm
        {
            get
            {
                return komajiroMedalsToFarm;
            }

            set
            {
                komajiroMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int KomajiroZoneChoice
        {
            get
            {
                return komajiroZoneChoice;
            }

            set
            {
                komajiroZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int KomasanMedalsToFarm
        {
            get
            {
                return komasanMedalsToFarm;
            }

            set
            {
                komasanMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int KomasanZoneChoice
        {
            get
            {
                return komasanZoneChoice;
            }

            set
            {
                komasanZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int KyubiMedalsToFarm
        {
            get
            {
                return kyubiMedalsToFarm;
            }

            set
            {
                kyubiMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int KyubiZoneChoice
        {
            get
            {
                return kyubiZoneChoice;
            }

            set
            {
                kyubiZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int ManjimuttMedalsToFarm
        {
            get
            {
                return manjimuttMedalsToFarm;
            }

            set
            {
                manjimuttMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int ManjimuttZoneChoice
        {
            get
            {
                return manjimuttZoneChoice;
            }

            set
            {
                manjimuttZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int NokoMedalsToFarm
        {
            get
            {
                return nokoMedalsToFarm;
            }

            set
            {
                nokoMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int NokoZoneChoice
        {
            get
            {
                return nokoZoneChoice;
            }

            set
            {
                nokoZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int RobonyanMedalsToFarm
        {
            get
            {
                return robonyanMedalsToFarm;
            }

            set
            {
                robonyanMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int RobonyanZoneChoice
        {
            get
            {
                return robonyanZoneChoice;
            }

            set
            {
                robonyanZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int ShogunyanMedalsToFarm
        {
            get
            {
                return shogunyanMedalsToFarm;
            }

            set
            {
                shogunyanMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int ShogunyanZoneChoice
        {
            get
            {
                return shogunyanZoneChoice;
            }

            set
            {
                shogunyanZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int UsapyonMedalsToFarm
        {
            get
            {
                return usapyonMedalsToFarm;
            }

            set
            {
                usapyonMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int UsapyonZoneChoice
        {
            get
            {
                return usapyonZoneChoice;
            }

            set
            {
                usapyonZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int VenoctMedalsToFarm
        {
            get
            {
                return venoctMedalsToFarm;
            }

            set
            {
                venoctMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int VenoctZoneChoice
        {
            get
            {
                return venoctZoneChoice;
            }

            set
            {
                venoctZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int WhisperMedalsToFarm
        {
            get
            {
                return whisperMedalsToFarm;
            }

            set
            {
                whisperMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(3)]
        [Setting]
        public int WhisperZoneChoice
        {
            get
            {
                return whisperZoneChoice;
            }

            set
            {
                whisperZoneChoice = value;
                Save();
            }
        }
    }
}