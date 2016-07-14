using System;
using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.PoiHooks.FateSelect
{
    internal static class Closest
    {
        public static async Task<bool> Main()
        {
            if (!await OracleFateManager.AnyViableFates())
            {
                return false;
            }

            var activeFates = await OracleFateManager.GetActiveFateDistances();
            var closestFates =
                activeFates.OrderBy(kvp => kvp.Value - (kvp.Key.Radius * 0.75)).Where(fate => OracleFateManager.FateFilter(fate.Key));
            foreach (var fate in closestFates)
            {
                var distance = Math.Round(fate.Value - (fate.Key.Radius * 0.75f), 0);

                if (distance > 0)
                {
                    Logger.SendDebugLog("Found FATE '" + fate.Key.Name + "'. Distance to it is ~" + distance + " yalms.");
                }
                else
                {
                    Logger.SendDebugLog("Found FATE '" + fate.Key.Name + "'. Distance to it is 0 yalms.");
                }
            }

            if (!closestFates.Any())
            {
                return false;
            }

            Logger.SendLog("Selecting closest FATE.");
            var closestFate = closestFates.FirstOrDefault().Key;

            Logger.SendLog("Selected FATE: '" + closestFate.Name + "'.");
            Logger.SendDebugLog("Location of FATE: " + closestFate.Location);
            OracleFateManager.CurrentFateId = closestFate.Id;
            Poi.Current = new Poi(closestFate, PoiType.Fate);

            return true;
        }
    }
}