using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;

using ff14bot.Helpers;

namespace Oracle.Settings
{
    internal sealed class OracleSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile OracleSettings instance;

        private bool showDebug;

        private OracleSettings() : base(Path.Combine(SettingsPath, @"Oracle\OracleSettings.json"))
        {
        }

        public static OracleSettings Instance
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
                        instance = new OracleSettings();
                    }
                }

                return instance;
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool ShowDebug
        {
            get => showDebug;

            set
            {
                showDebug = value;
                Save();
            }
        }

        public static T GetDefaultValue<T>(string propertyName)
        {
            var property = typeof(OracleSettings).GetProperty(propertyName);
            var attribute = (DefaultValueAttribute) property.GetCustomAttribute(typeof(DefaultValueAttribute));

            return (T) attribute.Value;
        }
    }
}