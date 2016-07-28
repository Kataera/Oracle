using System;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;

using Oracle.Helpers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class SummonMinion
    {
        public static async Task<bool> IsMinionSummoned(string minionName)
        {
            if (Core.Player.IsMounted)
            {
                return true;
            }

            // Ensure GameObjectManager data is fresh.
            RefreshObjectManagerCache();

            // For some reason, the player's minion always has an ObjectId of 1.
            var minionObject = GameObjectManager.GetObjectByObjectId(1);
            return minionObject != null && minionObject.EnglishName.Equals(minionName);
        }

        public static async Task<SummonMinionResult> Main(string minionName)
        {
            Logger.SendLog("Summoning " + minionName + ".");

            if (Core.Player.IsMounted)
            {
                await CommonTasks.StopAndDismount();
            }

            if (GameObjectManager.GetObjectByObjectId(1) != null)
            {
                ChatManager.SendChat("/minion");
                await Coroutine.Wait(TimeSpan.FromSeconds(5), () => GameObjectManager.GetObjectByObjectId(1) == null);
            }

            // Only way to summon minion with current RebornBuddy API is through a chat command.
            ChatManager.SendChat("/minion \"" + minionName + "\"");
            await Coroutine.Wait(TimeSpan.FromSeconds(5), () => GameObjectManager.GetObjectByObjectId(1) != null);

            // Ensure GameObjectManager data is fresh.
            RefreshObjectManagerCache();

            // For some reason, the player's minion always has an ObjectId of 1.
            var minionObject = GameObjectManager.GetObjectByObjectId(1);
            if (minionObject != null)
            {
                if (minionObject.EnglishName.Equals(minionName))
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