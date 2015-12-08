/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

namespace Tarot
{
    using System;
    using System.Reflection;

    using ff14bot.AClasses;
    using ff14bot.Behavior;
    using ff14bot.Helpers;
    using ff14bot.Managers;
    using ff14bot.Navigation;

    using global::Tarot.Behaviour;
    using global::Tarot.Behaviour.Tasks.Hooks;
    using global::Tarot.Data;
    using global::Tarot.Enumerations;
    using global::Tarot.Forms;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    using TreeSharp;

    public class Tarot : BotBase
    {
        private static bool playerFaceTargetOnAction;

        private static bool playerFlightMode;

        private static Composite root;

        private static SettingsForm settingsForm;

        public override string Name
        {
            get
            {
                return "Tarot";
            }
        }

        public override string EnglishName
        {
            get
            {
                return "Tarot";
            }
        }

        public override PulseFlags PulseFlags
        {
            get
            {
                return PulseFlags.All;
            }
        }

        public override bool IsAutonomous
        {
            get
            {
                return true;
            }
        }

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override bool RequiresProfile
        {
            get
            {
                return false;
            }
        }

        public override Composite Root
        {
            get
            {
                return root;
            }
        }

        internal static Tarot Instance { get; set; }

        internal static FateData CurrentFate { get; set; }

        internal static FateData PreviousFate { get; set; }

        internal static FateIdleMode CurrentIdle { get; set; }

        internal static Poi CurrentPoi { get; set; }

        internal static FateDatabase FateDatabase { get; set; }

        internal static string Version
        {
            get
            {
                var versionString = Assembly.GetExecutingAssembly().GetName().Version.Major + "."
                                    + Assembly.GetExecutingAssembly().GetName().Version.Minor + "."
                                    + Assembly.GetExecutingAssembly().GetName().Version.Revision;
                return versionString;
            }
        }

        public override void Initialize()
        {
            // Set the botbase instance so we can access its data.
            Instance = this;

            Logger.SendLog("Initialising " + this.Name + ".");

            // TODO: Implement rest of Updater.
            if (Updater.UpdateIsAvailable())
            {
                Logger.SendLog("An update for " + this.Name + " is available.");
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
                Logger.SendDebugLog("ArgumentOutOfRangeException thrown.\n\n" + exception + "\n");
            }
        }

        public override void Start()
        {
            // Set the botbase instance so we can access its data.
            Instance = this;

            Navigator.PlayerMover = new SlideMover();
            Navigator.NavigationProvider = new GaiaNavigator();
            CombatTargeting.Instance.Provider = new FateCombatTargetingProvider();

            playerFaceTargetOnAction = GameSettingsManager.FaceTargetOnAction;
            playerFlightMode = GameSettingsManager.FlightMode;
            GameSettingsManager.FaceTargetOnAction = true;
            GameSettingsManager.FlightMode = true;

            TreeHooks.Instance.ClearAll();
            root = BrainBehavior.CreateBrain();
            TreeHooks.Instance.AddHook("TreeStart", Main.Behaviour);
            TreeHooks.Instance.ReplaceHook("SelectPoiType", SelectPoiType.Behaviour);

            // List hook structure.
            if (TarotSettings.Instance.ListHooksOnStart)
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

            Logger.SendLog("Starting " + this.Name + ".");
        }

        public override void Stop()
        {
            // Clean up all botbase internal variables.
            CurrentFate = null;
            PreviousFate = null;
            FateDatabase = null;
            CurrentPoi = null;

            var navProvider = Navigator.NavigationProvider as GaiaNavigator;
            if (navProvider != null)
            {
                navProvider.Dispose();
            }

            Navigator.NavigationProvider = null;
            CombatTargeting.Instance.Provider = new DefaultCombatTargetingProvider();

            GameSettingsManager.FaceTargetOnAction = playerFaceTargetOnAction;
            GameSettingsManager.FlightMode = playerFlightMode;

            Logger.SendLog("Stopping " + this.Name + ".");
        }
    }
}