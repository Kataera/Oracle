using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

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

            var fateDistances = await OracleFateManager.GetActiveFateDistances();
            if (OracleSettings.Instance.TeleportIfQuicker && OracleSettings.Instance.FateSelectClosestDistanceConsiderAetheryte)
            {
                Logger.SendDebugLog("Taking into account the distance to a FATE if we teleport to it.");

                var combinedDistances = fateDistances;
                var fateDistancesFromAetherytes = new List<Dictionary<FateData, float>>();
                foreach (var aetheryte in
                    OracleFateManager.GetAetheryteIdsForZone(WorldManager.ZoneId).Where(item => WorldManager.KnownAetheryteIds.Contains(item.Item1)))
                {
                    fateDistancesFromAetherytes.Add(await OracleFateManager.GetActiveFateDistances(aetheryte.Item2));
                }

                // Don't teleport for FATEs above a set percentage.
                foreach (var fateDistance in combinedDistances)
                {
                    if (fateDistance.Key.Progress < OracleSettings.Instance.TeleportMaxFateProgress)
                    {
                        continue;
                    }

                    Logger.SendDebugLog("Ignoring teleport distance for " + fateDistance.Key.Name + ", its progress (" + fateDistance.Key.Progress
                                        + "%) exceeds " + OracleSettings.Instance.TeleportMaxFateProgress + "%.");
                    combinedDistances.Remove(fateDistance.Key);
                }

                foreach (var aetheryteDistanceDict in fateDistancesFromAetherytes)
                {
                    foreach (var fateDistance in aetheryteDistanceDict)
                    {
                        float currentDistance;
                        if (!combinedDistances.TryGetValue(fateDistance.Key, out currentDistance))
                        {
                            continue;
                        }

                        // Add the minimum distance delta to ensure we don't teleport if distance saved is under that amount.
                        if (fateDistance.Value + OracleSettings.Instance.TeleportMinimumDistanceDelta >= currentDistance)
                        {
                            continue;
                        }

                        combinedDistances.Remove(fateDistance.Key);
                        combinedDistances.Add(fateDistance.Key, fateDistance.Value);
                    }
                }

                fateDistances = combinedDistances;
            }

            var closestFates = fateDistances.OrderBy(kvp => kvp.Value - kvp.Key.Radius * 0.75).Where(fate => OracleFateManager.FateFilter(fate.Key));
            foreach (var fate in closestFates)
            {
                var distance = Math.Round(fate.Value - fate.Key.Radius * 0.75f, 0);

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