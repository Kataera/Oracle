using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;

using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;

using TreeSharp;

using Action = TreeSharp.Action;

namespace Oracle
{
    public class OracleLoader : BotBase
    {
        private const string BotbaseClass = "Oracle.OracleBot";
        private static BotBase oracleBotBase;
        private static readonly string ProjectAssembly = Path.Combine(Environment.CurrentDirectory, @"BotBases\Oracle\Oracle.dll");
        private static readonly Color LogColor = Color.FromRgb(179, 179, 255);

        public OracleLoader()
        {
            oracleBotBase = (BotBase) LoadOracle();
        }

        public override string EnglishName => "Oracle";

        public override bool IsAutonomous
        {
            get
            {
                if (oracleBotBase == null)
                {
                    oracleBotBase = (BotBase) LoadOracle();
                }

                return oracleBotBase?.IsAutonomous ?? true;
            }
        }

        public override string Name => "Oracle";

        public override PulseFlags PulseFlags
        {
            get
            {
                if (oracleBotBase == null)
                {
                    oracleBotBase = (BotBase) LoadOracle();
                }

                return oracleBotBase?.PulseFlags ?? PulseFlags.All;
            }
        }

        public override bool RequiresProfile
        {
            get
            {
                if (oracleBotBase == null)
                {
                    oracleBotBase = (BotBase) LoadOracle();
                }

                return oracleBotBase?.RequiresProfile ?? false;
            }
        }

        public override Composite Root
        {
            get
            {
                if (oracleBotBase == null)
                {
                    oracleBotBase = (BotBase) LoadOracle();
                }

                return oracleBotBase != null ? oracleBotBase.Root : new Action();
            }
        }

        public override bool WantButton
        {
            get
            {
                if (oracleBotBase == null)
                {
                    oracleBotBase = (BotBase)LoadOracle();
                }

                return oracleBotBase?.WantButton ?? true;
            }
        }

        private static object LoadOracle()
        {
            var dll = Assembly.LoadFile(ProjectAssembly);

            Type baseType;
            try
            {
                baseType = dll.GetType(BotbaseClass);
            }
            catch (Exception e)
            {
                Logging.Write(e.ToString());
                return null;
            }

            object botbaseObject;
            try
            {
                botbaseObject = Activator.CreateInstance(baseType);
            }
            catch (Exception e)
            {
                Logging.Write(e.ToString());
                return null;
            }

            if (botbaseObject != null)
            {
                SendLog("Oracle loaded successfully!");
            }

            return botbaseObject;
        }

        public override void OnButtonPress()
        {
            if (oracleBotBase == null)
            {
                oracleBotBase = (BotBase) LoadOracle();
            }

            oracleBotBase?.OnButtonPress();
        }

        internal static void SendLog(string log)
        {
            Logging.Write(LogColor, log);
        }

        public override void Start()
        {
            if (oracleBotBase == null)
            {
                oracleBotBase = (BotBase) LoadOracle();
            }

            oracleBotBase?.Start();
        }

        public override void Stop()
        {
            if (oracleBotBase == null)
            {
                oracleBotBase = (BotBase) LoadOracle();
            }

            oracleBotBase?.Stop();
        }
    }
}