using System.ComponentModel;
using System.Configuration;
using System.IO;

using ff14bot.Helpers;

using Oracle.Enumerations;

namespace Oracle.Settings
{
    internal sealed class ModeSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile ModeSettings instance;
        private int animaCrystalsToFarm;

        private int atmaToFarm;
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
        private OracleOperationMode oracleOperationMode;
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
        private int yokaiMedalsToFarm;
        private int yokaiMedalsZoneChoice;

        public ModeSettings() : base(Path.Combine(SettingsPath, @"Oracle\ModeSettings.json"))
        {
        }

        [DefaultValue(1)]
        [Setting]
        public int AnimaCrystalsToFarm
        {
            get => animaCrystalsToFarm;

            set
            {
                animaCrystalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int AtmaToFarm
        {
            get => atmaToFarm;

            set
            {
                atmaToFarm = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int BlizzariaMedalsToFarm
        {
            get => blizzariaMedalsToFarm;

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
            get => blizzariaZoneChoice;

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
            get => hovernyanMedalsToFarm;

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
            get => hovernyanZoneChoice;

            set
            {
                hovernyanZoneChoice = value;
                Save();
            }
        }

        public static ModeSettings Instance
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
                        instance = new ModeSettings();
                    }
                }

                return instance;
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int JibanyanMedalsToFarm
        {
            get => jibanyanMedalsToFarm;

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
            get => jibanyanZoneChoice;

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
            get => komajiroMedalsToFarm;

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
            get => komajiroZoneChoice;

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
            get => komasanMedalsToFarm;

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
            get => komasanZoneChoice;

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
            get => kyubiMedalsToFarm;

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
            get => kyubiZoneChoice;

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
            get => manjimuttMedalsToFarm;

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
            get => manjimuttZoneChoice;

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
            get => nokoMedalsToFarm;

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
            get => nokoZoneChoice;

            set
            {
                nokoZoneChoice = value;
                Save();
            }
        }

        // Use literal value rather than enum for obfuscator.
        // 0 = OracleOperationMode.FateGrind
        [DefaultValue(0)]
        [Setting]
        public OracleOperationMode OracleOperationMode
        {
            get => oracleOperationMode;

            set
            {
                oracleOperationMode = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int RobonyanMedalsToFarm
        {
            get => robonyanMedalsToFarm;

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
            get => robonyanZoneChoice;

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
            get => shogunyanMedalsToFarm;

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
            get => shogunyanZoneChoice;

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
            get => usapyonMedalsToFarm;

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
            get => usapyonZoneChoice;

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
            get => venoctMedalsToFarm;

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
            get => venoctZoneChoice;

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
            get => whisperMedalsToFarm;

            set
            {
                whisperMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int WhisperZoneChoice
        {
            get => whisperZoneChoice;

            set
            {
                whisperZoneChoice = value;
                Save();
            }
        }

        [DefaultValue(0)]
        [Setting]
        public int YokaiMedalsToFarm
        {
            get => yokaiMedalsToFarm;

            set
            {
                yokaiMedalsToFarm = value;
                Save();
            }
        }

        [DefaultValue(1)]
        [Setting]
        public int YokaiMedalZoneChoice
        {
            get => yokaiMedalsZoneChoice;

            set
            {
                yokaiMedalsZoneChoice = value;
                Save();
            }
        }
    }
}