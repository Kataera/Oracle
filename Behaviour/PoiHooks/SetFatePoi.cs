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

namespace Tarot.Behaviour.PoiHooks
{
    using System.Linq;
    using System.Threading.Tasks;

    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Behaviour.PoiHooks.FateSelect;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class SetFatePoi
    {
        private static async Task<bool> ChainFate()
        {
            if (Tarot.PreviousFate != null)
            {
                var chainSuccessId = Tarot.FateDatabase.GetFateWithId(Tarot.PreviousFate.Id).ChainIdSuccess;
                var chainFailId = Tarot.FateDatabase.GetFateWithId(Tarot.PreviousFate.Id).ChainIdFailure;

                // If there's a success chain only.
                if (chainSuccessId != 0 && chainFailId == 0)
                {
                    Logger.SendLog("Waiting for the follow up FATE.");
                    var chainSuccess = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainSuccessId);

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
                if (chainSuccessId == 0 && chainFailId != 0)
                {
                    Logger.SendLog("Waiting for the follow up FATE.");
                    var chainFail = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainFailId);

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
                if (chainSuccessId != 0 && chainFailId != 0)
                {
                    Logger.SendLog("Waiting for the follow up FATE.");
                    var chainSuccess = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainSuccessId);
                    var chainFail = FateManager.ActiveFates.FirstOrDefault(result => result.Id == chainFailId);

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

            // Check if previous FATE had a chain.
            if (await ChainFate())
            {
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
    }
}