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

namespace Tarot.Behaviour.Tasks.Utilities
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Buddy.Coroutines;

    using ff14bot;
    using ff14bot.RemoteWindows;

    using global::Tarot.Helpers;

    internal static class LevelSync
    {
        private static Stopwatch levelSyncCooldown;

        public static async Task<bool> Task()
        {
            while (!Core.Player.IsLevelSynced && Tarot.CurrentFate.Within2D(Core.Player.Location))
            {
                if (levelSyncCooldown == null)
                {
                    ToDoList.LevelSync();
                    levelSyncCooldown = new Stopwatch();
                    levelSyncCooldown.Start();
                }

                if (levelSyncCooldown.ElapsedMilliseconds > 2000)
                {
                    ToDoList.LevelSync();
                    levelSyncCooldown.Restart();
                }

                await Coroutine.Yield();
            }

            Logger.SendLog("Synced level to " + Tarot.CurrentFate.MaxLevel + " to participate in FATE.");
            return true;
        }
    }
}