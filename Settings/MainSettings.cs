using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;

using ff14bot.Helpers;

namespace Oracle.Settings
{
    internal sealed class MainSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile MainSettings instance;

        private int actionDelay;
        private int chocoboGreensRestockAmount;
        private bool chocoboGreensRestockEnabled;
        private bool chocoboHandlingEnabled;
        private int chocoboMinRemainingGreensToRestock;
        private int chocoboStanceChocoboHealthThreshold;
        private int chocoboStancePlayerHealthThreshold;
        private int chocoboStanceReturnToAttackThreshold;
        private int combatNoDamageTimeout;
        private bool listHooksOnStart;
        private bool overrideRestBehaviour;
        private int restHealthPercent;
        private int restTpManaPercent;
        private bool showDebugInConsole;
        private int targetListCacheDuration;

        private MainSettings() : base(Path.Combine(SettingsPath, @"Oracle\MainSettings.json"))
        {
        }

        [DefaultValue(1000)]
        [Setting]
        public int ActionDelay
        {
            get
            {
                return actionDelay;
            }

            set
            {
                actionDelay = value;
                Save();
            }
        }

        [DefaultValue(99)]
        [Setting]
        public int ChocoboGreensRestockAmount
        {
            get
            {
                return chocoboGreensRestockAmount;
            }

            set
            {
                chocoboGreensRestockAmount = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ChocoboGreensRestockEnabled
        {
            get
            {
                return chocoboGreensRestockEnabled;
            }

            set
            {
                chocoboGreensRestockEnabled = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ChocoboHandlingEnabled
        {
            get
            {
                return chocoboHandlingEnabled;
            }

            set
            {
                chocoboHandlingEnabled = value;
                Save();
            }
        }

        [DefaultValue(5)]
        [Setting]
        public int ChocoboMinRemainingGreensToRestock
        {
            get
            {
                return chocoboMinRemainingGreensToRestock;
            }

            set
            {
                chocoboMinRemainingGreensToRestock = value;
                Save();
            }
        }

        [DefaultValue(50)]
        [Setting]
        public int ChocoboStanceChocoboHealthThreshold
        {
            get
            {
                return chocoboStanceChocoboHealthThreshold;
            }

            set
            {
                chocoboStanceChocoboHealthThreshold = value;
                Save();
            }
        }

        [DefaultValue(70)]
        [Setting]
        public int ChocoboStancePlayerHealthThreshold
        {
            get
            {
                return chocoboStancePlayerHealthThreshold;
            }

            set
            {
                chocoboStancePlayerHealthThreshold = value;
                Save();
            }
        }

        [DefaultValue(85)]
        [Setting]
        public int ChocoboStanceReturnToAttackThreshold
        {
            get
            {
                return chocoboStanceReturnToAttackThreshold;
            }

            set
            {
                chocoboStanceReturnToAttackThreshold = value;
                Save();
            }
        }

        [DefaultValue(15000)]
        [Setting]
        public int CombatNoDamageTimeout
        {
            get
            {
                return combatNoDamageTimeout;
            }

            set
            {
                combatNoDamageTimeout = value;
                Save();
            }
        }

        public static MainSettings Instance
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
                        instance = new MainSettings();
                    }
                }

                return instance;
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool ListHooksOnStart
        {
            get
            {
                return listHooksOnStart;
            }

            set
            {
                listHooksOnStart = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool OverrideRestBehaviour
        {
            get
            {
                return overrideRestBehaviour;
            }

            set
            {
                overrideRestBehaviour = value;
                Save();
            }
        }

        [DefaultValue(50)]
        [Setting]
        public int RestHealthPercent
        {
            get
            {
                return restHealthPercent;
            }

            set
            {
                restHealthPercent = value;
                Save();
            }
        }

        [DefaultValue(30)]
        [Setting]
        public int RestTPManaPercent
        {
            get
            {
                return restTpManaPercent;
            }

            set
            {
                restTpManaPercent = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool ShowDebugInConsole
        {
            get
            {
                return showDebugInConsole;
            }

            set
            {
                showDebugInConsole = value;
                Save();
            }
        }

        [DefaultValue(2500)]
        [Setting]
        public int TargetListCacheDuration
        {
            get
            {
                return targetListCacheDuration;
            }

            set
            {
                targetListCacheDuration = value;
                Save();
            }
        }

        public static T GetDefaultValue<T>(string propertyName)
        {
            var property = typeof(MainSettings).GetProperty(propertyName);
            var attribute = (DefaultValueAttribute) property.GetCustomAttribute(typeof(DefaultValueAttribute));

            return (T) attribute.Value;
        }
    }
}