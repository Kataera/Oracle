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

    using ff14bot.AClasses;
    using ff14bot.Behavior;
    using ff14bot.Helpers;
    using ff14bot.Managers;
    using ff14bot.Navigation;

    using global::Tarot.Behaviour;
    using global::Tarot.Data;
    using global::Tarot.Forms;
    using global::Tarot.Helpers;

    using TreeSharp;

    using static System.Reflection.Assembly;

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

        internal static Poi CurrentPoi { get; set; }

        internal static FateDatabase FateDatabase { get; set; }

        internal static Version Version
        {
            get
            {
                return GetExecutingAssembly().GetName().Version;
            }
        }

        public override void Initialize()
        {
            Logger.SendLog("Initialising " + this.Name + ".");

            // Check for updates
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
            Logger.SendLog("Starting " + this.Name + ".");

            // Set the botbase instance so we can access its data.
            Instance = this;

            // Set navigator.
            Navigator.PlayerMover = new SlideMover();
            Navigator.NavigationProvider = new GaiaNavigator();

            // Set target provider.
            CombatTargeting.Instance.Provider = new DefaultCombatTargetingProvider();

            // Make sure game settings are correct for botbase.
            playerFaceTargetOnAction = GameSettingsManager.FaceTargetOnAction;
            playerFlightMode = GameSettingsManager.FlightMode;
            GameSettingsManager.FaceTargetOnAction = true;
            GameSettingsManager.FlightMode = true;

            // Set root behaviour.
            root = Main.Behaviour;
        }

        public override void Stop()
        {
            Logger.SendLog("Stopping " + this.Name + ".");

            // Dispose of the navigator if it exists.
            var navProvider = Navigator.NavigationProvider as GaiaNavigator;
            if (navProvider != null)
            {
                navProvider.Dispose();
            }

            Navigator.NavigationProvider = null;

            // Reset the target provider.
            CombatTargeting.Instance.Provider = new DefaultCombatTargetingProvider();

            // Restore player's game settings.
            GameSettingsManager.FaceTargetOnAction = playerFaceTargetOnAction;
            GameSettingsManager.FlightMode = playerFlightMode;
        }
    }
}