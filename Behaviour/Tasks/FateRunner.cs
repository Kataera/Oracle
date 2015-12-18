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
using System.Threading.Tasks;

using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks.FateTask;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks
{
    internal static class FateRunner
    {
        public static async Task<bool> Main()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null)
            {
                return false;
            }

            switch (OracleFateManager.OracleDatabase.GetFateFromFateData(currentFate).Type)
            {
                case FateType.Kill:
                    await KillFate.Main();
                    return true;
                case FateType.Collect:
                    await CollectFate.Main();
                    return true;
                case FateType.Escort:
                    await EscortFate.Main();
                    return true;
                case FateType.Defence:
                    await DefenceFate.Main();
                    return true;
                case FateType.Boss:
                    await BossFate.Main();
                    return true;
                case FateType.MegaBoss:
                    await MegaBossFate.Main();
                    return true;
                case FateType.Null:
                    Logger.SendDebugLog("Cannot find FATE in database, using Rebornbuddy's FATE type identifier.");
                    break;
            }

            switch (currentFate.Icon)
            {
                case FateIconType.Battle:
                    await KillFate.Main();
                    return true;
                case FateIconType.Boss:
                    Logger.SendDebugLog("Cannot determine if FATE is a regular or mega-boss, assuming regular.");
                    await BossFate.Main();
                    return true;
                case FateIconType.KillHandIn:
                    await CollectFate.Main();
                    return true;
                case FateIconType.ProtectNPC:
                    await EscortFate.Main();
                    return true;
                case FateIconType.ProtectNPC2:
                    await DefenceFate.Main();
                    return true;
            }

            Logger.SendDebugLog("Cannot determine FATE type, blacklisting.");
            Blacklist.Add(currentFate.Id, BlacklistFlags.Node, TimeSpan.MaxValue, "Cannot determine FATE type.");
            OracleFateManager.ClearCurrentFate("Cannot determine FATE type.");

            return false;
        }
    }
}