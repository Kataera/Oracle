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

namespace Tarot.Settings
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.IO;

    using Clio.Utilities;

    using ff14bot.Helpers;

    internal sealed class TarotSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();

        private static volatile TarotSettings instance;

        private bool debugEnabled;

        private bool exBuddyFlightEnabled;

        private int fateIdleMode;

        private int fateSelectMode;

        public Dictionary<ushort, Vector3> FateWaitLocations;

        private bool waitForChainFates;

        private bool listHooksOnStart;

        private TarotSettings()
            : base(Path.Combine(CharacterSettingsDirectory, "TarotSettings.json"))
        {
            if (this.FateWaitLocations == null)
            {
                this.FateWaitLocations = new Dictionary<ushort, Vector3>();
            }
        }

        public static TarotSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TarotSettings();
                        }
                    }
                }

                return instance;
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DebugEnabled
        {
            get
            {
                return this.debugEnabled;
            }

            set
            {
                this.debugEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ExBuddyFlightEnabled
        {
            get
            {
                return this.exBuddyFlightEnabled;
            }

            set
            {
                this.exBuddyFlightEnabled = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool WaitForChainFates
        {
            get
            {
                return this.waitForChainFates;
            }

            set
            {
                this.waitForChainFates = value;
                this.Save();
            }
        }

        [DefaultValue(Enumerations.FateIdleMode.ReturnToAetheryte)]
        [Setting]
        public int FateIdleMode
        {
            get
            {
                return this.fateIdleMode;
            }

            set
            {
                this.fateIdleMode = value;
                this.Save();
            }
        }

        [DefaultValue(Enumerations.FateSelectMode.Closest)]
        [Setting]
        public int FateSelectMode
        {
            get
            {
                return this.fateSelectMode;
            }

            set
            {
                this.fateSelectMode = value;
                this.Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ListHooksOnStart
        {
            get
            {
                return this.listHooksOnStart;
            }

            set
            {
                this.listHooksOnStart = value;
                this.Save();
            }
        }
    }
}