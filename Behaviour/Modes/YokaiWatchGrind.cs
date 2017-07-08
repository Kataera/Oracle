using System.Threading.Tasks;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.NeoProfiles;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Data;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;
using Oracle.Structs;

namespace Oracle.Behaviour.Modes
{
    internal static class YokaiWatchGrind
    {
        private static bool ignoreNormalMedals;

        private static uint GetMinionMedalZone(YokaiMinion minion)
        {
            var zoneIndex = YokaiWatchGrindData.GetZoneChoice(minion);

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

        private static async Task<HandleMinionResult> HandleMinion(YokaiMinion minion)
        {
            if (ConditionParser.HasAtLeast(minion.MedalItemId, YokaiWatchGrindData.GetMedalsToFarm(minion)))
            {
                return HandleMinionResult.Finished;
            }

            if (minion.Ignored)
            {
                return HandleMinionResult.Ignored;
            }

            if (SummonMinion.CanSummonMinion() && !await SummonMinion.IsMinionSummoned(minion.EnglishName))
            {
                var summonMinionResult = await SummonMinion.Main(minion.EnglishName);
                if (summonMinionResult != SummonMinionResult.Success)
                {
                    minion.Ignored = true;
                    return HandleMinionResult.Failed;
                }
            }

            if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == GetMinionMedalZone(minion))
            {
                return HandleMinionResult.InProgress;
            }

            var medalsToFarm = YokaiWatchGrindData.GetMedalsToFarm(minion) - OracleInventoryManager.GetItemAmount(minion.MedalItemId);
            Logger.SendLog("We need " + medalsToFarm + " Legendary " + minion.EnglishName + " Medal(s). Teleporting to your chosen zone.");
            await ZoneChange.HandleZoneChange(GetMinionMedalZone(minion), true);

            return HandleMinionResult.InProgress;
        }

        internal static async Task<bool> HandleYokaiWatchGrind()
        {
            // Summoning a chocobo dismisses your minion, meaning we can't use it here.
            if (!ChocoboManager.BlockSummon)
            {
                ChocoboManager.BlockSummon = true;
            }

            if (ChocoboManager.Summoned)
            {
                await ChocoboManager.DismissChocobo();
            }

            if (!ConditionParser.HasAtLeast(YokaiWatchGrindData.YokaiMedal, ModeSettings.Instance.YokaiMedalsToFarm))
            {
                if (OracleInventoryManager.IsItemEquipped(YokaiWatchGrindData.YokaiWatchItem, EquipmentSlot.Bracelet))
                {
                    var equipResult = await OracleInventoryManager.EquipItem(YokaiWatchGrindData.YokaiWatchItem, EquipmentSlot.Bracelet);

                    if (equipResult != EquipItemResult.Success)
                    {
                        Logger.SendErrorLog("Unable to equip the Yo-kai Watch.");
                        ignoreNormalMedals = true;
                    }
                }
            }

            foreach (var minion in YokaiWatchGrindData.Minions)
            {
                if (await HandleMinion(minion) == HandleMinionResult.InProgress)
                {
                    return true;
                }
            }

            if (!ConditionParser.HasAtLeast(YokaiWatchGrindData.YokaiMedal, ModeSettings.Instance.YokaiMedalsToFarm) && !ignoreNormalMedals)
            {
                await TeleportIfNeeded(YokaiWatchGrindData.GetNormalMedalZone());
                return true;
            }

            if (Core.Player.InCombat)
            {
                return true;
            }

            Logger.SendLog("We've farmed the medals we need for all minions! Stopping Oracle.");
            await OracleTeleportManager.TeleportToClosestCity();
            OracleBot.StopOracle("We are done!");

            return true;
        }

        internal static void ResetIgnoredYokai()
        {
            ignoreNormalMedals = false;
            YokaiWatchGrindData.Jibanyan.Ignored = false;
            YokaiWatchGrindData.Komasan.Ignored = false;
            YokaiWatchGrindData.Usapyon.Ignored = false;
            YokaiWatchGrindData.Whisper.Ignored = false;
            YokaiWatchGrindData.Shogunyan.Ignored = false;
            YokaiWatchGrindData.Hovernyan.Ignored = false;
            YokaiWatchGrindData.Komajiro.Ignored = false;
            YokaiWatchGrindData.Noko.Ignored = false;
            YokaiWatchGrindData.Venoct.Ignored = false;
            YokaiWatchGrindData.Kyubi.Ignored = false;
            YokaiWatchGrindData.Robonyan.Ignored = false;
            YokaiWatchGrindData.Blizzaria.Ignored = false;
            YokaiWatchGrindData.Manjimutt.Ignored = false;
        }

        private static async Task TeleportIfNeeded(uint zone)
        {
            if (!Core.Player.InCombat && WorldManager.CanTeleport() && WorldManager.ZoneId != zone)
            {
                await ZoneChange.HandleZoneChange(zone, true);
            }
        }
    }

    internal enum HandleMinionResult
    {
        Ignored,

        Failed,

        InProgress,

        Finished
    }
}