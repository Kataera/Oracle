using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class GrindMobs
    {
        internal static async Task<bool> HandleGrindMobs()
        {
            if (GameObjectManager.Attackers.Any(bc => !bc.IsFateGone) && Poi.Current.Type != PoiType.Kill && Poi.Current.Type != PoiType.None)
            {
                OracleFateManager.ClearPoi("We're being attacked.", false);
                return true;
            }

            await OracleCombatManager.SelectGrindTarget();
            return true;
        }
    }
}