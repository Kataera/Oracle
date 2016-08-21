using System;
using System.IO;
using System.Linq;
using System.Reflection;

using ff14bot;
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
    public class OracleBot
    {
        private static bool playerFaceTargetOnAction;
        private static bool playerFlightMode;
        private static Composite root;
        private static UI.Settings settingsWindow;

        private static readonly string VersionPath = Path.Combine(Environment.CurrentDirectory, @"BotBases\Oracle\version.txt");

        internal Composite Root => root;

        internal static string Version
        {
            get
            {
                if (!File.Exists(VersionPath))
                {
                    return null;
                }

                try
                {
                    var version = File.ReadAllText(VersionPath);
                    return version;
                }
                catch
                {
                    return null;
                }
            }
        }

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

        public Composite GetRoot()
        {
            return Root;
        }

        internal void Initialize()
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
                    Logger.SendDebugLog("\tComposite " + count + ": " + composite.GetType().FullName + ".");
                }

                Logger.SendDebugLog(string.Empty);
            }
        }

        private static void LogCurrentMode()
        {
            switch (ModeSettings.Instance.OracleOperationMode)
            {
                case OracleOperationMode.FateGrind:
                    Logger.SendLog("Starting Oracle in FATE grind mode.");
                    break;
                case OracleOperationMode.LevelMode:
                    Logger.SendLog("Starting Oracle in single-class levelling mode.");
                    break;
                case OracleOperationMode.MultiLevelMode:
                    Logger.SendLog("Starting Oracle in multiple-class levelling mode.");
                    break;
                case OracleOperationMode.SpecificFates:
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
                    Logger.SendWarningLog("No setting chosen for operation mode. Defaulting to FATE grind mode.");
                    Logger.SendLog("Starting Oracle in FATE grind mode.");
                    break;
            }
        }

        public void OnButtonPress()
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
            OracleFateManager.FateDatabase = null;

            OracleMovementManager.ZoneFlightMesh = null;

            YokaiWatchGrind.ResetIgnoredYokai();
        }

        private static void SetUpHooks()
        {
            // Add Oracle's behaviour to the start of the behaviour tree.
            TreeHooks.Instance.AddHook("TreeStart", OracleBehaviour.Behaviour);

            // Clear unused or to be replaced hooks.
            TreeHooks.Instance.ClearHook("HotspotPoi");
            TreeHooks.Instance.ClearHook("SetHotspotPoi");
            TreeHooks.Instance.ClearHook("SetCombatPoi");

            // Replace with our own hooks.
            TreeHooks.Instance.ReplaceHook("SelectPoiType", SelectPoiType.Behaviour);

            if (MainSettings.Instance.OverrideRestBehaviour)
            {
                Logger.SendDebugLog("Replacing the combat routine's rest behaviour.");
                TreeHooks.Instance.ReplaceHook("Rest", Rest.Behaviour);
            }

            if (MainSettings.Instance.ListHooksOnStart)
            {
                ListHooks();
            }
        }

        public void Start()
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
            SetUpHooks();

            LogCurrentMode();
            WarnAboutPotentialIssues();
        }

        public void Stop()
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
                Logger.SendDebugLog("Restoring the combat routine's rest behaviour.");
                TreeHooks.Instance.ReplaceHook("Rest", RoutineManager.Current.RestBehavior);
            }

            Logger.SendLog("Stopping Oracle.");
        }

        internal static void StopOracle(string reason)
        {
            TreeRoot.Stop(" " + reason);
        }

        private static void WarnAboutPotentialIssues()
        {
            if (PluginManager.GetEnabledPlugins().Contains("Enable Flight"))
            {
                Logger.SendWarningLog("Detected ExBuddy's flight plugin; it's advised that you do not run this and Oracle's own flight navigator together.");
            }

            if (Environment.Is64BitProcess)
            {
                Logger.SendWarningLog("Running Oracle in 64-bit mode. If you have any issues, please try running in 32-bit mode prior to seeking assistance.");
            }
        }
    }
}