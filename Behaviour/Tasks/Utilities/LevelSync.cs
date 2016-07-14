using System.Threading.Tasks;

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

            return fate.MaxLevel < Core.Player.ClassLevel && !Core.Player.IsLevelSynced && fate.Within2D(Core.Player.Location);
        }

        public static async Task<bool> Main(FateData fate)
        {
            if (!IsLevelSyncNeeded(fate))
            {
                return false;
            }

            ToDoList.LevelSync();
            if (Core.Player.IsLevelSynced)
            {
                Logger.SendLog("Synced to level " + fate.MaxLevel + " to participate in FATE.");
            }

            return true;
        }
    }
}