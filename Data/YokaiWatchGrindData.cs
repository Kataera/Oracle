using System;
using System.Collections.Generic;

using Oracle.Enumerations;
using Oracle.Settings;
using Oracle.Structs;

namespace Oracle.Data
{
    internal class YokaiWatchGrindData
    {
        internal const uint YokaiMedal = 15167;
        internal const uint YokaiWatchItem = 15222;

        internal static YokaiMinion Jibanyan = new YokaiMinion
        {
            MinionId = YokaiMinionId.Jibanyan,
            EnglishName = "Jibanyan",
            MedalItemId = 15168,
            MedalZoneOne = 148,
            MedalZoneTwo = 135,
            MedalZoneThree = 141,
            Ignored = false
        };

        internal static YokaiMinion Komasan = new YokaiMinion
        {
            MinionId = YokaiMinionId.Komasan,
            EnglishName = "Komasan",
            MedalItemId = 15169,
            MedalZoneOne = 152,
            MedalZoneTwo = 138,
            MedalZoneThree = 145,
            Ignored = false
        };

        internal static YokaiMinion Usapyon = new YokaiMinion
        {
            MinionId = YokaiMinionId.Usapyon,
            EnglishName = "USApyon",
            MedalItemId = 15180,
            MedalZoneOne = 180,
            MedalZoneTwo = 134,
            MedalZoneThree = 140,
            Ignored = false
        };

        internal static YokaiMinion Whisper = new YokaiMinion
        {
            MinionId = YokaiMinionId.Whisper,
            EnglishName = "Whisper",
            MedalItemId = 15170,
            MedalZoneOne = 153,
            MedalZoneTwo = 139,
            MedalZoneThree = 146,
            Ignored = false
        };

        internal static YokaiMinion Shogunyan = new YokaiMinion
        {
            MinionId = YokaiMinionId.Shogunyan,
            EnglishName = "Shogunyan",
            MedalItemId = 15177,
            MedalZoneOne = 135,
            MedalZoneTwo = 141,
            MedalZoneThree = 152,
            Ignored = false
        };

        internal static YokaiMinion Hovernyan = new YokaiMinion
        {
            MinionId = YokaiMinionId.Hovernyan,
            EnglishName = "Hovernyan",
            MedalItemId = 15178,
            MedalZoneOne = 138,
            MedalZoneTwo = 145,
            MedalZoneThree = 153,
            Ignored = false
        };

        internal static YokaiMinion Komajiro = new YokaiMinion
        {
            MinionId = YokaiMinionId.Komajiro,
            EnglishName = "Komajiro",
            MedalItemId = 15173,
            MedalZoneOne = 141,
            MedalZoneTwo = 152,
            MedalZoneThree = 138,
            Ignored = false
        };

        internal static YokaiMinion Noko = new YokaiMinion
        {
            MinionId = YokaiMinionId.Noko,
            EnglishName = "Noko",
            MedalItemId = 15175,
            MedalZoneOne = 146,
            MedalZoneTwo = 154,
            MedalZoneThree = 180,
            Ignored = false
        };

        internal static YokaiMinion Venoct = new YokaiMinion
        {
            MinionId = YokaiMinionId.Venoct,
            EnglishName = "Venoct",
            MedalItemId = 15176,
            MedalZoneOne = 134,
            MedalZoneTwo = 140,
            MedalZoneThree = 148,
            Ignored = false
        };

        internal static YokaiMinion Kyubi = new YokaiMinion
        {
            MinionId = YokaiMinionId.Kyubi,
            EnglishName = "Kyubi",
            MedalItemId = 15172,
            MedalZoneOne = 140,
            MedalZoneTwo = 148,
            MedalZoneThree = 135,
            Ignored = false
        };

        internal static YokaiMinion Robonyan = new YokaiMinion
        {
            MinionId = YokaiMinionId.Robonyan,
            EnglishName = "Robonyan F-type",
            MedalItemId = 15179,
            MedalZoneOne = 139,
            MedalZoneTwo = 146,
            MedalZoneThree = 154,
            Ignored = false
        };

        internal static YokaiMinion Blizzaria = new YokaiMinion
        {
            MinionId = YokaiMinionId.Blizzaria,
            EnglishName = "Blizzaria",
            MedalItemId = 15171,
            MedalZoneOne = 154,
            MedalZoneTwo = 180,
            MedalZoneThree = 134,
            Ignored = false
        };

        internal static YokaiMinion Manjimutt = new YokaiMinion
        {
            MinionId = YokaiMinionId.Manjimutt,
            EnglishName = "Manjimutt",
            MedalItemId = 15174,
            MedalZoneOne = 145,
            MedalZoneTwo = 153,
            MedalZoneThree = 139,
            Ignored = false
        };

        internal static IEnumerable<YokaiMinion> Minions => new List<YokaiMinion>
        {
            Jibanyan,
            Komasan,
            Usapyon,
            Whisper,
            Shogunyan,
            Hovernyan,
            Komajiro,
            Noko,
            Venoct,
            Kyubi,
            Robonyan,
            Blizzaria,
            Manjimutt
        };

        internal static int GetMedalsToFarm(YokaiMinion minion)
        {
            switch (minion.MinionId)
            {
                case YokaiMinionId.Jibanyan:
                    return ModeSettings.Instance.JibanyanMedalsToFarm;
                case YokaiMinionId.Komasan:
                    return ModeSettings.Instance.KomasanMedalsToFarm;
                case YokaiMinionId.Usapyon:
                    return ModeSettings.Instance.UsapyonMedalsToFarm;
                case YokaiMinionId.Whisper:
                    return ModeSettings.Instance.WhisperMedalsToFarm;
                case YokaiMinionId.Shogunyan:
                    return ModeSettings.Instance.ShogunyanMedalsToFarm;
                case YokaiMinionId.Hovernyan:
                    return ModeSettings.Instance.HovernyanMedalsToFarm;
                case YokaiMinionId.Komajiro:
                    return ModeSettings.Instance.KomajiroMedalsToFarm;
                case YokaiMinionId.Noko:
                    return ModeSettings.Instance.NokoMedalsToFarm;
                case YokaiMinionId.Venoct:
                    return ModeSettings.Instance.VenoctMedalsToFarm;
                case YokaiMinionId.Kyubi:
                    return ModeSettings.Instance.KyubiMedalsToFarm;
                case YokaiMinionId.Robonyan:
                    return ModeSettings.Instance.RobonyanMedalsToFarm;
                case YokaiMinionId.Blizzaria:
                    return ModeSettings.Instance.BlizzariaMedalsToFarm;
                case YokaiMinionId.Manjimutt:
                    return ModeSettings.Instance.ManjimuttMedalsToFarm;
                default:
                    throw new ArgumentOutOfRangeException(nameof(minion), minion, null);
            }
        }

        internal static uint GetNormalMedalZone()
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

        internal static int GetZoneChoice(YokaiMinion minion)
        {
            switch (minion.MinionId)
            {
                case YokaiMinionId.Jibanyan:
                    return ModeSettings.Instance.JibanyanZoneChoice;
                case YokaiMinionId.Komasan:
                    return ModeSettings.Instance.KomasanZoneChoice;
                case YokaiMinionId.Usapyon:
                    return ModeSettings.Instance.UsapyonZoneChoice;
                case YokaiMinionId.Whisper:
                    return ModeSettings.Instance.WhisperZoneChoice;
                case YokaiMinionId.Shogunyan:
                    return ModeSettings.Instance.ShogunyanZoneChoice;
                case YokaiMinionId.Hovernyan:
                    return ModeSettings.Instance.HovernyanZoneChoice;
                case YokaiMinionId.Komajiro:
                    return ModeSettings.Instance.KomajiroZoneChoice;
                case YokaiMinionId.Noko:
                    return ModeSettings.Instance.NokoZoneChoice;
                case YokaiMinionId.Venoct:
                    return ModeSettings.Instance.VenoctZoneChoice;
                case YokaiMinionId.Kyubi:
                    return ModeSettings.Instance.KyubiZoneChoice;
                case YokaiMinionId.Robonyan:
                    return ModeSettings.Instance.RobonyanZoneChoice;
                case YokaiMinionId.Blizzaria:
                    return ModeSettings.Instance.BlizzariaZoneChoice;
                case YokaiMinionId.Manjimutt:
                    return ModeSettings.Instance.ManjimuttZoneChoice;
                default:
                    throw new ArgumentOutOfRangeException(nameof(minion), minion, null);
            }
        }
    }
}