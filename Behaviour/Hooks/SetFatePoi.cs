using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Hooks.FateSelect;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Hooks
{
    internal static class SetFatePoi
    {
        private static Stopwatch chainFateTimer;

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
                await SelectFate();
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

        private static async Task<bool> SelectFate()
        {
            switch (FateSettings.Instance.FateSelectMode)
            {
                case FateSelectMode.Closest:
                    await Closest.Main();
                    return true;
                case FateSelectMode.TypePriority:
                    await Closest.Main();
                    return true;
                case FateSelectMode.ChainPriority:
                    await Closest.Main();
                    return true;
                case FateSelectMode.TypeAndChainPriority:
                    await Closest.Main();
                    return true;
                default:
                    Logger.SendDebugLog("Cannot determine FATE selection strategy, defaulting to closest FATE.");
                    await Closest.Main();
                    return true;
            }
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