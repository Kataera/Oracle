using System.Threading.Tasks;

using ff14bot.Helpers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class WaitInPlace
    {
        internal static async Task<bool> HandleWaitInPlace()
        {
            if (OracleCombatManager.IsPlayerBeingAttacked() && Poi.Current.Type != PoiType.Kill && Poi.Current.Type != PoiType.None)
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