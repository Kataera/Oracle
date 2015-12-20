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
using ff14bot.Navigation;

using Oracle.Helpers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class Mount
    {
        public static async Task<bool> MountUp()
        {
            if (!Actionmanager.AvailableMounts.Any())
            {
                Logger.SendDebugLog("Character does not have any mount available, skipping mount task.");
                return true;
            }

            if (MovementManager.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }

            while (!Core.Player.IsMounted)
            {
                if (GameObjectManager.Attackers.Any())
                {
                    return false;
                }

                if (Actionmanager.CanMount == 0)
                {
                    Actionmanager.Mount();
                }

                await Coroutine.Yield();
            }

            return true;
        }
    }
}