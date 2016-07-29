using System;
using System.IO;
using System.Reflection;

using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;

using Oracle.Behaviour;
using Oracle.Behaviour.Hooks;
using Oracle.Behaviour.Modes;
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

        internal static string Version
            =>
                Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + "."
                + Assembly.GetExecutingAssembly().GetName().Version.Revision;

        public override bool WantButton => true;

        private static void AddAssemblyResolveHandler()
        {
            var botbaseAssembly = Assembly.GetExecutingAssembly();

            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                var missing = new AssemblyName(e.Name);

                // Sometimes the WPF assembly resolver cannot even find the executing assembly. :|
                if (missing.FullName == botbaseAssembly.FullName)
                {
                    return botbaseAssembly;
                }

                var botbaseFolder = Path.Combine(Environment.CurrentDirectory, @"BotBases\Oracle\");
                var missingPath = Path.Combine(botbaseFolder, missing.Name + ".dll");

                // If we find the DLL in Oracle's folder, load and return it.
                return File.Exists(missingPath) ? Assembly.LoadFrom(missingPath) : null;
            };
        }

        public override void Initialize()
        {
            Logger.SendLog("Initialising Oracle.");
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

        public override void OnButtonPress()
        {
            if (settingsWindow != null && settingsWindow.IsVisible)
            {
                return;
            }

            AddAssemblyResolveHandler();
            settingsWindow = new UI.Settings();
            settingsWindow.Show();
        }

        private static void ResetBotbaseVariables()
        {
            OracleFateManager.CurrentFateId = 0;
            OracleFateManager.PreviousFateId = 0;
            OracleFateManager.OracleDatabase = null;

            OracleMovementManager.ZoneFlightMesh = null;

            YokaiWatchGrind.ResetIgnoredYokai();
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

            if (MainSettings.Instance.OverrideRestBehaviour)
            {
                Logger.SendDebugLog("Overriding the combat routine rest behaviour.");
                TreeHooks.Instance.ReplaceHook("Rest", Rest.Behaviour);
            }

            if (MainSettings.Instance.ListHooksOnStart && MainSettings.Instance.DebugEnabled)
            {
                ListHooks();
            }

            switch (MainSettings.Instance.OracleOperationMode)
            {
                case OracleOperationMode.FateGrind:
                    Logger.SendLog("Starting Oracle in FATE grind mode.");
                    break;
                case OracleOperationMode.SpecificFate:
                    Logger.SendLog("Starting Oracle in specific FATE(s) mode.");
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
                case OracleOperationMode.YokaiWatchGrind:
                    Logger.SendLog("Starting Oracle in Yo-kai Watch grind mode. You cannot use your chocobo in this mode.");
                    break;
                default:
                    Logger.SendErrorLog("No setting chosen for operation mode. Defaulting to FATE grind mode.");
                    Logger.SendLog("Starting Oracle in FATE grind mode.");
                    break;
            }
        }

        public override void Stop()
        {
            ResetBotbaseVariables();

            if (LoadFlightMesh.MeshFileStream != null)
            {
                LoadFlightMesh.MeshFileStream.Dispose();
            }

            var navProvider = Navigator.NavigationProvider as GaiaNavigator;
            navProvider?.Dispose();

            Navigator.NavigationProvider = null;

            CombatTargeting.Instance.Provider = new DefaultCombatTargetingProvider();
            Blacklist.Flush();
            Chocobo.BlockSummon = false;

            GameSettingsManager.FaceTargetOnAction = playerFaceTargetOnAction;
            GameSettingsManager.FlightMode = playerFlightMode;

            if (MainSettings.Instance.OverrideRestBehaviour)
            {
                Logger.SendDebugLog("Restoring the combat routine rest behaviour.");
                TreeHooks.Instance.ReplaceHook("Rest", RoutineManager.Current.RestBehavior);
            }

            Logger.SendLog("Stopping Oracle.");
        }
    }
}