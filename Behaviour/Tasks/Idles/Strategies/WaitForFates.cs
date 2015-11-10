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

namespace Tarot.Behaviour.Tasks.Idles.Strategies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Buddy.Coroutines;

    using ff14bot;
    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Helpers;

    internal static class WaitForFates
    {
        public static async Task<bool> Task()
        {
            if (!IsFateActive())
            {
                Logger.SendLog("Waiting for a FATE to activate.");
                Poi.Current = new Poi(Core.Player.Location, PoiType.Wait);
                await Coroutine.Wait(TimeSpan.MaxValue, IsFateActive);
            }

            Logger.SendLog("Found a FATE, exiting idle coroutine.");
            return true;
        }

        private static bool IsFateActive()
        {
            var activeFates = FateManager.ActiveFates;
            if (activeFates != null && !activeFates.Any())
            {
                return true;
            }

            return false;
        }
    }
}