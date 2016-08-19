using System.Linq;
using System.Threading.Tasks;

using ff14bot;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    internal static class SpecificFates
    {
        internal static async Task<bool> HandleSpecificFates()
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