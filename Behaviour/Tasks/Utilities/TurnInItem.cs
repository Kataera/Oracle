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

using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Managers;
using ff14bot.RemoteWindows;

using Tarot.Helpers;

namespace Tarot.Behaviour.Tasks.Utilities
{
    internal static class TurnInItem
    {
        public static async Task<bool> Main()
        {
            if (!Request.IsOpen)
            {
                return false;
            }

            if (GameObjectManager.Attackers.Any())
            {
                return false;
            }

            var itemId = Tarot.FateDatabase.GetFateWithId(Tarot.CurrentFate.Id).ItemId;
            var turnInBagSlot = GetBagSlotFromItemId(itemId);

            if (turnInBagSlot == null)
            {
                return false;
            }

            Logger.SendLog("Attempting to hand over " + turnInBagSlot.Count + " of the item '" + turnInBagSlot.Name + "'.");
            turnInBagSlot.Handover();

            await Coroutine.Sleep(TimeSpan.FromMilliseconds(500));
            if (!Request.HandOverButtonClickable)
            {
                Logger.SendErrorLog("Hand over failed.");
                Request.Cancel();

                return false;
            }

            Logger.SendLog("Pressing 'Hand Over' button.");
            Request.HandOver();
            await Coroutine.Sleep(500);

            return true;
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
    }
}