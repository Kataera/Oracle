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

using Oracle.Behaviour.PoiHooks.FateSelect;
using Oracle.Data;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.PoiHooks
{
    internal static class SetFatePoi
    {
        private static Stopwatch chainFateTimer;

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

            if (ZoneChangeNeeded())
            {
                return false;
            }

            if (PreviousFateChained() && OracleSettings.Instance.OracleOperationMode != OracleOperationMode.SpecificFate)
            {
                await SelectChainFate();
                return true;
            }

            if (OracleSettings.Instance.OracleOperationMode == OracleOperationMode.SpecificFate)
            {
                await SelectSpecificFate();
            }
            else
            {
                await SelectFate();
            }

            if (OracleFateManager.GetCurrentFateData() != null && OracleSettings.Instance.FateDelayMovement
                && !OracleFateManager.DoNotWaitBeforeMovingFlag)
            {
                await WaitBeforeMoving();
            }

            OracleFateManager.DoNotWaitBeforeMovingFlag = false;
            return IsFateSet() && IsFatePoiSet();
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

        private static bool IsFateTypeEnabled(Fate oracleFate)
        {
            switch (oracleFate.Type)
            {
                case FateType.Kill:
                    return OracleSettings.Instance.KillFatesEnabled;
                case FateType.Collect:
                    return OracleSettings.Instance.CollectFatesEnabled;
                case FateType.Escort:
                    return OracleSettings.Instance.EscortFatesEnabled;
                case FateType.Defence:
                    return OracleSettings.Instance.DefenceFatesEnabled;
                case FateType.Boss:
                    return OracleSettings.Instance.BossFatesEnabled;
                case FateType.MegaBoss:
                    return OracleSettings.Instance.MegaBossFatesEnabled;
                case FateType.Null:
                    return true;
            }

            return true;
        }

        private static bool PreviousFateChained()
        {
            if (OracleFateManager.PreviousFateId == 0)
            {
                return false;
            }

            if (OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.PreviousFateId).ChainId != 0)
            {
                return true;
            }

            return false;
        }

        private static async Task<bool> SelectChainFate()
        {
            if (OracleFateManager.PreviousFateId == 0)
            {
                return false;
            }

            var chainId = OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.PreviousFateId).ChainId;
            if (chainId == 0)
            {
                return false;
            }

            var chainOracleFateInfo = OracleFateManager.OracleDatabase.GetFateFromId(chainId);
            if (!IsFateTypeEnabled(chainOracleFateInfo))
            {
                Logger.SendLog("Not waiting for the next FATE in the chain: its type is not enabled.");
                return false;
            }

            if (OracleSettings.Instance.BlacklistedFates.Contains(chainId))
            {
                Logger.SendLog("Not waiting for the next FATE in the chain: it is contained in the user blacklist.");
                return false;
            }

            if (chainFateTimer == null || !chainFateTimer.IsRunning)
            {
                chainFateTimer = Stopwatch.StartNew();
            }
            else if (chainFateTimer.Elapsed > TimeSpan.FromSeconds(OracleSettings.Instance.ChainFateWaitTimeout))
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
            switch (OracleSettings.Instance.FateSelectMode)
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
            var specificFate = FateManager.ActiveFates.FirstOrDefault(result => result.Name.Equals(OracleSettings.Instance.SpecificFates));

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
            var minTime = OracleSettings.Instance.FateDelayMovementMinimum * 1000;
            var maxTime = OracleSettings.Instance.FateDelayMovementMaximum * 1000;
            var randomWaitTime = rng.Next(minTime, maxTime);

            Logger.SendLog("Waiting " + Math.Round(randomWaitTime / 1000f, 2) + " seconds before moving to FATE.");
            await Coroutine.Wait(randomWaitTime, () => currentFate.Status == FateStatus.NOTACTIVE || Core.Player.InCombat);

            return true;
        }

        private static bool ZoneChangeNeeded()
        {
            const ushort dravanianHinterlands = 399;
            const ushort idyllshire = 478;

            uint aetheryteId;
            OracleSettings.Instance.ZoneLevels.TryGetValue(Core.Player.ClassLevel, out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                return false;
            }

            // Special case for Idyllshire/Dravanian Hinterlands.
            if (WorldManager.ZoneId == dravanianHinterlands && WorldManager.GetZoneForAetheryteId(aetheryteId) == idyllshire)
            {
                return false;
            }

            if (WorldManager.GetZoneForAetheryteId(aetheryteId) == WorldManager.ZoneId)
            {
                return false;
            }

            return true;
        }
    }
}