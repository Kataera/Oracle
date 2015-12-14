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

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Objects;

using Tarot.Helpers;

namespace Tarot.Behaviour.Tasks.Utilities
{
    internal static class BlacklistMob
    {
        private static Stopwatch blacklistTimer;
        private static BattleCharacter currentTarget;
        private static uint lastHealthLevel;

        public static async Task<bool> IsBlacklistNeeded()
        {
            if (blacklistTimer == null)
            {
                blacklistTimer = new Stopwatch();
            }

            if (Poi.Current.BattleCharacter == null || !Poi.Current.BattleCharacter.IsValid)
            {
                TarotBehaviour.ClearPoi("Target is no longer valid.", false);
                Core.Player.ClearTarget();
                return true;
            }

            if (currentTarget == null || currentTarget != Poi.Current.BattleCharacter)
            {
                currentTarget = Poi.Current.BattleCharacter;
                lastHealthLevel = currentTarget.CurrentHealth;
                blacklistTimer.Restart();

                return false;
            }

            if (currentTarget.CurrentHealth < lastHealthLevel)
            {
                lastHealthLevel = Poi.Current.BattleCharacter.CurrentHealth;
                blacklistTimer.Restart();

                return false;
            }

            if (blacklistTimer.Elapsed >= TimeSpan.FromSeconds(30))
            {
                Logger.SendLog("Current target's HP has not decreased in 30 seconds, blacklisting and moving on.");
                Blacklist.Add(currentTarget.ObjectId, BlacklistFlags.Combat, TimeSpan.FromMinutes(15),
                    "Target's HP has not changed in too long.");
                TarotBehaviour.ClearPoi("Target's HP has not changed in too long.");
                Core.Player.ClearTarget();
            }

            return true;
        }
    }
}