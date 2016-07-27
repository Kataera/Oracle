using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

using Oracle.Behaviour.Tasks;
using Oracle.Data;
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

            var bracerSlot = InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == BracerSlotId);
            if (bracerSlot != null && bracerSlot.RawItemId != YokaiWatchItem)
            {
                if (!ConditionParser.HasItem(15222))
                {
                    Logger.SendErrorLog("You do not have a Yo-kai Watch in your inventory or armory. Please pick one up by doing the event introduction quest prior to starting the bot.");
                    TreeRoot.Stop("Character does not have a Yo-kai Watch.");
                }

                Logger.SendLog("Equipping the Yo-kai Watch.");
                var watchItemSlot = InventoryManager.FilledInventoryAndArmory.FirstOrDefault(bs => bs.RawItemId == YokaiWatchItem);
                if (watchItemSlot != null)
                {
                    Logger.SendDebugLog("Yo-kai Watch is in bag: " + watchItemSlot.BagId + ", slot: " + watchItemSlot.Slot + ".");
                    watchItemSlot.Move(bracerSlot);
                }
            }

            // TODO: Move this somewhere appropriate.
            var jibanyan = new YokaiMinion { EnglishName = "Jibanyan", MedalItemId = JibanyanMedal, MedalZoneOne = 148, MedalZoneTwo = 135, MedalZoneThree = 141 };
            var komasan = new YokaiMinion { EnglishName = "Komasan", MedalItemId = KomasanMedal, MedalZoneOne = 152, MedalZoneTwo = 138, MedalZoneThree = 145 };
            var usapyon = new YokaiMinion { EnglishName = "USApyon", MedalItemId = UsapyonMedal, MedalZoneOne = 180, MedalZoneTwo = 134, MedalZoneThree = 140 };
            var whisper = new YokaiMinion { EnglishName = "Whisper", MedalItemId = WhisperMedal, MedalZoneOne = 153, MedalZoneTwo = 139, MedalZoneThree = 146 };
            var shogunyan = new YokaiMinion { EnglishName = "Shogunyan", MedalItemId = ShogunyanMedal, MedalZoneOne = 135, MedalZoneTwo = 141, MedalZoneThree = 152 };
            var hovernyan = new YokaiMinion { EnglishName = "Hovernyan", MedalItemId = HovernyanMedal, MedalZoneOne = 138, MedalZoneTwo = 145, MedalZoneThree = 153 };
            var komajiro = new YokaiMinion { EnglishName = "Komajiro", MedalItemId = KomajiroMedal, MedalZoneOne = 141, MedalZoneTwo = 152, MedalZoneThree = 138 };
            var noko = new YokaiMinion { EnglishName = "Noko", MedalItemId = NokoMedal, MedalZoneOne = 146, MedalZoneTwo = 154, MedalZoneThree = 180 };
            var venoct = new YokaiMinion { EnglishName = "Venoct", MedalItemId = VenoctMedal, MedalZoneOne = 134, MedalZoneTwo = 140, MedalZoneThree = 148 };
            var kyubi = new YokaiMinion { EnglishName = "Kyubi", MedalItemId = KyubiMedal, MedalZoneOne = 140, MedalZoneTwo = 148, MedalZoneThree = 135 };
            var robonyan = new YokaiMinion { EnglishName = "Robonyan F-type", MedalItemId = RobonyanMedal, MedalZoneOne = 139, MedalZoneTwo = 146, MedalZoneThree = 154 };
            var blizzaria = new YokaiMinion { EnglishName = "Blizzaria", MedalItemId = BlizzariaMedal, MedalZoneOne = 154, MedalZoneTwo = 180, MedalZoneThree = 134 };
            var manjimutt = new YokaiMinion { EnglishName = "Manjimutt", MedalItemId = ManjimuttMedal, MedalZoneOne = 145, MedalZoneTwo = 153, MedalZoneThree = 139 };
          
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