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