﻿/*
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

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

using Tarot.Helpers;
using Tarot.Managers;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.FateTask
{
    internal static class BossFate
    {
        public static async Task<bool> Main()
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            var tarotFate = TarotFateManager.FateDatabase.GetFateFromFateData(currentFate);

            if (currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
            {
                ClearFate();
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && currentFate.Progress < TarotSettings.Instance.BossEngagePercentage)
            {
                if (!TarotSettings.Instance.WaitAtFateForProgress)
                {
                    TarotFateManager.ClearCurrentFate("Current FATE progress reset below minimum level.", false);
                }
                else
                {
                    Logger.SendLog(
                        "Current FATE progress is too low, waiting for it to reach "
                        + TarotSettings.Instance.BossEngagePercentage + "%.");
                }

                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && AnyViableTargets())
            {
                BattleCharacter target = null;
                if (tarotFate.PreferredTargetId.Any())
                {
                    Logger.SendDebugLog("FATE has preferred targets listed, searching for them.");
                    var targets = GameObjectManager.GetObjectsByNPCIds<BattleCharacter>(tarotFate.PreferredTargetId.ToArray());
                    target = targets.OrderBy(bc => bc.Distance(Core.Player)).FirstOrDefault();

                    if (target == null)
                    {
                        Logger.SendDebugLog("Could not find any mobs with the preferred target's ID.");
                    }
                    else
                    {
                        Logger.SendDebugLog("Found target '" + target.Name + "' which matches the preferred target ID (" + target.NpcId + ").");
                    }
                }

                if (target == null)
                {
                    target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
                }

                if (target != null)
                {
                    Poi.Current = new Poi(target, PoiType.Kill);
                }
            }

            return true;
        }

        private static bool AnyViableTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableTarget).Any();
        }

        private static void ClearFate()
        {
            TarotFateManager.ClearCurrentFate("Current FATE is finished.");
        }

        private static bool IsViableTarget(BattleCharacter target)
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == currentFate.Id;
        }
    }
}