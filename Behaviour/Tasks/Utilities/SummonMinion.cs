using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Objects;

using Oracle.Helpers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class SummonMinion
    {
        public static GameObject CurrentMinion => GameObjectManager.GetObjectByObjectId(1);

        public static bool CanSummonMinion()
        {
            if (Core.Player.IsMounted)
            {
                return false;
            }

            if (Core.Player.InCombat)
            {
                return false;
            }

            if (GameObjectManager.Attackers.Any())
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> IsMinionSummoned(string minionName)
        {
            if (Core.Player.IsMounted)
            {
                return true;
            }

            RefreshObjectManagerCache();
            return CurrentMinion != null && CurrentMinion.EnglishName.Equals(minionName);
        }

        public static async Task<SummonMinionResult> Main(string minionName)
        {
            Logger.SendLog("Summoning " + minionName + ".");

            if (Core.Player.IsMounted)
            {
                await CommonTasks.StopAndDismount();
            }

            if (CurrentMinion != null)
            {
                ChatManager.SendChat("/minion");
                await Coroutine.Wait(TimeSpan.FromSeconds(5), () => CurrentMinion == null);
            }

            // Only way to summon minion with current RebornBuddy API is through a chat command.
            ChatManager.SendChat("/minion \"" + minionName + "\"");
            await Coroutine.Wait(TimeSpan.FromSeconds(5), () => CurrentMinion != null);

            RefreshObjectManagerCache();
            if (CurrentMinion != null)
            {
                if (CurrentMinion.EnglishName.Equals(minionName))
                {
                    Logger.SendLog(minionName + " has been summoned successfully!");
                    return SummonMinionResult.Success;
                }

                Logger.SendLog("The wrong minion is currently active, expected " + minionName + ".");
                return SummonMinionResult.WrongMinionActive;
            }

            Logger.SendErrorLog("No minion active, expected " + minionName + ".");
            return SummonMinionResult.NoMinionActive;
        }

        private static void RefreshObjectManagerCache()
        {
            GameObjectManager.Clear();
            GameObjectManager.Update();
        }
    }

    internal enum SummonMinionResult
    {
        Success,
        WrongMinionActive,
        NoMinionActive
    }
}