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

using System.Threading.Tasks;

using ff14bot.Helpers;

using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Helpers;

namespace Tarot.Behaviour.Tasks.WaitTask
{
    internal static class GrindMobs
    {
        public static async Task<bool> Main()
        {
            var target = await SelectGrindTarget.Main();
            if (target == null)
            {
                return true;
            }

            Logger.SendLog("Selecting '" + target.Name + "' as the next target to kill.");
            Poi.Current = new Poi(target, PoiType.Kill);
            return true;
        }
    }
}