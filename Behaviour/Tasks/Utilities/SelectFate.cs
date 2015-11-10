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
            // Check the FATE database has been populated.
            if (Tarot.FateDatabase == null)
            {
                await BuildFateDatabase.Task();
            }

            // Check if we have a FATE already set.
            if (IsFateSet())
            {
                // Check that the Poi has been set correctly.
                if (!IsFatePoiSet())
                {
                    Poi.Current = new Poi(Tarot.CurrentFate, PoiType.Fate);
                }

                return true;
            }

            // Determine FATE selection strategy.
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

            // Check if the FATE selection succeeded.
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

            // Check if we got something back from the FATE manager.
            if (activeFates == null)
            {
                return;
            }

            // Check if there's no active FATEs.
            if (activeFates.Length == 0)
            {
                return;
            }

            Logger.SendLog("Selecting closest active FATE.");
            foreach (var fate in activeFates)
            {
                // If this is the first FATE we're processing, just set as closest.
                if (closestFate == null)
                {
                    closestFate = fate;
                }

                // If distance to current FATE is less than current closest, set it as closest.
                if (playerLocation.Distance2D(closestFate.Location) > playerLocation.Distance2D(fate.Location))
                {
                    closestFate = fate;
                }
            }

            // Set FATE in Tarot and the Poi.
            if (closestFate != null)
            {
                Logger.SendLog("Selected FATE: '" + closestFate.Name + "'.");
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
            // Check for null Poi.
            if (Poi.Current == null)
            {
                return false;
            }

            // Check if the current Poi is not a FATE.
            if (Poi.Current.Type != PoiType.Fate)
            {
                return false;
            }

            // Check for current FATE Poi and Tarot FATE mismatch.
            if (Poi.Current.Fate != Tarot.CurrentFate)
            {
                return false;
            }

            return true;
        }
    }
}