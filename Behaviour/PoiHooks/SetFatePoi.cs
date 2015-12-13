/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;

using Tarot.Behaviour.PoiHooks.FateSelect;
using Tarot.Enumerations;
using Tarot.Helpers;
using Tarot.Managers;
using Tarot.Settings;

namespace Tarot.Behaviour.PoiHooks
{
    internal static class SetFatePoi
    {
        public static async Task<bool> Main()
        {
            if (IsFateSet())
            {
                if (!IsFatePoiSet() && Poi.Current.Type != PoiType.Death && !GameObjectManager.Attackers.Any() && !CommonBehaviors.IsLoading)
                {
                    Poi.Current = new Poi(TarotFateManager.CurrentFate, PoiType.Fate);
                }

                return true;
            }

            if (PreviousFateChained())
            {
                await SelectChainFate();
                return true;
            }

            switch (TarotSettings.Instance.FateSelectMode)
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

            if (TarotFateManager.CurrentFate != null && TarotSettings.Instance.FateDelayMovement
                && !TarotFateManager.DoNotWaitBeforeMovingFlag)
            {
                await WaitBeforeMoving();
            }

            TarotFateManager.SetDoNotWaitFlag(false);
            return IsFateSet() && IsFatePoiSet();
        }

        private static bool IsFatePoiSet()
        {
            if (Poi.Current.Type != PoiType.Fate || Poi.Current.Fate != TarotFateManager.CurrentFate)
            {
                return false;
            }

            return true;
        }

        private static bool IsFateSet()
        {
            if (TarotFateManager.CurrentFate == null)
            {
                return false;
            }

            var tarotFate = TarotFateManager.FateDatabase.GetFateWithId(TarotFateManager.CurrentFate.Id);
            if (TarotFateManager.CurrentFate.Status == FateStatus.COMPLETE && tarotFate.Type != FateType.Collect)
            {
                return false;
            }

            if (!TarotFateManager.CurrentFate.IsValid)
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
            if (TarotFateManager.PreviousFate == null)
            {
                return false;
            }

            if (TarotFateManager.FateDatabase.GetFateWithId(TarotFateManager.PreviousFate.Id).ChainIdFailure != 0)
            {
                return true;
            }

            return false;
        }

        private static bool PreviousFateChainedOnSuccess()
        {
            if (TarotFateManager.PreviousFate == null)
            {
                return false;
            }

            if (TarotFateManager.FateDatabase.GetFateWithId(TarotFateManager.PreviousFate.Id).ChainIdSuccess != 0)
            {
                return true;
            }

            return false;
        }

        private static async Task<bool> SelectChainFate()
        {
            if (TarotFateManager.PreviousFate == null)
            {
                return false;
            }

            var chainIdSuccess = TarotFateManager.FateDatabase.GetFateWithId(TarotFateManager.PreviousFate.Id).ChainIdSuccess;
            var chainIdFailure = TarotFateManager.FateDatabase.GetFateWithId(TarotFateManager.PreviousFate.Id).ChainIdFailure;

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
                TarotFateManager.CurrentFate = chainSuccess;
                Poi.Current = new Poi(chainSuccess, PoiType.Fate);
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
                TarotFateManager.CurrentFate = chainFail;
                Poi.Current = new Poi(chainFail, PoiType.Fate);
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
                    TarotFateManager.CurrentFate = chainSuccess;
                    Poi.Current = new Poi(chainSuccess, PoiType.Fate);
                    return true;
                }

                Logger.SendLog("Selected FATE: '" + chainFail.Name + "'.");
                TarotFateManager.CurrentFate = chainFail;
                Poi.Current = new Poi(chainFail, PoiType.Fate);
                return true;
            }

            return false;
        }

        private static async Task<bool> WaitBeforeMoving()
        {
            var minTime = TarotSettings.Instance.FateDelayMovementMinimum * 1000;
            var maxTime = TarotSettings.Instance.FateDelayMovementMaximum * 1000;
            var randomWaitTime = new Random().Next(minTime, maxTime);

            Logger.SendLog("Waiting " + Math.Round(randomWaitTime / 1000f, 2) + " seconds before moving to FATE.");
            await Coroutine.Wait(randomWaitTime, () => !TarotFateManager.CurrentFate.IsValid);

            return true;
        }
    }
}