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

using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;

using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Helpers;
using Tarot.Managers;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.FateTask
{
    internal static class CollectFate
    {
        public static async Task<bool> Main()
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            var tarotFate = TarotFateManager.FateDatabase.GetFateFromId(currentFate.Id);
            var fateItemBagSlot = GetBagSlotFromItemId(tarotFate.ItemId);

            if (currentFate.Status != FateStatus.NOTACTIVE && fateItemBagSlot != null)
            {
                // Wait for potential inventory update.
                var fateItemCount = fateItemBagSlot.Count;
                await Coroutine.Wait(TimeSpan.FromSeconds(2), () => fateItemCount < fateItemBagSlot.Count);

                if (GameObjectManager.GetObjectByNPCId(tarotFate.NpcId) != null)
                {
                    if (fateItemBagSlot.Count >= TarotSettings.Instance.CollectFateTurnInAtAmount)
                    {
                        Logger.SendLog("Turning in what we've collected.");
                        await TurnInFateItems(GameObjectManager.GetObjectByNPCId(tarotFate.NpcId));
                    }
                }
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && currentFate.Status == FateStatus.COMPLETE)
            {
                if (Core.Player.InCombat)
                {
                    return false;
                }

                if (fateItemBagSlot != null && fateItemBagSlot.Count >= 1)
                {
                    Logger.SendLog("FATE is complete, turning in remaining items.");
                    await TurnInFateItems(GameObjectManager.GetObjectByNPCId(tarotFate.NpcId));
                }

                ClearFate();
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && AnyViableTargets())
            {
                var target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
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

        private static BagSlot GetBagSlotFromItemId(uint itemId)
        {
            BagSlot bagSlot = null;
            foreach (var bagslot in InventoryManager.FilledSlots)
            {
                if (bagslot.TrueItemId == itemId)
                {
                    bagSlot = bagslot;
                }
            }

            return bagSlot;
        }

        private static bool IsViableTarget(BattleCharacter target)
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == currentFate.Id;
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

            turnInNpc.Interact();
            await Coroutine.Sleep(500);
            await SkipDialogue.Main();
            await TurnInItem.Main();
            await SkipDialogue.Main();

            Logger.SendLog("Items have been handed over to " + turnInNpc.Name + ".");
            return true;
        }
    }
}