/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

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

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks
{
    internal static class CombatHandler
    {
        public static async Task<bool> HandleCombat()
        {
            var currentBc = Poi.Current.BattleCharacter;

            if (currentBc == null)
            {
                return false;
            }

            if (!currentBc.IsValid)
            {
                OracleFateManager.ClearPoi("Targeted unit is not valid.", false);
                return true;
            }

            if (!currentBc.IsFate && !currentBc.IsDead && GameObjectManager.Attackers.All(mob => mob.ObjectId != currentBc.ObjectId)
                && OracleFateManager.CurrentFateId != 0)
            {
                OracleFateManager.ClearPoi("Targeted unit is not in combat with us, nor part of the current FATE.", false);
                return true;
            }

            // If target is not a FATE mob and is tapped by someone else.
            if (!currentBc.IsFate && currentBc.TappedByOther)
            {
                OracleFateManager.ClearPoi("Targeted unit is not a FATE mob and is tapped by someone else.");
                Blacklist.Add(currentBc, BlacklistFlags.Combat, TimeSpan.FromSeconds(30), "Tapped by another person.");
                Core.Player.ClearTarget();

                if (OracleSettings.Instance.FateWaitMode == FateWaitMode.GrindMobs)
                {
                    var target = await SelectGrindTarget.Main();
                    if (target == null)
                    {
                        return true;
                    }

                    Logger.SendLog("Selecting '" + target.Name + "' as the next target to kill.");
                    Poi.Current = new Poi(target, PoiType.Kill);
                }

                return true;
            }

            // If target is a FATE mob, we need to handle several potential issues.
            if (currentBc.IsFate)
            {
                var fate = FateManager.GetFateById(Poi.Current.BattleCharacter.FateId);

                if (fate == null)
                {
                    return true;
                }

                if (!LevelSync.IsLevelSyncNeeded(fate))
                {
                    return true;
                }

                if (fate.Status != FateStatus.NOTACTIVE)
                {
                    if (fate.Within2D(Core.Player.Location))
                    {
                        await LevelSync.Main(fate);
                    }
                    else if (GameObjectManager.Attackers.Contains(Poi.Current.BattleCharacter))
                    {
                        await MoveToFate.MoveToCurrentFate(true);
                        await LevelSync.Main(fate);
                    }
                }
            }

            return true;
        }
    }
}