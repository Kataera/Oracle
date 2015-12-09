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

using System.Linq;
using System.Threading.Tasks;
using ff14bot.Helpers;
using ff14bot.Managers;
using Tarot.Behaviour.PoiHooks.FateSelect;
using Tarot.Enumerations;
using Tarot.Helpers;
using Tarot.Settings;

namespace Tarot.Behaviour.PoiHooks
{
    internal static class SetFatePoi
    {
        public static async Task<bool> Main()
        {
            if (IsFateSet())
            {
                if (!IsFatePoiSet() && !GameObjectManager.Attackers.Any())
                {
                    Poi.Current = new Poi(Tarot.CurrentFate, PoiType.Fate);
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

            return IsFateSet() && IsFatePoiSet();
        }

        private static bool IsFatePoiSet()
        {
            if (Poi.Current == null || Poi.Current.Type != PoiType.Fate || Poi.Current.Fate != Tarot.CurrentFate)
            {
                return false;
            }

            return true;
        }

        private static bool IsFateSet()
        {
            return Tarot.CurrentFate != null && Tarot.CurrentFate.IsValid;
        }

        private static bool PreviousFateChained()
        {
            return PreviousFateChainedOnSuccess() || PreviousFateChainedOnFailure();
        }

        private static bool PreviousFateChainedOnFailure()
        {
            if (Tarot.PreviousFate != null)
            {
                var chainIdFailure = Tarot.FateDatabase.GetFateWithId(Tarot.PreviousFate.Id).ChainIdFailure;

                if (chainIdFailure != 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool PreviousFateChainedOnSuccess()
        {
            if (Tarot.PreviousFate != null)
            {
                var chainIdSuccess = Tarot.FateDatabase.GetFateWithId(Tarot.PreviousFate.Id).ChainIdSuccess;

                if (chainIdSuccess != 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static async Task<bool> SelectChainFate()
        {
            if (Tarot.PreviousFate != null)
            {
                var chainIdSuccess = Tarot.FateDatabase.GetFateWithId(Tarot.PreviousFate.Id).ChainIdSuccess;
                var chainIdFailure = Tarot.FateDatabase.GetFateWithId(Tarot.PreviousFate.Id).ChainIdFailure;

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
                    Tarot.CurrentFate = chainSuccess;
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
                    Tarot.CurrentFate = chainFail;
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
                        Tarot.CurrentFate = chainSuccess;
                        Poi.Current = new Poi(chainSuccess, PoiType.Fate);
                        return true;
                    }
                    Logger.SendLog("Selected FATE: '" + chainFail.Name + "'.");
                    Tarot.CurrentFate = chainFail;
                    Poi.Current = new Poi(chainFail, PoiType.Fate);
                    return true;
                }
            }

            return false;
        }
    }
}