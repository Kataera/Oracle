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

namespace Tarot.Behaviour.Tasks.Selectors
{
    using System.Threading.Tasks;

    using ff14bot.Helpers;

    using global::Tarot.Behaviour.Tasks.Selectors.Fates;
    using global::Tarot.Behaviour.Tasks.Utilities;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class FateSelector
    {
        public static async Task<bool> Task()
        {
            await BuildFateDatabase.Task();

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
                    await Closest.Task();
                    break;

                case (int) FateSelectMode.TypePriority:
                    // TODO: Implement.
                    await Closest.Task();
                    break;

                case (int) FateSelectMode.ChainPriority:
                    // TODO: Implement.
                    await Closest.Task();
                    break;

                case (int) FateSelectMode.TypeAndChainPriority:
                    // TODO: Implement.
                    await Closest.Task();
                    break;

                default:
                    Logger.SendDebugLog("Cannot determine FATE selection strategy, defaulting to closest FATE.");
                    await Closest.Task();
                    break;
            }

            return IsFateSet() && IsFatePoiSet();
        }

        private static bool IsFateSet()
        {
            return Tarot.CurrentFate != null && Tarot.CurrentFate.IsValid;
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