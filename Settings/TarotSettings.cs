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
    using System.ComponentModel;
    using System.Configuration;
    using System.IO;

    using ff14bot.Helpers;

    internal sealed class TarotSettings : JsonSettings
    {
        private static readonly object SyncRootObject = new object();

        private static volatile TarotSettings instance;

        // Movement Page
        private bool aetheryteIfCloserEnabled;

        // Boss FATEs Page
        private bool bossFatesAvoidAggro;

        private bool bossFatesEnabled;

        private int bossFatesMaxLevelAbove;

        private int bossFatesMinLevelBelow;

        private int bossFatesPercentageEngage;

        // Collect FATEs Page
        private bool collectFatesEnabled;

        private int collectFatesMaxLevelAbove;

        private int collectFatesMinLevelBelow;

        // Miscellaneous Page
        private bool debugEnabled;

        // Defence FATEs Page
        private bool defenceFatesEnabled;

        private int defenceFatesMaxLevelAbove;

        private int defenceFatesMinLevelBelow;

        // Escort FATEs Page
        private bool escortFatesEnabled;

        private int escortFatesMaxLevelAbove;

        private int escortFatesMinLevelBelow;

        private int escortFatesNpcRangeMax;

        private int escortFatesNpcRangeMin;

        private bool exBuddyFlightEnabled;

        private int fateSelectionMode;

        private int ignoreFatesAbovePercentage;

        // FATE Settings Tab
        // Kill FATEs Page
        private bool killFatesEnabled;

        private int killFatesMaxLevelAbove;

        private int killFatesMinLevelBelow;

        private bool logXpPerHour;

        // Mega-Boss FATEs Page
        private bool megaBossFatesAvoidAggro;

        private bool megaBossFatesEnabled;

        private int megaBossFatesPercentageEngage;

        private int megaFatesMaxLevelAbove;

        private int megaFatesMinLevelBelow;

        private bool moveToBossFates;

        // Patrol Page
        private bool patrolEnabled;

        private int patrolMaxLevelAbove;

        private int patrolMinLevelBelow;

        // General Settings Tab
        // Fate Selection Page
        private bool prioritiseChainFates;

        private bool prioritiseMegaBossFates;

        private bool recordFateData;

        // Scheduler Page
        private bool schedulerEnabled;

        private int stopAfterDuration;

        private bool stopAfterDurationEnabled;

        private int stopAfterLevel;

        private bool stopAfterLevelEnabled;

        // Bot Mode Page
        private int tarotMode;

        private bool waitForChainFates;

        private TarotSettings()
            : base(Path.Combine(CharacterSettingsDirectory, "TarotSettings.json")) {}

        public static TarotSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRootObject)
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

        //// TODO: Remaining settings.
        //// TODO: Rest of the accessors/mutators.

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
    }
}