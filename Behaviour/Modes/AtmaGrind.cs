using System;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.NeoProfiles;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    internal static class AtmaGrind
    {
        private const uint WeaponPaladinItemId = 6257;
        private const uint ShieldPaladinItemId = 6266;
        private const uint WeaponMonkItemId = 6258;
        private const uint WeaponWarriorItemId = 6259;
        private const uint WeaponDragoonItemId = 6260;
        private const uint WeaponBardItemId = 6261;
        private const uint WeaponBlackMageItemId = 6263;
        private const uint WeaponSummonerItemId = 6264;
        private const uint WeaponScholarItemId = 6265;
        private const uint WeaponWhiteMageItemId = 6262;
        private const uint WeaponNinjaItemId = 9250;
        private const uint AtmaLionItemId = 7858;
        private const uint AtmaWaterBearerItemId = 7853;
        private const uint AtmaRamItemId = 7856;
        private const uint AtmaCrabItemId = 7862;
        private const uint AtmaFishItemId = 7859;
        private const uint AtmaBullItemId = 7855;
        private const uint AtmaScalesItemId = 7861;
        private const uint AtmaTwinsItemId = 7857;
        private const uint AtmaScorpionItemId = 7852;
        private const uint AtmaArcherItemId = 7860;
        private const uint AtmaGoatItemId = 7854;
        private const uint AtmaMaidenItemId = 7851;

        private const ushort AtmaLionZoneId = 180;
        private const ushort AtmaWaterBearerZoneId = 139;
        private const ushort AtmaRamZoneId = 134;
        private const ushort AtmaCrabZoneId = 138;
        private const ushort AtmaFishZoneId = 135;
        private const ushort AtmaBullZoneId = 145;
        private const ushort AtmaScalesZoneId = 141;
        private const ushort AtmaTwinsZoneId = 140;
        private const ushort AtmaScorpionZoneId = 146;
        private const ushort AtmaArcherZoneId = 154;
        private const ushort AtmaGoatZoneId = 152;
        private const ushort AtmaMaidenZoneId = 148;

        private static async Task EquipMainHand()
        {
            var equipResult = await OracleInventoryManager.EquipItem(GetClassJobZenithWeapon(Core.Player.CurrentJob), EquipmentSlot.MainHand);

            switch (equipResult)
            {
                case EquipItemResult.ItemNotFound:
                    Logger.SendErrorLog("Could not find the Zenith Relic Weapon for the current class.");
                    OracleBot.StopOracle("Can't find Zenith Relic Weapon in character inventory.");
                    break;
                case EquipItemResult.BagSlotNotFound:
                    Logger.SendDebugLog("Could not find the bag slot for the Zenith Relic Weapon.");
                    break;
                case EquipItemResult.Success:
                    Logger.SendLog("Successfully equipped " + OracleInventoryManager.GetEquipmentSlotBagSlot(EquipmentSlot.MainHand).EnglishName + ".");
                    break;
                case EquipItemResult.Failure:
                    Logger.SendDebugLog("Equipping the Zenith Relic Weapon failed.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task EquipOffHand()
        {
            var equipResult = await OracleInventoryManager.EquipItem(ShieldPaladinItemId, EquipmentSlot.OffHand);

            switch (equipResult)
            {
                case EquipItemResult.ItemNotFound:
                    Logger.SendErrorLog("Could not find the Zenith Relic Offhand for the current class.");
                    OracleBot.StopOracle("Can't find Zenith Relic Offhand in character inventory.");
                    break;
                case EquipItemResult.BagSlotNotFound:
                    Logger.SendDebugLog("Could not find the bag slot for the Zenith Relic Offhand.");
                    break;
                case EquipItemResult.Success:
                    Logger.SendLog("Successfully equipped " + OracleInventoryManager.GetEquipmentSlotBagSlot(EquipmentSlot.OffHand).EnglishName + ".");
                    break;
                case EquipItemResult.Failure:
                    Logger.SendDebugLog("Equipping the Zenith Relic Offhand failed.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static uint GetClassJobZenithWeapon(ClassJobType classJob)
        {
            switch (classJob)
            {
                case ClassJobType.Adventurer:
                    return 0;
                case ClassJobType.Gladiator:
                    return WeaponPaladinItemId;
                case ClassJobType.Pugilist:
                    return WeaponMonkItemId;
                case ClassJobType.Marauder:
                    return WeaponWarriorItemId;
                case ClassJobType.Lancer:
                    return WeaponDragoonItemId;
                case ClassJobType.Archer:
                    return WeaponBardItemId;
                case ClassJobType.Conjurer:
                    return WeaponWhiteMageItemId;
                case ClassJobType.Thaumaturge:
                    return WeaponBlackMageItemId;
                case ClassJobType.Carpenter:
                    return 0;
                case ClassJobType.Blacksmith:
                    return 0;
                case ClassJobType.Armorer:
                    return 0;
                case ClassJobType.Goldsmith:
                    return 0;
                case ClassJobType.Leatherworker:
                    return 0;
                case ClassJobType.Weaver:
                    return 0;
                case ClassJobType.Alchemist:
                    return 0;
                case ClassJobType.Culinarian:
                    return 0;
                case ClassJobType.Miner:
                    return 0;
                case ClassJobType.Botanist:
                    return 0;
                case ClassJobType.Fisher:
                    return 0;
                case ClassJobType.Paladin:
                    return WeaponPaladinItemId;
                case ClassJobType.Monk:
                    return WeaponMonkItemId;
                case ClassJobType.Warrior:
                    return WeaponWarriorItemId;
                case ClassJobType.Dragoon:
                    return WeaponDragoonItemId;
                case ClassJobType.Bard:
                    return WeaponBardItemId;
                case ClassJobType.WhiteMage:
                    return WeaponWhiteMageItemId;
                case ClassJobType.BlackMage:
                    return WeaponBlackMageItemId;
                case ClassJobType.Arcanist:
                    return WeaponSummonerItemId;
                case ClassJobType.Summoner:
                    return WeaponSummonerItemId;
                case ClassJobType.Scholar:
                    return WeaponScholarItemId;
                case ClassJobType.Rogue:
                    return WeaponNinjaItemId;
                case ClassJobType.Ninja:
                    return WeaponNinjaItemId;
                case ClassJobType.Machinist:
                    return 0;
                case ClassJobType.DarkKnight:
                    return 0;
                case ClassJobType.Astrologian:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(classJob), classJob, null);
            }
        }

        public static async Task<bool> Main()
        {
            if (!OracleClassManager.ClassJobCanFarmAtma(Core.Player.CurrentJob))
            {
                Logger.SendErrorLog("Your current class (" + OracleClassManager.GetClassJobName(Core.Player.CurrentJob) + ") cannot farm Atma.");
                OracleBot.StopOracle("Current class cannot farm Atma.");
                return true;
            }

            var weaponId = GetClassJobZenithWeapon(Core.Player.CurrentJob);
            if (!OracleInventoryManager.IsItemEquipped(weaponId, EquipmentSlot.MainHand))
            {
                await EquipMainHand();
            }

            // Special case for Gladiator/Paladin shield.
            if ((Core.Player.CurrentJob == ClassJobType.Gladiator || Core.Player.CurrentJob == ClassJobType.Paladin)
                && !OracleInventoryManager.IsItemEquipped(ShieldPaladinItemId, EquipmentSlot.OffHand))
            {
                await EquipOffHand();
            }

            if (!ConditionParser.HasAtLeast(AtmaLionItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaLionZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaLionItemId)) + " Atma of the Lion.");
                await ZoneChange.HandleZoneChange(AtmaLionZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaWaterBearerItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaWaterBearerZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaWaterBearerItemId))
                               + " Atma of the Water-bearer.");
                await ZoneChange.HandleZoneChange(AtmaWaterBearerZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaRamItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaRamZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaRamItemId)) + " Atma of the Ram.");
                await ZoneChange.HandleZoneChange(AtmaRamZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaCrabItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaCrabZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaCrabItemId)) + " Atma of the Crab.");
                await ZoneChange.HandleZoneChange(AtmaCrabZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaFishItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaFishZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaFishItemId)) + " Atma of the Fish.");
                await ZoneChange.HandleZoneChange(AtmaFishZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaBullItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaBullZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaBullItemId)) + " Atma of the Bull.");
                await ZoneChange.HandleZoneChange(AtmaBullZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaScalesItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaScalesZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaScalesItemId)) + " Atma of the Scales.");
                await ZoneChange.HandleZoneChange(AtmaScalesZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaTwinsItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaTwinsZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaTwinsItemId)) + " Atma of the Twins.");
                await ZoneChange.HandleZoneChange(AtmaTwinsZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaScorpionItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaScorpionZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaScorpionItemId))
                               + " Atma of the Scorpion.");
                await ZoneChange.HandleZoneChange(AtmaScorpionZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaArcherItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaArcherZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaArcherItemId)) + " Atma of the Archer.");
                await ZoneChange.HandleZoneChange(AtmaArcherZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaGoatItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaGoatZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaGoatItemId)) + " Atma of the Goat.");
                await ZoneChange.HandleZoneChange(AtmaGoatZoneId);
            }
            else if (!ConditionParser.HasAtLeast(AtmaMaidenItemId, ModeSettings.Instance.AtmaToFarm))
            {
                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == AtmaMaidenZoneId)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AtmaToFarm - OracleInventoryManager.GetItemAmount(AtmaMaidenItemId)) + " Atma of the Maiden.");
                await ZoneChange.HandleZoneChange(AtmaMaidenZoneId);
            }
            else if (!Core.Player.InCombat)
            {
                Logger.SendLog("We have collected " + ModeSettings.Instance.AtmaToFarm + " of every Atma! Stopping Oracle.");
                await OracleTeleportManager.TeleportToClosestCity();
                OracleBot.StopOracle("We are done!");
            }

            return true;
        }
    }
}