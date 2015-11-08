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

namespace Tarot.Behaviour.Handlers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using ff14bot;
    using ff14bot.Managers;
    using ff14bot.RemoteWindows;

    using global::Tarot.Behaviour.Selectors;
    using global::Tarot.Helpers;

    using TreeSharp;

    internal static class FateHandler
    {
        private static Stopwatch levelSyncCooldown;

        public static Composite Behaviour
        {
            get
            {
                return CreateBehaviour();
            }
        }

        private static bool LevelSyncNeeded()
        {
            if (MainBehaviour.CurrentRbFate == null)
            {
                return false;
            }

            return Core.Player.ClassLevel > MainBehaviour.CurrentRbFate.MaxLevel;
        }

        private static async Task<bool> LevelSyncTask()
        {
            if (MainBehaviour.CurrentRbFate == null)
            {
                return false;
            }

            if (GameObjectManager.LocalPlayer.IsLevelSynced)
            {
                Logger.SendDebugLog("Already level synced, escaping task.");
                return true;
            }

            if (levelSyncCooldown == null)
            {
                levelSyncCooldown = new Stopwatch();
            }

            if (levelSyncCooldown.ElapsedMilliseconds > 2000 || !levelSyncCooldown.IsRunning)
            {
                int level = MainBehaviour.CurrentRbFate.MaxLevel;
                var name = MainBehaviour.CurrentRbFate.Name;
                Logger.SendLog("Syncing to level " + level + " to participate in " + name + ".");

                ToDoList.LevelSync();
                levelSyncCooldown.Reset();
                levelSyncCooldown.Start();
            }

            return true;
        }

        private static Composite CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new Decorator(
                    check => LevelSyncNeeded(),
                    new ActionRunCoroutine(coroutine => LevelSyncTask())),
                FateTypeSelector.Behaviour
            };
            return new Sequence(behaviours);
        }
    }
}