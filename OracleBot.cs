using System;
using System.IO;
using System.Reflection;

using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Pathing.Service_Navigation;

using Oracle.Behaviour;
using Oracle.Behaviour.HookOverrides;
using Oracle.Helpers;
using Oracle.Managers;

using TreeSharp;

namespace Oracle
{
    public class OracleBot
    {
        private static bool playerFaceTargetOnAction;
        private static bool playerFlightMode;
        private static Composite root;

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

        private static void ClearBotVariables()
        {
            Poi.Clear("Oracle stopping.");
            OracleFateManager.ClearFate();
        }

        public Composite GetRoot()
        {
            return Root;
        }

        internal void Initialize()
        {
            Logger.SendLog("Initialising Oracle.");
        }

        public void OnButtonPress()
        {
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
        }

        public void Start()
        {
            Navigator.NavigationProvider = new ServiceNavigationProvider();
            Navigator.PlayerMover = new SlideMover();

            playerFaceTargetOnAction = GameSettingsManager.FaceTargetOnAction;
            playerFlightMode = GameSettingsManager.FlightMode;
            GameSettingsManager.FaceTargetOnAction = true;
            GameSettingsManager.FlightMode = true;

            TreeHooks.Instance.ClearAll();
            root = BrainBehavior.CreateBrain();
            SetUpHooks();
        }

        public void Stop()
        {
            (Navigator.NavigationProvider as ServiceNavigationProvider)?.Dispose();
            Navigator.NavigationProvider = null;

            CombatTargeting.Instance.Provider = new DefaultCombatTargetingProvider();
            Blacklist.Flush();
            ChocoboManager.BlockSummon = false;

            GameSettingsManager.FaceTargetOnAction = playerFaceTargetOnAction;
            GameSettingsManager.FlightMode = playerFlightMode;

            ClearBotVariables();

            Logger.SendLog("Stopping Oracle.");
        }
    }
}