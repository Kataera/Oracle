using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;

using ff14bot.Helpers;

using Oracle.Enumerations;

namespace Oracle.Settings
{
    internal sealed class MainSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile MainSettings instance;

        private int actionDelay;
        private int combatNoDamageTimeout;
        private bool debugEnabled;
        private bool listHooksOnStart;
        private OracleOperationMode oracleOperationMode;
        private bool overrideRestBehaviour;
        private int restHealthPercent;
        private int restManaPercent;
        private int targetListCacheDuration;

        private MainSettings() : base(Path.Combine(SettingsPath, @"Oracle\MainSettings.json"))
        {
        }

        [DefaultValue(1500)]
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

        [DefaultValue(true)]
        [Setting]
        public bool DebugEnabled
        {
            get
            {
                return debugEnabled;
            }

            set
            {
                debugEnabled = value;
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

        [DefaultValue(OracleOperationMode.FateGrind)]
        [Setting]
        public OracleOperationMode OracleOperationMode
        {
            get
            {
                return oracleOperationMode;
            }

            set
            {
                oracleOperationMode = value;
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
        public int RestManaPercent
        {
            get
            {
                return restManaPercent;
            }

            set
            {
                restManaPercent = value;
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