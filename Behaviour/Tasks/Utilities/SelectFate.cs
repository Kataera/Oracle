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

namespace Tarot.Behaviour.Tasks.Utilities
{
    using System.Threading.Tasks;

    using ff14bot;
    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class SelectFate
    {
        public static async Task<bool> Task()
        {
            if (Tarot.FateDatabase == null)
            {
                await BuildFateDatabase.Task();
            }

            if (IsFateSet())
            {
                if (!IsFatePoiSet())
                {
                    Poi.Current = new Poi(Tarot.CurrentFate, PoiType.Fate);
                }

                return true;
            }

            switch (TarotSettings.Instance.FateSelectMode)
            {
                case (int) FateSelectMode.Closest:
                    SelectClosestFate();
                    break;

                case (int) FateSelectMode.TypePriority:
                    // TODO: Implement.
                    SelectClosestFate();
                    break;

                case (int) FateSelectMode.ChainPriority:
                    // TODO: Implement.
                    SelectClosestFate();
                    break;

                case (int) FateSelectMode.TypeAndChainPriority:
                    // TODO: Implement.
                    SelectClosestFate();
                    break;

                default:
                    Logger.SendDebugLog("Cannot determine FATE selection strategy, defaulting to closest FATE.");
                    SelectClosestFate();
                    break;
            }

            if (!IsFateSet() || !IsFatePoiSet())
            {
                return false;
            }

            return true;
        }

        private static void SelectClosestFate()
        {
            var activeFates = FateManager.ActiveFates as FateData[];
            var playerLocation = Core.Player.Location;
            FateData closestFate = null;

            if (activeFates == null || activeFates.Length == 0)
            {
                return;
            }

            Logger.SendLog("Selecting closest active FATE.");
            foreach (var fate in activeFates)
            {
                if (closestFate == null
                    || playerLocation.Distance2D(closestFate.Location) > playerLocation.Distance2D(fate.Location))
                {
                    closestFate = fate;
                }
            }

            if (closestFate != null)
            {
                Logger.SendLog("Selected FATE: '" + closestFate.Name + "'.");

                // Set FATE in Tarot and the Poi.
                Tarot.CurrentFate = closestFate;
                Poi.Current = new Poi(closestFate, PoiType.Fate);
            }
        }

        private static bool IsFateSet()
        {
            return Tarot.CurrentFate != null;
        }

        private static bool IsFatePoiSet()
        {
            if (Poi.Current == null || Poi.Current.Type != PoiType.Fate || Poi.Current.Fate != Tarot.CurrentFate)
            {
                return false;
            }

            return true;
        }
    }
}