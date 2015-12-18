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

using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class SkipDialogue
    {
        public static async Task<bool> Main()
        {
            if (!Talk.DialogOpen && !Talk.ConvoLock)
            {
                return true;
            }

            Logger.SendDebugLog("Skipping dialogue.");
            while (Talk.DialogOpen)
            {
                if (GameObjectManager.Attackers.Any())
                {
                    return false;
                }

                if (!Core.Player.HasTarget)
                {
                    return false;
                }

                Talk.Next();
                await Coroutine.Yield();
            }

            await Coroutine.Sleep(OracleSettings.Instance.ActionDelay);
            return true;
        }
    }
}