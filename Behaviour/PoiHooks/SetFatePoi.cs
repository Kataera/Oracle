/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

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
                if (!IsFatePoiSet() && Poi.Current.Type != PoiType.Death && !GameObjectManager.Attackers.Any())
                {
                    if (OracleFateManager.GetCurrentFateData() != null)
                    {
                        Poi.Current = new Poi(OracleFateManager.GetCurrentFateData(), PoiType.Fate);
                    }
                }

                return true;
            }

            OracleFateManager.CurrentFateId = 0;

            if (ZoneChangeNeeded())
            {
                return false;
            }

            if (PreviousFateChained())
            {
                await SelectChainFate();
                return true;
            }

            switch (OracleSettings.Instance.FateSelectMode)
            {
                case FateSelectMode.Closest:
                    await Closest.Main();
                    break;

                case FateSelectMode.TypePriority:

                    // TODO: Implement.
                    await Closest.Main();
                    break;

                case FateSelectMode.ChainPriority:

                    // TODO: Implement.
                    await Closest.Main();
                    break;

                case FateSelectMode.TypeAndChainPriority:

                    // TODO: Implement.
                    await Closest.Main();
                    break;

                default:
                    Logger.SendDebugLog("Cannot determine FATE selection strategy, defaulting to closest FATE.");
                    await Closest.Main();
                    break;
            }

            if (OracleFateManager.GetCurrentFateData() != null && OracleSettings.Instance.FateDelayMovement
                && !OracleFateManager.DoNotWaitBeforeMovingFlag)
            {
                await WaitBeforeMoving();
            }

            OracleFateManager.SetDoNotWaitFlag(false);
            return IsFateSet() && IsFatePoiSet();
        }

        private static bool IsFatePoiSet()
        {
            if (Poi.Current.Type != PoiType.Fate || Poi.Current.Fate != OracleFateManager.GetCurrentFateData())
            {
                return false;
            }

            return true;
        }

        private static bool IsFateSet()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
            if (currentFate == null)
            {
                return false;
            }

            if (currentFate.Status == FateStatus.NOTACTIVE)
            {
                return false;
            }

            var oracleFate = OracleFateManager.OracleDatabase.GetFateFromFateData(currentFate);
            if (currentFate.Status == FateStatus.COMPLETE && oracleFate.Type != FateType.Collect)
            {
                return false;
            }

            return true;
        }

        private static bool PreviousFateChained()
        {
            return PreviousFateChainedOnSuccess() || PreviousFateChainedOnFailure();
        }

        private static bool PreviousFateChainedOnFailure()
        {
            if (OracleFateManager.PreviousFateId == 0)
            {
                return false;
            }

            if (OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.PreviousFateId).ChainIdFailure != 0)
            {
                return true;
            }

            return false;
        }

        private static bool PreviousFateChainedOnSuccess()
        {
            if (OracleFateManager.PreviousFateId == 0)
            {
                return false;
            }

            if (OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.PreviousFateId).ChainIdSuccess != 0)
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

            var chainIdSuccess = OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.PreviousFateId).ChainIdSuccess;
            var chainIdFailure = OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.PreviousFateId).ChainIdFailure;

            // If there's a success chain only.
            if (chainIdSuccess != 0 && chainIdFailure == 0)
            {
                Logger.SendLog("Waiting for the follow up FATE.");
                var chainSuccess = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainIdSuccess);

                if (chainSuccess == null)
                {
                    return false;
                }

                Logger.SendLog("Selected FATE: '" + chainSuccess.Name + "'.");
                OracleFateManager.CurrentFateId = chainSuccess.Id;
                Poi.Current = new Poi(chainSuccess, PoiType.Fate);
                chainFateTimer.Reset();
                return true;
            }

            // If there's a fail chain only.
            if (chainIdSuccess == 0 && chainIdFailure != 0)
            {
                Logger.SendLog("Waiting for the follow up FATE.");
                var chainFail = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainIdFailure);

                if (chainFail == null)
                {
                    return false;
                }

                Logger.SendLog("Selected FATE: '" + chainFail.Name + "'.");
                OracleFateManager.CurrentFateId = chainFail.Id;
                Poi.Current = new Poi(chainFail, PoiType.Fate);
                chainFateTimer.Reset();
                return true;
            }

            // If there's both.
            if (chainIdSuccess != 0 && chainIdFailure != 0)
            {
                Logger.SendLog("Waiting for the follow up FATE.");
                var chainSuccess = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainIdSuccess);
                var chainFail = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainIdFailure);

                if (chainSuccess == null && chainFail == null)
                {
                    return false;
                }

                if (chainSuccess != null && chainFail == null)
                {
                    Logger.SendLog("Selected FATE: '" + chainSuccess.Name + "'.");
                    OracleFateManager.CurrentFateId = chainSuccess.Id;
                    Poi.Current = new Poi(chainSuccess, PoiType.Fate);
                    chainFateTimer.Reset();
                    return true;
                }

                Logger.SendLog("Selected FATE: '" + chainFail.Name + "'.");
                OracleFateManager.CurrentFateId = chainFail.Id;
                Poi.Current = new Poi(chainFail, PoiType.Fate);
                chainFateTimer.Reset();
                return true;
            }

            return false;
        }

        private static async Task<bool> WaitBeforeMoving()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
            var minTime = OracleSettings.Instance.FateDelayMovementMinimum * 1000;
            var maxTime = OracleSettings.Instance.FateDelayMovementMaximum * 1000;
            var randomWaitTime = new Random().Next(minTime, maxTime);

            Logger.SendLog("Waiting " + Math.Round(randomWaitTime / 1000f, 2) + " seconds before moving to FATE.");
            await Coroutine.Wait(randomWaitTime, () => currentFate.Status == FateStatus.NOTACTIVE || Core.Player.InCombat);

            return true;
        }

        private static bool ZoneChangeNeeded()
        {
            uint aetheryteId = 0;
            OracleSettings.Instance.ZoneLevels.TryGetValue(Core.Player.ClassLevel, out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
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