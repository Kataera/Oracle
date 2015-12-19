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

using System.Threading.Tasks;

using ff14bot;

using Oracle.Behaviour.Tasks.WaitTask;
using Oracle.Enumerations;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks
{
    internal static class WaitHandler
    {
        public static async Task<bool> HandleWait()
        {
            if (OracleManager.IsPlayerBeingAttacked() && !Core.Player.IsMounted)
            {
                OracleManager.ClearPoi("We're being attacked.", false);
                return true;
            }

            if (await OracleManager.AnyViableFates())
            {
                OracleManager.ClearPoi("Viable FATE detected.");
                return true;
            }

            return await RunWait();
        }

        private static async Task<bool> RunWait()
        {
            switch (OracleSettings.Instance.FateWaitMode)
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
                case FateWaitMode.WaitInPlace:
                    await WaitInPlace.Main();
                    return true;
            }

            return false;
        }
    }
}