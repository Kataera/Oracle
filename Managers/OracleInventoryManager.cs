using System.Linq;
using System.Threading.Tasks;

using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.NeoProfiles;

using Oracle.Helpers;

namespace Oracle.Managers
{
    internal static class OracleInventoryManager
    {
        public static async Task<EquipItemResult> EquipItem(uint itemId, EquipmentSlot slot)
        {
            var equipmentBagSlot = GetEquipmentSlotBagSlot(slot);
            if (equipmentBagSlot == null)
            {
                return EquipItemResult.Failure;
            }

            if (equipmentBagSlot.RawItemId == itemId)
            {
                return EquipItemResult.Success;
            }

            if (!ConditionParser.HasItem(itemId))
            {
                return EquipItemResult.ItemNotFound;
            }

            var itemBagSlot = GetBagSlotByItemId(itemId);
            if (itemBagSlot == null)
            {
                return EquipItemResult.BagSlotNotFound;
            }

            Logger.SendDebugLog(itemBagSlot.EnglishName + " is in bag: " + itemBagSlot.BagId + ", slot: " + itemBagSlot.Slot + ".");
            itemBagSlot.Move(equipmentBagSlot);
            return equipmentBagSlot.RawItemId == itemId ? EquipItemResult.Success : EquipItemResult.Failure;
        }

        public static BagSlot GetBagSlotByItemId(uint itemId)
        {
            return InventoryManager.FilledInventoryAndArmory.FirstOrDefault(bs => bs.RawItemId == itemId);
        }

        public static BagSlot GetEmptyBagSlot()
        {
            var bags = InventoryManager.GetBagsByInventoryBagId(InventoryBagId.Bag1, InventoryBagId.Bag2, InventoryBagId.Bag3, InventoryBagId.Bag4);
            return bags.Select(bag => bag.FirstOrDefault(slot => !slot.IsFilled)).FirstOrDefault();
        }

        public static BagSlot GetEquipmentSlotBagSlot(EquipmentSlot slot)
        {
            return InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == (ushort) slot);
        }

        public static uint GetItemAmount(uint itemId)
        {
            return GetBagSlotByItemId(itemId).Item.ItemCount();
        }

        public static bool IsItemEquipped(uint itemId, EquipmentSlot slot)
        {
            var equipmentBagSlot = InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == (ushort) slot);
            if (equipmentBagSlot == null)
            {
                Logger.SendErrorLog("Error retrieving equipment bag slot.");
                return true;
            }

            return equipmentBagSlot.RawItemId == itemId;
        }
    }

    internal enum EquipItemResult
    {
        ItemNotFound,

        BagSlotNotFound,

        Success,

        Failure
    }
}