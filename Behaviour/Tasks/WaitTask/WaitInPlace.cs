using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class WaitInPlace
    {
        public static async Task<bool> Main()
        {
            if (Core.Player.InCombat)
            {
                OracleFateManager.ClearPoi("We're being attacked.", false);
                return true;
            }

            if (Poi.Current.Type != PoiType.Wait)
            {
                return false;
            }

            if (await OracleFateManager.AnyViableFates())
            {
                OracleFateManager.ClearPoi("Found a FATE.");
            }

            return true;
        }
    }
}