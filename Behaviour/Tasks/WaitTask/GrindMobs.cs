using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class GrindMobs
    {
        public static async Task<bool> Main()
        {
            if (Core.Player.InCombat && Poi.Current.Type != PoiType.Kill)
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