﻿/*
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using Clio.Utilities;
using ff14bot.Helpers;
using Tarot.Enumerations;

namespace Tarot.Settings
{
    internal sealed class TarotSettings : JsonSettings
    {
        private static readonly object SyncRoot = new object();
        private static volatile TarotSettings instance;

        public Dictionary<uint, Vector3> FateWaitLocations;

        private int bossEngagePercentage;

        private bool debugEnabled;

        private FateIdleMode fateIdleMode;

        private FateSelectMode fateSelectMode;

        private bool listHooksOnStart;

        private int megaBossEngagePercentage;

        private bool runProblematicFates;

        private bool waitAtFateForProgress;

        private bool waitForChainFates;

        private TarotSettings()
            : base(Path.Combine(CharacterSettingsDirectory, "TarotSettings.json"))
        {
            if (FateWaitLocations == null)
            {
                FateWaitLocations = new Dictionary<uint, Vector3>();
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

        [DefaultValue(0)]
        [Setting]
        public int BossEngagePercentage
        {
            get { return bossEngagePercentage; }

            set
            {
                bossEngagePercentage = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool DebugEnabled
        {
            get { return debugEnabled; }

            set
            {
                debugEnabled = value;
                Save();
            }
        }

        [DefaultValue(FateIdleMode.ReturnToAetheryte)]
        [Setting]
        public FateIdleMode FateIdleMode
        {
            get { return fateIdleMode; }

            set
            {
                fateIdleMode = value;
                Save();
            }
        }

        [DefaultValue(FateSelectMode.Closest)]
        [Setting]
        public FateSelectMode FateSelectMode
        {
            get { return fateSelectMode; }

            set
            {
                fateSelectMode = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool ListHooksOnStart
        {
            get { return listHooksOnStart; }

            set
            {
                listHooksOnStart = value;
                Save();
            }
        }

        [DefaultValue(20)]
        [Setting]
        public int MegaBossEngagePercentage
        {
            get { return megaBossEngagePercentage; }

            set
            {
                megaBossEngagePercentage = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool RunProblematicFates
        {
            get { return runProblematicFates; }

            set
            {
                runProblematicFates = value;
                Save();
            }
        }

        [DefaultValue(false)]
        [Setting]
        public bool WaitAtFateForProgress
        {
            get { return waitAtFateForProgress; }

            set
            {
                waitAtFateForProgress = value;
                Save();
            }
        }

        [DefaultValue(true)]
        [Setting]
        public bool WaitForChainFates
        {
            get { return waitForChainFates; }

            set
            {
                waitForChainFates = value;
                Save();
            }
        }
    }
}