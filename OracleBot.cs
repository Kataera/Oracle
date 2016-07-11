using System.Reflection;

using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using Oracle.Behaviour;
using Oracle.Behaviour.PoiHooks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Providers;
using Oracle.Settings;

using TreeSharp;

namespace Oracle
{
    public class OracleBot : BotBase
    {
        private static bool playerFaceTargetOnAction;

        private static bool playerFlightMode;

        private static Composite root;

        private static UI.Settings settingsWindow;

        public override string EnglishName => "Oracle";

        public override bool IsAutonomous => true;

        public override string Name => "Oracle";

        public override PulseFlags PulseFlags => PulseFlags.All;

        public override bool RequiresProfile => false;

        public override Composite Root => root;

        public override bool WantButton => true;

        internal static string Version => Assembly.GetExecutingAssembly().GetName().Version.Major + "."
                                          + Assembly.GetExecutingAssembly().GetName().Version.Minor + "."
                                          + Assembly.GetExecutingAssembly().GetName().Version.Revision;

        public override void Initialize()
        {
            Logger.SendLog("Initialising Oracle.");
        }

        public override void OnButtonPress()
        {
            if (settingsWindow == null)
            {
                settingsWindow = new UI.Settings();
            }

            settingsWindow.Show();
        }

        public override void Start()
        {
            Navigator.NavigationProvider = new GaiaNavigator();
            Navigator.PlayerMover = new SlideMover();
            CombatTargeting.Instance.Provider = new OracleCombatTargetingProvider();

            playerFaceTargetOnAction = GameSettingsManager.FaceTargetOnAction;
            playerFlightMode = GameSettingsManager.FlightMode;
            GameSettingsManager.FaceTargetOnAction = true;
            GameSettingsManager.FlightMode = true;

            TreeHooks.Instance.ClearAll();
            root = BrainBehavior.CreateBrain();
            TreeHooks.Instance.AddHook("TreeStart", OracleBehaviour.Behaviour);
            TreeHooks.Instance.ReplaceHook("SelectPoiType", SelectPoiType.Behaviour);

            if (OracleSettings.Instance.ListHooksOnStart && OracleSettings.Instance.DebugEnabled)
            {
                ListHooks();
            }

            switch (OracleSettings.Instance.OracleOperationMode)
            {
                case OracleOperationMode.FateGrind:
                    Logger.SendLog("Starting Oracle in FATE grind mode.");
                    break;
                case OracleOperationMode.SpecificFate:
                    Logger.SendLog("Starting Oracle in specific FATE mode.");
                    break;
                case OracleOperationMode.AtmaGrind:
                    Logger.SendLog("Starting Oracle in Atma grind mode.");
                    break;
                case OracleOperationMode.AnimusGrind:
                    Logger.SendLog("Starting Oracle in Animus grind mode.");
                    break;
                case OracleOperationMode.AnimaGrind:
                    Logger.SendLog("Starting Oracle in Anima grind mode.");
                    break;
            }
        }

        public override void Stop()
        {
            // Clean up all botbase internal variables.
            OracleFateManager.CurrentFateId = 0;
            OracleFateManager.PreviousFateId = 0;
            OracleFateManager.OracleDatabase = null;
            OracleMovementManager.ZoneFlightMesh = null;

            if (LoadFlightMesh.MeshFileStream != null)
            {
                LoadFlightMesh.MeshFileStream.Dispose();
            }

            var navProvider = Navigator.NavigationProvider as GaiaNavigator;
            navProvider?.Dispose();

            Navigator.NavigationProvider = null;

            CombatTargeting.Instance.Provider = new DefaultCombatTargetingProvider();
            Blacklist.Flush();

            GameSettingsManager.FaceTargetOnAction = playerFaceTargetOnAction;
            GameSettingsManager.FlightMode = playerFlightMode;

            Logger.SendLog("Stopping Oracle.");
        }

        private static void ListHooks()
        {
            Logger.SendDebugLog("Listing RebornBuddy hooks.");
            foreach (var hook in TreeHooks.Instance.Hooks)
            {
                Logger.SendDebugLog(hook.Key + ": " + hook.Value.Count + " Composite(s).");

                var count = 0;
                foreach (var composite in hook.Value)
                {
                    count++;
                    Logger.SendDebugLog("\tComposite " + count + ": " + composite + ".");
                }

                Logger.SendDebugLog(string.Empty);
            }
        }
    }
}