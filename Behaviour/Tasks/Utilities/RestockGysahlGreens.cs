using System;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class RestockGysahlGreens
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

        public static async Task<RestockGysahlGreensResult> HandleRestockGyshalGreens()
        {
            if (!OracleInventoryManager.ShouldRestockGreens())
            {
                return RestockGysahlGreensResult.Success;
            }

            Logger.SendLog("Going to restock our Gysahl Greens.");
            originZoneId = WorldManager.ZoneId;
            await OracleTeleportManager.TeleportToClosestCity();

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
                    return RestockGysahlGreensResult.Failure;
            }

            Logger.SendDebugLog("Going to NPC " + vendorNpcId + " at " + vendorLocation + ".");
            if (Core.Player.Distance(vendorLocation) > 4f)
            {
                await OracleMovementManager.NavigateToLocation(vendorLocation, 2f, false);
            }

            var openCorrectShopResult = await OpenCorrectShopWindow(vendorNpcId);
            if (!openCorrectShopResult)
            {
                return RestockGysahlGreensResult.Failure;
            }

            var amountToBuy = Convert.ToUInt32(MainSettings.Instance.ChocoboGreensRestockAmount) - OracleInventoryManager.GetItemAmount(GysahlGreenItemId);
            var buyItemResult = await OracleInventoryManager.BuyItem(GysahlGreenItemId, amountToBuy);

            if (buyItemResult != BuyItemResult.Success)
            {
                return RestockGysahlGreensResult.Failure;
            }

            if (ModeSettings.Instance.OracleOperationMode != OracleOperationMode.LevelMode
                && ModeSettings.Instance.OracleOperationMode != OracleOperationMode.MultiLevelMode)
            {
                await OracleTeleportManager.TeleportToAetheryte(WorldManager.GetZoneForAetheryteId(originZoneId));
            }

            return OracleInventoryManager.GetItemAmount(GysahlGreenItemId) >= MainSettings.Instance.ChocoboGreensRestockAmount
                       ? RestockGysahlGreensResult.Success
                       : RestockGysahlGreensResult.Failure;
        }

        private static async Task<bool> OpenCorrectShopWindow(uint npcId)
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
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));
            SelectIconString.ClickSlot(0);
            await Coroutine.Wait(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay), () => Shop.Open);

            return Shop.Open;
        }
    }

    internal enum RestockGysahlGreensResult
    {
        Success,

        Failure
    }
}