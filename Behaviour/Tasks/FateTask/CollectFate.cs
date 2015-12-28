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
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.FateTask
{
    internal static class CollectFate
    {
        public static async Task<bool> Main()
        {
            var currentFate = OracleManager.GetCurrentFateData();
            var oracleFate = OracleManager.OracleDatabase.GetFateFromId(OracleManager.CurrentFateId);
            var fateItemCount = ConditionParser.ItemCount(oracleFate.ItemId);

            if (currentFate == null)
            {
                await ClearFate();
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE)
            {
                if (GameObjectManager.GetObjectByNPCId(oracleFate.NpcId) != null)
                {
                    if (fateItemCount >= OracleSettings.Instance.CollectFateTurnInAtAmount)
                    {
                        Logger.SendLog("Turning in what we've collected.");
                        await TurnInFateItems(GameObjectManager.GetObjectByNPCId(oracleFate.NpcId));
                    }
                }
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && currentFate.Status == FateStatus.COMPLETE)
            {
                if (Core.Player.InCombat)
                {
                    return false;
                }

                if (fateItemCount >= 1)
                {
                    Logger.SendLog("FATE is complete, turning in remaining items.");
                    await TurnInFateItems(GameObjectManager.GetObjectByNPCId(oracleFate.NpcId));
                }

                await ClearFate();
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && AnyViableTargets())
            {
                SelectTarget();
            }

            return true;
        }

        private static bool AnyViableTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableTarget).Any();
        }

        private static async Task ClearFate()
        {
            await OracleManager.ClearCurrentFate("Current FATE is finished.");
        }

        private static bool IsViableTarget(BattleCharacter target)
        {
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == OracleManager.CurrentFateId;
        }

        private static async Task<bool> MoveToTurnInNpc(GameObject turnInNpc)
        {
            Logger.SendLog("Moving to interact with " + turnInNpc.Name + ".");

            while (Core.Player.Distance2D(turnInNpc.Location) > 5f)
            {
                Navigator.MoveToPointWithin(turnInNpc.Location, 5f, "Moving to NPC.");
                await Coroutine.Yield();
            }

            Navigator.Stop();
            return true;
        }

        private static void SelectTarget()
        {
            var oracleFate = OracleManager.OracleDatabase.GetFateFromId(OracleManager.CurrentFateId);
            BattleCharacter target = null;

            if (oracleFate.PreferredTargetId.Any())
            {
                var targets = GameObjectManager.GetObjectsByNPCIds<BattleCharacter>(oracleFate.PreferredTargetId.ToArray());
                target = targets.OrderBy(bc => bc.Distance(Core.Player)).FirstOrDefault(bc => bc.IsValid && bc.IsAlive);

                if (target == null)
                {
                    Logger.SendDebugLog("Could not find any mobs with the preferred targets' NPC id.");
                }
                else
                {
                    Logger.SendDebugLog("Found preferred target '" + target.Name + "' (" + target.NpcId + ").");
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

        private static async Task<bool> TurnInFateItems(GameObject turnInNpc)
        {
            if (Core.Player.Distance2D(turnInNpc.Location) > 5f)
            {
                await MoveToTurnInNpc(turnInNpc);
            }

            if (GameObjectManager.Attackers.Any())
            {
                return false;
            }

            turnInNpc.Face();
            turnInNpc.Interact();
            await Coroutine.Sleep(500);
            var result = await SkipDialogue.Main() && await TurnInItem.Main() && await SkipDialogue.Main();

            if (result)
            {
                Logger.SendLog("Items have been handed over to " + turnInNpc.Name + ".");
            }

            return result;
        }
    }
}