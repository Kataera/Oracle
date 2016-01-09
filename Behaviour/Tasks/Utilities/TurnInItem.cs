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

using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Managers;
using ff14bot.RemoteWindows;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
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

            var currentFate = OracleFateManager.GetCurrentFateData();
            var itemId = OracleFateManager.OracleDatabase.GetFateFromId(currentFate.Id).ItemId;
            var turnInBagSlot = GetBagSlotFromItemId(itemId);

            if (turnInBagSlot == null)
            {
                return false;
            }

            Logger.SendLog("Attempting to hand over " + turnInBagSlot.Count + " of the item '" + turnInBagSlot.Name + "'.");
            turnInBagSlot.Handover();

            await Coroutine.Sleep(OracleSettings.Instance.ActionDelay);
            if (!Request.IsOpen || !Request.HandOverButtonClickable)
            {
                Logger.SendErrorLog("Hand over failed.");
                Request.Cancel();

                return false;
            }

            Logger.SendDebugLog("Pressing 'Hand Over' button.");
            Request.HandOver();
            await Coroutine.Sleep(OracleSettings.Instance.ActionDelay);

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