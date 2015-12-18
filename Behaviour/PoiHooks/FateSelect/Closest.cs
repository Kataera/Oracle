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
using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.PoiHooks.FateSelect
{
    internal static class Closest
    {
        public static async Task<bool> Main()
        {
            if (!await OracleFateManager.AnyViableFates())
            {
                return false;
            }

            var activeFates = await OracleFateManager.GetActiveFateDistances();
            var closestFates = activeFates.OrderBy(kvp => kvp.Value).Where(fate => OracleFateManager.FateFilter(fate.Key));
            foreach (var fate in closestFates)
            {
                var distance = Math.Round(fate.Value - (fate.Key.Radius * 0.75f), 0);

                if (distance > 0)
                {
                    Logger.SendDebugLog("Found FATE '" + fate.Key.Name + "'. Distance to it is ~" + distance + " yalms.");
                }
                else
                {
                    Logger.SendDebugLog("Found FATE '" + fate.Key.Name + "'. Distance to it is 0 yalms.");
                }
            }

            if (!closestFates.Any())
            {
                return false;
            }

            Logger.SendLog("Selecting closest FATE.");
            var closestFate = closestFates.FirstOrDefault().Key;

            Logger.SendLog("Selected FATE: '" + closestFate.Name + "'.");
            OracleFateManager.CurrentFateId = closestFate.Id;
            Poi.Current = new Poi(closestFate, PoiType.Fate);

            return true;
        }
    }
}