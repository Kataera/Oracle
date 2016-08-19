using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.RemoteWindows;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleInventoryManager
    {
        public static async Task<BuyItemResult> BuyItem(uint itemId, uint itemAmount)
        {
            if (!Shop.Open)
            {
                return BuyItemResult.ShopNotOpen;
            }

            if (!Shop.Items.Any(item => item.ItemId == itemId))
            {
                return BuyItemResult.ItemNotFound;
            }

            if (itemAmount > 99)
            {
                return BuyItemResult.TooManyItemsRequested;
            }

            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));
            Shop.Purchase(itemId, itemAmount);
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));
            SelectYesno.ClickYes();
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));
            Shop.Close();

            return GetItemAmount(itemId) >= itemAmount ? BuyItemResult.Success : BuyItemResult.Failure;
        }

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
            return GetBagSlotByItemId(itemId) == null ? 0 : GetBagSlotByItemId(itemId).Item.ItemCount();
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

        public static bool ShouldRestockGreens()
        {
            const uint gysahlGreensItemId = 4868;
            if (!MainSettings.Instance.ChocoboGreensRestockEnabled)
            {
                return false;
            }

            return GetItemAmount(gysahlGreensItemId) < MainSettings.Instance.ChocoboMinRemainingGreensToRestock;
        }
    }

    internal enum EquipItemResult
    {
        ItemNotFound,

        BagSlotNotFound,

        Success,

        Failure
    }

    internal enum BuyItemResult
    {
        ShopNotOpen,

        ItemNotFound,

        TooManyItemsRequested,

        Success,

        Failure
    }
}