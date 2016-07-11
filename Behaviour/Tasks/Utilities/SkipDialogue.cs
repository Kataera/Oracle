using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class SkipDialogue
    {
        public static async Task<bool> Main()
        {
            if (!Talk.DialogOpen && !Talk.ConvoLock)
            {
                return false;
            }

            Logger.SendDebugLog("Skipping dialogue.");
            while (Talk.DialogOpen)
            {
                if (GameObjectManager.Attackers.Any())
                {
                    return false;
                }

                if (!Core.Player.HasTarget)
                {
                    return false;
                }

                Talk.Next();
                await Coroutine.Yield();
            }

            await Coroutine.Sleep(OracleSettings.Instance.ActionDelay);
            return true;
        }
    }
}