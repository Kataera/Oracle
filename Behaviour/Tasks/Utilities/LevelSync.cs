/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

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

using System.Diagnostics;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

using Oracle.Helpers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class LevelSync
    {
        public static bool IsLevelSyncNeeded(FateData fate)
        {
            if (!fate.IsValid || fate.Status == FateStatus.NOTACTIVE || fate.Status == FateStatus.COMPLETE)
            {
                return false;
            }

            return fate.MaxLevel < Core.Player.ClassLevel && !Core.Player.IsLevelSynced;
        }

        public static async Task<bool> Main(FateData fate)
        {
            if (!IsLevelSyncNeeded(fate))
            {
                return false;
            }

            var levelSyncCooldown = new Stopwatch();
            while (!Core.Player.IsLevelSynced
                   && FateManager.WithinFate
                   && fate.Status != FateStatus.NOTACTIVE
                   && fate.Status != FateStatus.COMPLETE)
            {
                if (!levelSyncCooldown.IsRunning || levelSyncCooldown.ElapsedMilliseconds > 2000)
                {
                    ToDoList.LevelSync();
                    levelSyncCooldown.Restart();
                }

                await Coroutine.Yield();
            }

            Logger.SendLog("Synced to level " + fate.MaxLevel + " to participate in FATE.");
            levelSyncCooldown.Stop();
            return true;
        }
    }
}