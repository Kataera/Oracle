using System;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

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
        private const uint YokaiMedal = 15167;
        private const uint YokaiWatchItem = 15222;

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
        private static bool ignoreNormalMedals;

        private static uint GetMinionMedalZone(YokaiMinion yokai)
        {
            Structs.YokaiMinion minion;
            int zoneIndex;
            switch (yokai)
            {
                case YokaiMinion.Jibanyan:
                    minion = YokaiMinions.Jibanyan;
                    zoneIndex = ModeSettings.Instance.JibanyanZoneChoice;
                    break;
                case YokaiMinion.Komasan:
                    minion = YokaiMinions.Komasan;
                    zoneIndex = ModeSettings.Instance.KomasanZoneChoice;
                    break;
                case YokaiMinion.Usapyon:
                    minion = YokaiMinions.Usapyon;
                    zoneIndex = ModeSettings.Instance.UsapyonZoneChoice;
                    break;
                case YokaiMinion.Whisper:
                    minion = YokaiMinions.Whisper;
                    zoneIndex = ModeSettings.Instance.WhisperZoneChoice;
                    break;
                case YokaiMinion.Shogunyan:
                    minion = YokaiMinions.Shogunyan;
                    zoneIndex = ModeSettings.Instance.ShogunyanZoneChoice;
                    break;
                case YokaiMinion.Hovernyan:
                    minion = YokaiMinions.Hovernyan;
                    zoneIndex = ModeSettings.Instance.HovernyanZoneChoice;
                    break;
                case YokaiMinion.Komajiro:
                    minion = YokaiMinions.Komajiro;
                    zoneIndex = ModeSettings.Instance.KomajiroZoneChoice;
                    break;
                case YokaiMinion.Noko:
                    minion = YokaiMinions.Noko;
                    zoneIndex = ModeSettings.Instance.NokoZoneChoice;
                    break;
                case YokaiMinion.Venoct:
                    minion = YokaiMinions.Venoct;
                    zoneIndex = ModeSettings.Instance.VenoctZoneChoice;
                    break;
                case YokaiMinion.Kyubi:
                    minion = YokaiMinions.Kyubi;
                    zoneIndex = ModeSettings.Instance.KyubiZoneChoice;
                    break;
                case YokaiMinion.Robonyan:
                    minion = YokaiMinions.Robonyan;
                    zoneIndex = ModeSettings.Instance.RobonyanZoneChoice;
                    break;
                case YokaiMinion.Blizzaria:
                    minion = YokaiMinions.Blizzaria;
                    zoneIndex = ModeSettings.Instance.BlizzariaZoneChoice;
                    break;
                case YokaiMinion.Manjimutt:
                    minion = YokaiMinions.Manjimutt;
                    zoneIndex = ModeSettings.Instance.ManjimuttZoneChoice;
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
                    return minion.MedalZoneOne;
            }
        }

        private static uint GetNormalMedalZone()
        {
            switch (ModeSettings.Instance.YokaiMedalZoneChoice)
            {
                case 1:
                    return 134;
                case 2:
                    return 135;
                case 3:
                    return 138;
                case 4:
                    return 139;
                case 5:
                    return 140;
                case 6:
                    return 141;
                case 7:
                    return 145;
                case 8:
                    return 146;
                case 9:
                    return 148;
                case 10:
                    return 152;
                case 11:
                    return 153;
                case 12:
                    return 154;
                case 13:
                    return 180;
                default:
                    return 134;
            }
        }

        public static async Task<bool> Main()
        {
            // Summoning a chocobo dismisses your minion, meaning we can't use it here.
            if (!Chocobo.BlockSummon)
            {
                Chocobo.BlockSummon = true;
            }

            if (Chocobo.Summoned)
            {
                await Chocobo.DismissChocobo();
            }

            if (!ConditionParser.HasAtLeast(YokaiMedal, ModeSettings.Instance.YokaiMedalsToFarm))
            {
                if (OracleInventoryManager.IsItemEquipped(YokaiWatchItem, EquipmentSlot.Bracelet))
                {
                    var equipResult = await OracleInventoryManager.EquipItem(YokaiWatchItem, EquipmentSlot.Bracelet);

                    if (equipResult != EquipItemResult.Success)
                    {
                        Logger.SendErrorLog("Unable to equip the Yo-kai Watch.");
                        ignoreNormalMedals = true;
                    }
                }
            }

            if (!ConditionParser.HasAtLeast(YokaiMinions.Jibanyan.MedalItemId, ModeSettings.Instance.JibanyanMedalsToFarm) && !ignoreJibanyan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Jibanyan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Jibanyan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreJibanyan = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Jibanyan))
                {
                    return true;
                }

                Logger.SendLog("We need "
                               + (ModeSettings.Instance.JibanyanMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Jibanyan.MedalItemId))
                               + " Legendary Jibanyan Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Jibanyan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Komasan.MedalItemId, ModeSettings.Instance.KomasanMedalsToFarm) && !ignoreKomasan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Komasan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Komasan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreKomasan = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Komasan))
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.KomasanMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Komasan.MedalItemId))
                               + " Legendary Komasan Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Komasan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Usapyon.MedalItemId, ModeSettings.Instance.UsapyonMedalsToFarm) && !ignoreUsapyon)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Usapyon.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Usapyon.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreUsapyon = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Usapyon))
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.UsapyonMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Usapyon.MedalItemId))
                               + " Legendary USApyon Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Usapyon));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Whisper.MedalItemId, ModeSettings.Instance.WhisperMedalsToFarm) && !ignoreWhisper)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Whisper.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Whisper.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreWhisper = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Whisper))
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.WhisperMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Whisper.MedalItemId))
                               + " Legendary Whisper Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Whisper));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Shogunyan.MedalItemId, ModeSettings.Instance.ShogunyanMedalsToFarm) && !ignoreShogunyan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Shogunyan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Shogunyan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreShogunyan = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Shogunyan))
                {
                    return true;
                }

                Logger.SendLog("We need "
                               + (ModeSettings.Instance.ShogunyanMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Shogunyan.MedalItemId))
                               + " Legendary Shogunyan Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Shogunyan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Hovernyan.MedalItemId, ModeSettings.Instance.HovernyanMedalsToFarm) && !ignoreHovernyan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Hovernyan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Hovernyan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreHovernyan = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Hovernyan))
                {
                    return true;
                }

                Logger.SendLog("We need "
                               + (ModeSettings.Instance.HovernyanMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Hovernyan.MedalItemId))
                               + " Legendary Hovernyan Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Hovernyan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Komajiro.MedalItemId, ModeSettings.Instance.KomajiroMedalsToFarm) && !ignoreKomajiro)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Komajiro.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Komajiro.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreKomajiro = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Komajiro))
                {
                    return true;
                }

                Logger.SendLog("We need "
                               + (ModeSettings.Instance.KomajiroMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Komajiro.MedalItemId))
                               + " Legendary Komajiro Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Komajiro));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Noko.MedalItemId, ModeSettings.Instance.NokoMedalsToFarm) && !ignoreNoko)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Noko.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Noko.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreNoko = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Noko))
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.NokoMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Noko.MedalItemId))
                               + " Legendary Noko Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Noko));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Venoct.MedalItemId, ModeSettings.Instance.VenoctMedalsToFarm) && !ignoreVenoct)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Venoct.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Venoct.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreVenoct = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Venoct))
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.VenoctMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Venoct.MedalItemId))
                               + " Legendary Venoct Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Venoct));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Kyubi.MedalItemId, ModeSettings.Instance.KyubiMedalsToFarm) && !ignoreKyubi)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Kyubi.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Kyubi.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreKyubi = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Kyubi))
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.KyubiMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Kyubi.MedalItemId))
                               + " Legendary Kyubi Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Kyubi));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Robonyan.MedalItemId, ModeSettings.Instance.RobonyanMedalsToFarm) && !ignoreRobonyan)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Robonyan.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Robonyan.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreRobonyan = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Robonyan))
                {
                    return true;
                }

                Logger.SendLog("We need "
                               + (ModeSettings.Instance.RobonyanMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Robonyan.MedalItemId))
                               + " Legendary Robonyan F-type Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Robonyan));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Blizzaria.MedalItemId, ModeSettings.Instance.BlizzariaMedalsToFarm) && !ignoreBlizzaria)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Blizzaria.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Blizzaria.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreBlizzaria = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Blizzaria))
                {
                    return true;
                }

                Logger.SendLog("We need "
                               + (ModeSettings.Instance.BlizzariaMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Blizzaria.MedalItemId))
                               + " Legendary Blizzaria Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Blizzaria));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMinions.Manjimutt.MedalItemId, ModeSettings.Instance.ManjimuttMedalsToFarm) && !ignoreManjimutt)
            {
                if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(YokaiMinions.Manjimutt.EnglishName))
                {
                    var summonMinionResult = await SummonMinion.Main(YokaiMinions.Manjimutt.EnglishName);
                    if (summonMinionResult != SummonMinionResult.Success)
                    {
                        ignoreManjimutt = true;
                    }
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(YokaiMinion.Manjimutt))
                {
                    return true;
                }

                Logger.SendLog("We need "
                               + (ModeSettings.Instance.ManjimuttMedalsToFarm - OracleInventoryManager.GetItemAmount(YokaiMinions.Manjimutt.MedalItemId))
                               + " Legendary Manjimutt Medal(s).");
                await ZoneChange.HandleZoneChange(GetMinionMedalZone(YokaiMinion.Manjimutt));
            }
            else if (!ConditionParser.HasAtLeast(YokaiMedal, ModeSettings.Instance.YokaiMedalsToFarm) && !ignoreNormalMedals)
            {
                await TeleportIfNeeded(GetNormalMedalZone());
            }
            else if (!Core.Player.InCombat)
            {
                Logger.SendLog("We've farmed the medals we need for all minions! Stopping Oracle.");
                await OracleTeleportManager.TeleportToClosestCity();
                OracleBot.StopOracle("We are done!");
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
            if (!Core.Player.InCombat && WorldManager.CanTeleport() && WorldManager.ZoneId != zone)
            {
                await ZoneChange.HandleZoneChange(zone);
            }
        }
    }
}