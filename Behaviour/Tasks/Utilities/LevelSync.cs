using System;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class LevelSync
    {
        public static async Task<bool> DesyncLevel()
        {
            if (Core.Player.IsLevelSynced)
            {
                ToDoList.LevelSync();
            }

            return true;
        }

        public static bool IsLevelSyncNeeded(FateData fate)
        {
            if (!fate.IsValid || fate.Status == FateStatus.NOTACTIVE || fate.Status == FateStatus.COMPLETE)
            {
                return false;
            }

            return fate.MaxLevel < OracleFateManager.GetTrueLevel() && !Core.Player.IsLevelSynced && fate.Within2D(Core.Player.Location);
        }

        public static async Task<bool> SyncLevel(FateData fate)
        {
            if (!IsLevelSyncNeeded(fate))
            {
                return false;
            }

            ToDoList.LevelSync();
            await Coroutine.Wait(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay), () => Core.Player.IsLevelSynced);

            if (Core.Player.IsLevelSynced)
            {
                Logger.SendLog("Synced to level " + fate.MaxLevel + " to participate in FATE.");
            }

            return true;
        }
    }
}