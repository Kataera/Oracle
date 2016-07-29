using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;

using ff14bot.Helpers;

using Oracle.Enumerations;

namespace Oracle.Settings
{
    internal sealed class FateSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile FateSettings instance;

        private int bossEngagePercentage;
        private bool bossFatesEnabled;
        private int chainWaitTimeout;
        private bool collectFatesEnabled;
        private int collectTurnInAmount;
        private bool defenceFatesEnabled;
        private bool escortFatesEnabled;
        private int fateMaxLevelAbove;
        private int fateMinLevelBelow;
        private FateSelectMode fateSelectMode;
        private bool ignoreLowDuration;
        private bool killFatesEnabled;
        private int lowFateDuration;
        private int megaBossEngagePercentage;
        private bool megaBossFatesEnabled;
        private bool runProblematicFates;
        private List<uint> specificFateList;
        private bool waitAtBossForProgress;
        private bool waitAtMegaBossForProgress;
        private bool waitForChain;

        public FateSettings() : base(Path.Combine(SettingsPath, @"Oracle\FateSettings.json"))
        {
            if (SpecificFateList == null)
            {
                SpecificFateList = new List<uint>();
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
        public int ChainWaitTimeout
        {
            get
            {
                return chainWaitTimeout;
            }

            set
            {
                chainWaitTimeout = value;
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
        public int CollectTurnInAmount
        {
            get
            {
                return collectTurnInAmount;
            }

            set
            {
                collectTurnInAmount = value;
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

        [DefaultValue(4)]
        [Setting]
        public int FateMaxLevelAbove
        {
            get
            {
                return fateMaxLevelAbove;
            }

            set
            {
                fateMaxLevelAbove = value;
                Save();
            }
        }

        [DefaultValue(8)]
        [Setting]
        public int FateMinLevelBelow
        {
            get
            {
                return fateMinLevelBelow;
            }

            set
            {
                fateMinLevelBelow = value;
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

        [DefaultValue(true)]
        [Setting]
        public bool IgnoreLowDuration
        {
            get
            {
                return ignoreLowDuration;
            }

            set
            {
                ignoreLowDuration = value;
                Save();
            }
        }

        public static FateSettings Instance
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
                        instance = new FateSettings();
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

        [DefaultValue(180)]
        [Setting]
        public int LowFateDuration
        {
            get
            {
                return lowFateDuration;
            }

            set
            {
                lowFateDuration = value;
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

        [Setting]
        public List<uint> SpecificFateList
        {
            get
            {
                return specificFateList;
            }

            set
            {
                specificFateList = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool WaitAtBossForProgress
        {
            get
            {
                return waitAtBossForProgress;
            }

            set
            {
                waitAtBossForProgress = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool WaitAtMegaBossForProgress
        {
            get
            {
                return waitAtMegaBossForProgress;
            }

            set
            {
                waitAtMegaBossForProgress = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool WaitForChain
        {
            get
            {
                return waitForChain;
            }

            set
            {
                waitForChain = value;
                Save();
            }
        }
    }
}