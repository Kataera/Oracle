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

using Tarot.Behaviour.Tasks.WaitTask;
using Tarot.Enumerations;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks
{
    internal static class WaitRunner
    {
        public static async Task<bool> Main()
        {
            switch (TarotSettings.Instance.FateWaitMode)
            {
                case FateWaitMode.ReturnToAetheryte:
                    await ReturnToAetheryte.Main();
                    return true;
                case FateWaitMode.MoveToWaitLocation:
                    await MoveToWaitLocation.Main();
                    return true;
                case FateWaitMode.GrindMobs:
                    await GrindMobs.Main();
                    return true;
                case FateWaitMode.WaitForFates:
                    await WaitForFates.Main();
                    return true;
            }

            return false;
        }
    }
}