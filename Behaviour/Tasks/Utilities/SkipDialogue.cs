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

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

using Tarot.Helpers;

namespace Tarot.Behaviour.Tasks.Utilities
{
    internal static class SkipDialogue
    {
        public static async Task<bool> Main()
        {
            if (!Talk.DialogOpen && !Talk.ConvoLock)
            {
                return true;
            }

            Logger.SendLog("Skipping dialogue.");
            while (Talk.DialogOpen)
            {
                if (GameObjectManager.Attackers.Any())
                {
                    return false;
                }

                Talk.Next();
                await Coroutine.Yield();
            }

            await Coroutine.Sleep(200);
            return true;
        }
    }
}