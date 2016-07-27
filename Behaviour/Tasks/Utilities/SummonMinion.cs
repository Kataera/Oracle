using System;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Managers;

using Oracle.Helpers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class SummonMinion
    {
        public static async Task<bool> Main(string minionName)
        {
            Logger.SendDebugLog("Summoning " + minionName + ".");

            // Only way to summon minion with current RebornBuddy API is through a chat command.
            ChatManager.SendChat("/minion");
            await Coroutine.Sleep(TimeSpan.FromSeconds(1));
            ChatManager.SendChat("/minion \"" + minionName + "\"");
            await Coroutine.Sleep(TimeSpan.FromSeconds(5));

            // For some reason, the player's minion always has an ObjectId of 1.
            var minionObject = GameObjectManager.GetObjectByObjectId(1);
            if (minionObject != null)
            {
                if (minionObject.EnglishName.Equals(minionName))
                {
                    Logger.SendLog(minionName + " has been summoned successfully!");
                    return true;
                }
                else
                {
                    Logger.SendLog("The wrong minion is currently active, assuming " + minionName + " has not been learned yet.");
                    return false;
                }
            }
            else
            {
                Logger.SendErrorLog("No pet active, assuming " + minionName + " has not been learned yet.");
                return false;
            }
        }
    }
}