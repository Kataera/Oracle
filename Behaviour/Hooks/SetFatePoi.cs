using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Hooks
{
    internal static class SetFatePoi
    {
        private static Stopwatch chainFateTimer;

        private static async Task<Dictionary<FateData, float>> GetFateDistances()
        {
            if (!MovementSettings.Instance.TeleportIfQuicker || !MovementSettings.Instance.ConsiderAetheryteFateDistances || WorldManager.CanFly)
            {
                return await OracleFateManager.GetActiveFateDistances();
            }

            Logger.SendDebugLog("Taking into account the distance to a FATE if we teleport to it.");
            var fateDistances = await OracleFateManager.GetActiveFateDistances();
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
                                    + "%) equals or exceeds the limit (" + MovementSettings.Instance.FateProgressTeleportLimit + "%).");
            }

            foreach (var aetheryteDistanceDict in fateDistancesFromAetherytes)
            {
                foreach (var fateDistance in aetheryteDistanceDict)
                {
                    float currentDistance;
                    if (!fateDistances.TryGetValue(fateDistance.Key, out currentDistance))
                    {
                        continue;
                    }

                    if (fateDistance.Key.Progress >= MovementSettings.Instance.FateProgressTeleportLimit)
                    {
                        continue;
                    }

                    // Add the minimum distance delta to ensure we don't teleport if distance saved is under that amount.
                    if (fateDistance.Value + MovementSettings.Instance.MinDistanceToTeleport >= currentDistance)
                    {
                        continue;
                    }

                    fateDistances.Remove(fateDistance.Key);
                    fateDistances.Add(fateDistance.Key, fateDistance.Value);
                }
            }

            return fateDistances;
        }

        private static bool IsFatePoiSet()
        {
            if (Poi.Current.Type != PoiType.Fate || Poi.Current.Fate.Id != OracleFateManager.GetCurrentFateData().Id)
            {
                return false;
            }

            return true;
        }

        private static bool IsFateSet()
        {
            if (OracleFateManager.CurrentFateId == 0)
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> Main()
        {
            if (CommonBehaviors.IsLoading)
            {
                return false;
            }

            if (IsFateSet())
            {
                if (OracleFateManager.GetCurrentFateData() == null)
                {
                    return true;
                }

                if (!IsFatePoiSet() && Poi.Current.Type != PoiType.Death && !GameObjectManager.Attackers.Any())
                {
                    Poi.Current = new Poi(OracleFateManager.GetCurrentFateData(), PoiType.Fate);
                }

                return true;
            }

            if (OracleClassManager.ZoneChangeNeeded())
            {
                return false;
            }

            if (OracleFateManager.PreviousFateChained() && ModeSettings.Instance.OracleOperationMode != OracleOperationMode.SpecificFates)
            {
                await SelectChainFate();
            }
            else if (ModeSettings.Instance.OracleOperationMode == OracleOperationMode.SpecificFates)
            {
                await SelectSpecificFate();
            }
            else
            {
                await SelectClosestFate();
            }

            if (OracleFateManager.GetCurrentFateData() != null && MovementSettings.Instance.DelayFateMovement && !OracleFateManager.DoNotWaitBeforeMovingFlag)
            {
                await WaitBeforeMoving();
            }

            OracleFateManager.DoNotWaitBeforeMovingFlag = false;
            return IsFateSet() && IsFatePoiSet();
        }

        private static async Task<bool> SelectChainFate()
        {
            if (!OracleFateManager.WaitingForChainFate())
            {
                return false;
            }

            var chainId = OracleFateManager.FateDatabase.GetFateFromId(OracleFateManager.PreviousFateId).ChainId;
            var chainOracleFateInfo = OracleFateManager.FateDatabase.GetFateFromId(chainId);

            if (chainFateTimer == null || !chainFateTimer.IsRunning)
            {
                chainFateTimer = Stopwatch.StartNew();
            }
            else if (chainFateTimer.Elapsed > TimeSpan.FromSeconds(FateSettings.Instance.ChainWaitTimeout))
            {
                Logger.SendLog("Timed out waiting for the next FATE in the chain to appear.");
                OracleFateManager.PreviousFateId = 0;
                chainFateTimer.Reset();
            }

            Logger.SendLog("Waiting for the follow up FATE.");
            var chainFateData = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainId);
            if (chainFateData == null)
            {
                return false;
            }

            // Fix for FATEs that spawn instantly after the previous ends.
            if (chainFateData.Name.Equals(string.Empty))
            {
                Logger.SendLog("Selected FATE: '" + chainOracleFateInfo.Name + "'.");
            }
            else
            {
                Logger.SendLog("Selected FATE: '" + chainFateData.Name + "'.");
            }

            OracleFateManager.CurrentFateId = chainFateData.Id;
            Poi.Current = new Poi(chainFateData, PoiType.Fate);
            chainFateTimer.Reset();
            return true;
        }

        private static async Task<bool> SelectClosestFate()
        {
            if (!await OracleFateManager.AnyViableFates())
            {
                return false;
            }

            var fateDistances = await GetFateDistances();
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

        private static async Task<bool> SelectSpecificFate()
        {
            var specificFate = FateManager.ActiveFates.FirstOrDefault(result => FateSettings.Instance.SpecificFateList.Contains(result.Id));
            if (specificFate == null)
            {
                return false;
            }

            Logger.SendLog("Selected FATE: '" + specificFate.Name + "'.");
            OracleFateManager.CurrentFateId = specificFate.Id;
            Poi.Current = new Poi(specificFate, PoiType.Fate);
            return true;
        }

        private static async Task<bool> WaitBeforeMoving()
        {
            var rng = new Random();
            var currentFate = OracleFateManager.GetCurrentFateData();
            var minTime = MovementSettings.Instance.DelayFateMovementMin * 1000;
            var maxTime = MovementSettings.Instance.DelayFateMovementMax * 1000;
            var randomWaitTime = rng.Next(minTime, maxTime);

            Logger.SendLog("Waiting " + Math.Round(randomWaitTime / 1000f, 2) + " seconds before moving to FATE.");
            await Coroutine.Wait(randomWaitTime, () => currentFate.Status == FateStatus.NOTACTIVE || Core.Player.InCombat);

            return true;
        }
    }
}