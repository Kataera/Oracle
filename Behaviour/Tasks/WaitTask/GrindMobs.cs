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
        public static async Task<bool> Main()
        {
            if (GameObjectManager.Attackers.Any(bc => !bc.IsFateGone) && Poi.Current.Type != PoiType.Kill)
            {
                OracleFateManager.ClearPoi("We're being attacked.", false);
                return true;
            }

            var target = await SelectGrindTarget.Main();
            if (target == null)
            {
                return true;
            }

            Logger.SendLog("Selecting '" + target.Name + "' as the next target to kill.");
            Poi.Current = new Poi(target, PoiType.Kill);
            return true;
        }
    }
}