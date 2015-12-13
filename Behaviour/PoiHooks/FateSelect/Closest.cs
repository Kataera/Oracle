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

using Tarot.Helpers;
using Tarot.Managers;

namespace Tarot.Behaviour.PoiHooks.FateSelect
{
    internal static class Closest
    {
        public static async Task<bool> Main()
        {
            if (!await TarotFateManager.AnyViableFates())
            {
                return false;
            }

            var activeFates = await TarotFateManager.GetActiveFateDistances();
            var closestFates = activeFates.OrderBy(kvp => kvp.Value).Where(fate => TarotFateManager.FateFilter(fate.Key));

            if (!closestFates.Any())
            {
                return false;
            }

            Logger.SendLog("Selecting closest FATE.");
            var closestFate = closestFates.FirstOrDefault().Key;

            Logger.SendLog("Selected FATE: '" + closestFate.Name + "'.");
            TarotFateManager.CurrentFate = closestFate;
            Poi.Current = new Poi(closestFate, PoiType.Fate);

            return true;
        }
    }
}