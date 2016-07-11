using System.Threading.Tasks;

using ff14bot.Helpers;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class GrindMobs
    {
        public static async Task<bool> Main()
        {
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