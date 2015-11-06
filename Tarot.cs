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
    using ff14bot.Managers;
    using ff14bot.Navigation;

    using global::Tarot.Behaviour;
    using global::Tarot.Forms;
    using global::Tarot.Helpers;

    using TreeSharp;

    public class Tarot : BotBase
    {
        internal const string BotName = "Tarot";

        private bool playerFaceTargetOnAction;

        private bool playerFlightMode;

        private Composite root;

        private SettingsForm settingsForm;

        public static Tarot Instance { get; set; }

        public override string Name
        {
            get
            {
                return BotName;
            }
        }

        public override string EnglishName
        {
            get
            {
                return BotName;
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
                return this.root;
            }
        }

        public override void Initialize()
        {
            Logger.SendLog("Initialising " + BotName + ".");
            Instance = this;
            Updater.CheckForUpdates();
        }

        public override void OnButtonPress()
        {
            if (this.settingsForm == null || this.settingsForm.IsDisposed)
            {
                this.settingsForm = new SettingsForm();
            }

            try
            {
                this.settingsForm.Show();
                this.settingsForm.Activate();
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Logger.SendErrorLog("Error opening the settings window.");
                Logger.SendDebugLog("ArgumentOutOfRangeException thrown.\n\n" + exception + "\n");
            }
        }

        public override void Start()
        {
            Logger.SendLog("Starting " + BotName + ".");

            // TODO: Move this to navigator selector behaviour.
            // Set navigator.
            Navigator.PlayerMover = new SlideMover();
            Navigator.NavigationProvider = new GaiaNavigator();

            // Make sure game settings are correct for botbase.
            this.playerFaceTargetOnAction = GameSettingsManager.FaceTargetOnAction;
            this.playerFlightMode = GameSettingsManager.FlightMode;
            GameSettingsManager.FaceTargetOnAction = true;
            GameSettingsManager.FlightMode = true;

            // Insert botbase hooks.
            TreeHooks.Instance.ClearAll();
            TreeHooks.Instance.InsertHook("TreeStart", 0, MainBehaviour.Instance.Behaviour);

            // Set default root behaviour.
            this.root = BrainBehavior.CreateBrain();
        }

        public override void Stop()
        {
            Logger.SendLog("Stopping " + BotName + ".");

            // Dispose of the navigator if it exists.
            var navProvider = Navigator.NavigationProvider as GaiaNavigator;
            if (navProvider != null)
            {
                navProvider.Dispose();
            }

            Navigator.NavigationProvider = null;

            // Clear current fate.
            MainBehaviour.Instance.SetCurrentFate(null, null);

            // Restore player's game settings.
            GameSettingsManager.FaceTargetOnAction = this.playerFaceTargetOnAction;
            GameSettingsManager.FlightMode = this.playerFlightMode;
        }
    }
}