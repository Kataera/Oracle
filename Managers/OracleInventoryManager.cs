using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.RemoteWindows;

using Oracle.Enumerations;
using Oracle.Enumerations.TaskResults;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleInventoryManager
    {
        private const uint GysahlGreenItemId = 4868;
        private const uint MaisentaNpcId = 1001276;
        private const uint BangoZangoNpcId = 1001787;
        private const uint RoarichNpcId = 1004417;
        private const uint JunkmongerNpcId = 1017189;
        private const ushort GridaniaZoneId = 132;
        private const ushort LimsaLominsaZoneId = 129;
        private const ushort UldahZoneId = 130;
        private const ushort IdyllshireZoneId = 478;
        private static ushort originZoneId;

        private static readonly Vector3 MaisentaLocation = new Vector3(12.00642f, 0.1280167f, 2.007058f);
        private static readonly Vector3 BangoZangoLocation = new Vector3(-63.55167f, 18.00033f, 8.810302f);
        private static readonly Vector3 RoarichLocation = new Vector3(-31.04098f, 9f, -85.05112f);
        private static readonly Vector3 JunkmongerLocation = new Vector3(-2.882056f, 206.4994f, 54.65067f);

        internal static async Task<BuyItemResult> BuyItem(uint itemId, uint itemAmount)
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

            await Coroutine.Sleep(TimeSpan.FromMilliseconds(500));
            Shop.Purchase(itemId, itemAmount);
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(500));
            SelectYesno.ClickYes();
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(500));
            Shop.Close();

            return GetItemAmount(itemId) >= itemAmount ? BuyItemResult.Success : BuyItemResult.Failure;
        }

        internal static async Task<EquipItemResult> EquipItem(uint itemId, EquipmentSlot slot)
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

        internal static BagSlot GetBagSlotByItemId(uint itemId)
        {
            return InventoryManager.FilledInventoryAndArmory.FirstOrDefault(bs => bs.RawItemId == itemId);
        }

        internal static BagSlot GetBagSlotFromItemId(uint itemId)
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

        internal static BagSlot GetEquipmentSlotBagSlot(EquipmentSlot slot)
        {
            return InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == (ushort) slot);
        }

        internal static uint GetItemAmount(uint itemId)
        {
            return GetBagSlotByItemId(itemId) == null ? 0 : GetBagSlotByItemId(itemId).Item.ItemCount();
        }

        internal static async Task<bool> OpenShopWindow(uint npcId)
        {
            if (WorldManager.ZoneId == IdyllshireZoneId)
            {
                return true;
            }

            var npc = GameObjectManager.GetObjectByNPCId(npcId);
            if (npc == null)
            {
                return false;
            }

            npc.Interact();
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(500));
            SelectIconString.ClickSlot(0);
            await Coroutine.Wait(TimeSpan.FromMilliseconds(500), () => Shop.Open);

            return Shop.Open;
        }

        internal static async Task<bool> RestockGyshalGreens()
        {
            if (!ShouldRestockGreens())
            {
                return true;
            }

            Logger.SendLog("Going to restock our Gysahl Greens.");
            originZoneId = WorldManager.ZoneId;
            await OracleNavigationManager.TeleportToClosestCity();

            uint vendorNpcId;
            Vector3 vendorLocation;
            switch (WorldManager.ZoneId)
            {
                case GridaniaZoneId:
                    vendorNpcId = MaisentaNpcId;
                    vendorLocation = MaisentaLocation;
                    break;
                case LimsaLominsaZoneId:
                    vendorNpcId = BangoZangoNpcId;
                    vendorLocation = BangoZangoLocation;
                    break;
                case UldahZoneId:
                    vendorNpcId = RoarichNpcId;
                    vendorLocation = RoarichLocation;
                    break;
                case IdyllshireZoneId:
                    vendorNpcId = JunkmongerNpcId;
                    vendorLocation = JunkmongerLocation;
                    break;
                default:
                    Logger.SendErrorLog("Not in a city that sells Gysahl Greens.");
                    return false;
            }

            Logger.SendDebugLog("Going to NPC " + vendorNpcId + " at " + vendorLocation + ".");
            if (Core.Player.Distance(vendorLocation) > 4f)
            {
                await OracleNavigationManager.NavigateToLocation(vendorLocation, 2f);
            }

            var openCorrectShopResult = await OpenShopWindow(vendorNpcId);
            if (!openCorrectShopResult)
            {
                return false;
            }

            var amountToBuy = Convert.ToUInt32(OracleSettings.Instance.RestockGreensAmount) - GetItemAmount(GysahlGreenItemId);
            var buyItemResult = await BuyItem(GysahlGreenItemId, amountToBuy);

            if (buyItemResult != BuyItemResult.Success)
            {
                return false;
            }

            if (OracleSettings.Instance.OracleFateMode != OracleFateMode.LevelMode
                && OracleSettings.Instance.OracleFateMode != OracleFateMode.MultiLevelMode)
            {
                await OracleNavigationManager.TeleportToAetheryte(WorldManager.GetZoneForAetheryteId(originZoneId));
            }

            return GetItemAmount(GysahlGreenItemId) >= OracleSettings.Instance.RestockGreensAmount;
        }

        internal static bool ShouldRestockGreens()
        {
            if (Core.Player.InCombat)
            {
                return false;
            }

            if (!OracleSettings.Instance.RestockGreens)
            {
                return false;
            }

            return GetItemAmount(GysahlGreenItemId) < OracleSettings.Instance.RestockGreensMin;
        }
    }
}