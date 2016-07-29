using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Hooks.FateSelect
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
            if (MovementSettings.Instance.TeleportIfQuicker && MovementSettings.Instance.ConsiderAetheryteFateDistances)
            {
                Logger.SendDebugLog("Taking into account the distance to a FATE if we teleport to it.");

                var combinedDistances = fateDistances;
                var fateDistancesFromAetherytes = new List<Dictionary<FateData, float>>();
                foreach (var aetheryte in
                    OracleFateManager.GetAetheryteIdsForZone(WorldManager.ZoneId).Where(item => WorldManager.KnownAetheryteIds.Contains(item.Item1)))
                {
                    fateDistancesFromAetherytes.Add(await OracleFateManager.GetActiveFateDistances(aetheryte.Item2));
                }

                foreach (var fateDistance in await OracleFateManager.GetActiveFateDistances())
                {
                    if (fateDistance.Key.Progress < MovementSettings.Instance.FateProgressTeleportLimit)
                    {
                        continue;
                    }

                    Logger.SendDebugLog("Ignoring teleport distance for " + fateDistance.Key.Name + ", its progress (" + fateDistance.Key.Progress
                                        + "%) equals or exceeds " + MovementSettings.Instance.FateProgressTeleportLimit + "%.");
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

                        if (fateDistance.Key.Progress < MovementSettings.Instance.FateProgressTeleportLimit)
                        {
                            continue;
                        }

                        combinedDistances.Remove(fateDistance.Key);

                        // Add the minimum distance delta to ensure we don't teleport if distance saved is under that amount.
                        if (fateDistance.Value + MovementSettings.Instance.MinDistanceToTeleport >= currentDistance)
                        {
                            continue;
                        }

                        combinedDistances.Remove(fateDistance.Key);
                        combinedDistances.Add(fateDistance.Key, fateDistance.Value);
                    }
                }

                fateDistances = combinedDistances;
            }

            var closestFates = fateDistances.OrderBy(kvp => kvp.Value).Where(fate => OracleFateManager.FateFilter(fate.Key)).ToArray();
            foreach (var fate in closestFates)
            {
                if (fate.Value > 0)
                {
                    Logger.SendDebugLog("Found FATE '" + fate.Key.Name + "'. Distance to it is " + Math.Round(fate.Value, 2) + " yalms.");
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