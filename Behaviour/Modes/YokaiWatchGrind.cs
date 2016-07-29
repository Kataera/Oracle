using System;
using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

using Oracle.Behaviour.Tasks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Data;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    public static class YokaiWatchGrind
    {
        private const uint YokaiWatchItem = 15222;
        private const uint BracerSlotId = 10;

        private static bool ignoreJibanyan;
        private static bool ignoreKomasan;
        private static bool ignoreUsapyon;
        private static bool ignoreWhisper;
        private static bool ignoreShogunyan;
        private static bool ignoreHovernyan;
        private static bool ignoreKomajiro;
        private static bool ignoreNoko;
        private static bool ignoreVenoct;
        private static bool ignoreKyubi;
        private static bool ignoreRobonyan;
        private static bool ignoreBlizzaria;
        private static bool ignoreManjimutt;

        private static async Task EquipYokaiWatch()
        {
            var bracerSlot = InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == BracerSlotId);
            if (bracerSlot != null && bracerSlot.RawItemId != YokaiWatchItem)
            {
                if (!ConditionParser.HasItem(15222))
                {
                    Logger.SendErrorLog(
                                        "You do not have a Yo-kai Watch in your inventory or armory. Please pick one up by doing the event introduction quest prior to starting the bot.");
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
        }

        private static uint GetMedalZone(Yokai yokai)
        {
            YokaiMinion minion;
            int zoneIndex;
            switch (yokai)
            {
                case Yokai.Jibanyan:
                    minion = YokaiMinions.Jibanyan;
                    zoneIndex = YokaiSettings.Instance.JibanyanZoneChoice;
                    break;
                case Yokai.Komasan:
                    minion = YokaiMinions.Komasan;
                    zoneIndex = YokaiSettings.Instance.KomasanZoneChoice;
                    break;
                case Yokai.Usapyon:
                    minion = YokaiMinions.Usapyon;
                    zoneIndex = YokaiSettings.Instance.UsapyonZoneChoice;
                    break;
                case Yokai.Whisper:
                    minion = YokaiMinions.Whisper;
                    zoneIndex = YokaiSettings.Instance.WhisperZoneChoice;
                    break;
                case Yokai.Shogunyan:
                    minion = YokaiMinions.Shogunyan;
                    zoneIndex = YokaiSettings.Instance.ShogunyanZoneChoice;
                    break;
                case Yokai.Hovernyan:
                    minion = YokaiMinions.Hovernyan;
                    zoneIndex = YokaiSettings.Instance.HovernyanZoneChoice;
                    break;
                case Yokai.Komajiro:
                    minion = YokaiMinions.Komajiro;
                    zoneIndex = YokaiSettings.Instance.KomajiroZoneChoice;
                    break;
                case Yokai.Noko:
                    minion = YokaiMinions.Noko;
                    zoneIndex = YokaiSettings.Instance.NokoZoneChoice;
                    break;
                case Yokai.Venoct:
                    minion = YokaiMinions.Venoct;
                    zoneIndex = YokaiSettings.Instance.VenoctZoneChoice;
                    break;
                case Yokai.Kyubi:
                    minion = YokaiMinions.Kyubi;
                    zoneIndex = YokaiSettings.Instance.KyubiZoneChoice;
                    break;
                case Yokai.Robonyan:
                    minion = YokaiMinions.Robonyan;
                    zoneIndex = YokaiSettings.Instance.RobonyanZoneChoice;
                    break;
                case Yokai.Blizzaria:
                    minion = YokaiMinions.Blizzaria;
                    zoneIndex = YokaiSettings.Instance.BlizzariaZoneChoice;
                    break;
                case Yokai.Manjimutt:
                    minion = YokaiMinions.Manjimutt;
                    zoneIndex = YokaiSettings.Instance.ManjimuttZoneChoice;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(yokai), yokai, null);
            }

            switch (zoneIndex)
            {
                case 1:
                    return minion.MedalZoneOne;
                case 2:
                    return minion.MedalZoneTwo;
                case 3:
                    return minion.MedalZoneThree;
                default:
                    Logger.SendErrorLog("Medal zone index out of bounds. Please let Kataera know this occured at the Oracle Discord server.");
                    return minion.MedalZoneOne;
            }
        }

        public static async Task<bool> Main()
        {
            // Summoning a chocobo dismisses your minion, meaning we can't use it here.
            if (!Chocobo.BlockSummon)
            {
                Chocobo.BlockSummon = true;
            }

            if (Poi.Current.Type == PoiType.Kill)
            {
                await CombatHandler.HandleCombat();
                return true;
            }

            if (!ConditionParser.HasAtLeast(YokaiMinions.Jibanyan.MedalItemId, YokaiSettings.Instance.JibanyanMedalsToFarm) && !ignoreJibanyan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Jibanyan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Jibanyan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreJibanyan = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Jibanyan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Komasan.MedalItemId, YokaiSettings.Instance.KomasanMedalsToFarm) && !ignoreKomasan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Komasan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Komasan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreKomasan = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Komasan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Usapyon.MedalItemId, YokaiSettings.Instance.UsapyonMedalsToFarm) && !ignoreUsapyon)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Usapyon.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Usapyon.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreUsapyon = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Usapyon));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Whisper.MedalItemId, YokaiSettings.Instance.WhisperMedalsToFarm) && !ignoreWhisper)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Whisper.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Whisper.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreWhisper = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Whisper));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Shogunyan.MedalItemId, YokaiSettings.Instance.ShogunyanMedalsToFarm) && !ignoreShogunyan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Shogunyan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Shogunyan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreShogunyan = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Shogunyan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Hovernyan.MedalItemId, YokaiSettings.Instance.HovernyanMedalsToFarm) && !ignoreHovernyan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Hovernyan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Hovernyan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreHovernyan = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Hovernyan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Komajiro.MedalItemId, YokaiSettings.Instance.KomajiroMedalsToFarm) && !ignoreKomajiro)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Komajiro.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Komajiro.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreKomajiro = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Komajiro));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Noko.MedalItemId, YokaiSettings.Instance.NokoMedalsToFarm) && !ignoreNoko)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Noko.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Noko.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreNoko = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Noko));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Venoct.MedalItemId, YokaiSettings.Instance.VenoctMedalsToFarm) && !ignoreVenoct)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Venoct.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Venoct.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreVenoct = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Venoct));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Kyubi.MedalItemId, YokaiSettings.Instance.KyubiMedalsToFarm) && !ignoreKyubi)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Kyubi.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Kyubi.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreKyubi = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Kyubi));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Robonyan.MedalItemId, YokaiSettings.Instance.RobonyanMedalsToFarm) && !ignoreRobonyan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Robonyan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Robonyan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreRobonyan = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Robonyan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Blizzaria.MedalItemId, YokaiSettings.Instance.BlizzariaMedalsToFarm) && !ignoreBlizzaria)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Blizzaria.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Blizzaria.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreBlizzaria = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Blizzaria));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Manjimutt.MedalItemId, YokaiSettings.Instance.ManjimuttMedalsToFarm) && !ignoreManjimutt)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Manjimutt.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Manjimutt.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreManjimutt = true;
                    }
                }

                await TeleportIfNeeded(GetMedalZone(Yokai.Manjimutt));
            }
            else
            {
                Logger.SendLog("We've farmed all the medals we need for all minions! Stopping Oracle.");
                TreeRoot.Stop("We're done!");
            }

            if (Poi.Current.Type == PoiType.Fate || OracleFateManager.CurrentFateId != 0)
            {
                await FateHandler.HandleFate();
            }

            else if (Poi.Current.Type == PoiType.Wait)
            {
                await WaitHandler.HandleWait();
            }

            return true;
        }

        public static void ResetIgnoredYokai()
        {
            ignoreJibanyan = false;
            ignoreKomasan = false;
            ignoreUsapyon = false;
            ignoreWhisper = false;
            ignoreShogunyan = false;
            ignoreHovernyan = false;
            ignoreKomajiro = false;
            ignoreNoko = false;
            ignoreVenoct = false;
            ignoreKyubi = false;
            ignoreRobonyan = false;
            ignoreBlizzaria = false;
            ignoreManjimutt = false;
        }

        private static async Task TeleportIfNeeded(uint zone)
        {
            if (!Core.Player.InCombat && WorldManager.ZoneId != zone)
            {
                await ZoneChangeHandler.HandleZoneChange(zone);
            }
        }
    }
}