using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;

using ff14bot.Helpers;

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
            get => bossEngagePercentage;

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
            get => bossFatesEnabled;

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
            get => chainWaitTimeout;

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
            get => collectFatesEnabled;

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
            get => collectTurnInAmount;

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
            get => defenceFatesEnabled;

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
            get => escortFatesEnabled;

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
            get => fateMaxLevelAbove;

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
            get => fateMinLevelBelow;

            set
            {
                fateMinLevelBelow = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool IgnoreLowDuration
        {
            get => ignoreLowDuration;

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
            get => killFatesEnabled;

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
            get => lowFateDuration;

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
            get => megaBossEngagePercentage;

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
            get => megaBossFatesEnabled;

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
            get => runProblematicFates;

            set
            {
                runProblematicFates = value;
                Save();
            }
        }

        [Setting]
        public List<uint> SpecificFateList
        {
            get => specificFateList;

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
            get => waitAtBossForProgress;

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
            get => waitAtMegaBossForProgress;

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
            get => waitForChain;

            set
            {
                waitForChain = value;
                Save();
            }
        }
    }
}