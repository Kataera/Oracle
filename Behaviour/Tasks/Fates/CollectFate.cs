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
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;

using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Helpers;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.Fates
{
    internal static class CollectFate
    {
        public static async Task<bool> Main()
        {
            var fate = Tarot.FateDatabase.GetFateWithId(Tarot.CurrentFate.Id);
            var fateItemId = fate.ItemId;
            var fateItemBagSlot = GetBagSlotFromItemId(fateItemId);
            var turnInNpc = GameObjectManager.GetObjectByNPCId(fate.NpcId);

            if (fateItemBagSlot != null && turnInNpc != null)
            {
                if (fateItemBagSlot.Count >= TarotSettings.Instance.CollectFateTurnInAtAmount || Tarot.CurrentFate.Status == FateStatus.COMPLETE)
                {
                    await TurnInFateItems(turnInNpc);
                }
            }

            if (AnyViableTargets())
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
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == Tarot.CurrentFate.Id;
        }

        private static async Task<bool> TurnInFateItems(GameObject turnInNpc)
        {
            Logger.SendLog("Going to turn in our items.");

            if (Core.Player.Distance2D(turnInNpc.Location) > 5f)
            {
                await MoveToTurnInNpc(turnInNpc);
            }

            if (Core.Player.InCombat)
            {
                return false;
            }

            turnInNpc.Interact();
            await Coroutine.Sleep(500);
            await SkipDialogue.Main();
            await TurnInItem.Main();
            await SkipDialogue.Main();

            return true;
        }

        private static async Task<bool> MoveToTurnInNpc(GameObject turnInNpc)
        {
            Logger.SendLog("Moving to interact with " + turnInNpc.Name + ".");

            while (Core.Player.Distance2D(turnInNpc.Location) > 5f)
            {
                if (Core.Player.InCombat)
                {
                    break;
                }

                Navigator.MoveToPointWithin(turnInNpc.Location, 5f, "Moving to NPC.");
                await Coroutine.Yield();
            }

            Navigator.Stop();
            return true;
        }
    }
}