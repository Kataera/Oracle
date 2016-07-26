using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

using Oracle.Behaviour.Tasks;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    public class YokaiWatchGrind
    {
        private const uint YokaiWatchItem = 15222;
        private const uint BracerSlotId = 10;
        private const uint JibanyanMedal = 15168;
        private const uint KomasanMedal = 15169;
        private const uint UsapyonMedal = 15180;
        private const uint WhisperMedal = 15170;
        private const uint ShogunyanMedal = 15177;
        private const uint HovernyanMedal = 15178;
        private const uint KomajiroMedal = 15173;
        private const uint NokoMedal = 15175;
        private const uint VenoctMedal = 15176;
        private const uint KyubiMedal = 15172;
        private const uint RobonyanMedal = 15179;
        private const uint BlizzariaMedal = 15171;
        private const uint ManjimuttMedal = 15174;

        public static async Task<bool> Main()
        {
            // Summoning a chocobo dismisses your minion, meaning we can't use it here.
            if (!Chocobo.BlockSummon)
            {
                Chocobo.BlockSummon = true;
            }

            var bracerSlot = InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == 10);
            if (bracerSlot != null && bracerSlot.RawItemId != 15222)
            {
                if (!ConditionParser.HasItem(15222))
                {
                    Logger.SendErrorLog("You do not have a Yo-kai Watch in your inventory or armory. Please pick one up by doing the event quest prior to running the bot.");
                    TreeRoot.Stop("Character does not have a Yo-kai Watch.");
                }

                Logger.SendLog("Equipping the Yo-kai Watch.");
                var watchItemSlot = InventoryManager.FilledInventoryAndArmory.FirstOrDefault(bs => bs.RawItemId == 15222);
                if (watchItemSlot != null)
                {
                    Logger.SendDebugLog("Yo-kai Watch is in bag: " + watchItemSlot.BagId + ", slot: " + watchItemSlot.Slot + ".");
                    watchItemSlot.Move(bracerSlot);
                }
            }

            if (Poi.Current.Type == PoiType.Kill)
            {
                await CombatHandler.HandleCombat();
            }

            else if (Poi.Current.Type == PoiType.Fate || OracleFateManager.CurrentFateId != 0)
            {
                await FateHandler.HandleFate();
            }

            else if (Poi.Current.Type == PoiType.Wait)
            {
                await WaitHandler.HandleWait();
            }

            return true;
        }
    }
}