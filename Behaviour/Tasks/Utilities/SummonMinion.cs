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
            // TODO: Check command and minion names in different locales.
            Logger.SendDebugLog("Summoning " + minionName + ".");
            ChatManager.SendChat("/minion \"" + minionName + "\"");
            await Coroutine.Sleep(TimeSpan.FromSeconds(5));

            return true;
        }
    }
}