using System.Linq;
using System.Threading.Tasks;

using ff14bot;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    internal static class SpecificFate
    {
        public static async Task<bool> Main()
        {
            if (!Core.Player.InCombat && !FateSettings.Instance.SpecificFateList.Any())
            {
                Logger.SendErrorLog("Please set at least one FATE to search for before starting the bot.");
                OracleBot.StopOracle("No FATE set.");
            }

            return true;
        }
    }
}