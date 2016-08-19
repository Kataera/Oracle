using System;
using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class GrindMobs
    {
        internal static async Task<bool> HandleGrindMobs()
        {
            if (GameObjectManager.Attackers.Any(bc => !bc.IsFateGone) && Poi.Current.Type != PoiType.Kill)
            {
                OracleFateManager.ClearPoi("We're being attacked.", false);
                return true;
            }

            if (OracleClassManager.ClassChangedTimer != null && OracleClassManager.ClassChangedTimer.Elapsed < TimeSpan.FromSeconds(30))
            {
                Logger.SendLog("Waiting for class change skill cooldown to expire before selecting a target.");
                return true;
            }

            var target = await SelectGrindTarget.HandleSelectGrindTarget();
            if (target == null)
            {
                return true;
            }

            Logger.SendLog("Selecting '" + target.Name + "' as the next target to kill.", true);
            Poi.Current = new Poi(target, PoiType.Kill);
            return true;
        }
    }
}