/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Reflection;

using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using Oracle.Behaviour;
using Oracle.Behaviour.PoiHooks;
using Oracle.Enumerations;
using Oracle.Forms;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Providers;
using Oracle.Settings;

using TreeSharp;

namespace Oracle
{
    public class Oracle : BotBase
    {
        private static bool playerFaceTargetOnAction;

        private static bool playerFlightMode;

        private static bool reenableFlightPlugin;

        private static Composite root;

        private static SettingsForm settingsForm;

        public override string EnglishName
        {
            get { return "Oracle"; }
        }

        public override bool IsAutonomous
        {
            get { return true; }
        }

        public override string Name
        {
            get { return "Oracle"; }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override bool RequiresProfile
        {
            get { return false; }
        }

        public override Composite Root
        {
            get { return root; }
        }

        public override bool WantButton
        {
            get { return true; }
        }

        internal static string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.Major + "."
                       + Assembly.GetExecutingAssembly().GetName().Version.Minor + "."
                       + Assembly.GetExecutingAssembly().GetName().Version.Revision;
            }
        }

        public override void Initialize()
        {
            Logger.SendLog("Initialising Oracle.");

            // TODO: Implement rest of Updater.
            if (Updater.UpdateIsAvailable())
            {
                Logger.SendLog("An update for Oracle is available.");
            }
        }

        public override void OnButtonPress()
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new SettingsForm();
            }

            try
            {
                settingsForm.Show();
                settingsForm.Activate();
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Logger.SendErrorLog("Error opening the settings window.");
                Logger.SendDebugLog("ArgumentOutOfRangeException thrown.\n\n" + exception);
            }
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
            OracleManager.CurrentFateId = 0;
            OracleManager.PreviousFateId = 0;
            OracleManager.OracleDatabase = null;
            OracleManager.ZoneFlightMesh = null;

            var navProvider = Navigator.NavigationProvider as GaiaNavigator;
            if (navProvider != null)
            {
                navProvider.Dispose();
            }

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