using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class WaitInPlace
    {
        internal static async Task<bool> HandleWaitInPlace()
        {
            if (GameObjectManager.Attackers.Any(bc => !bc.IsFateGone))
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