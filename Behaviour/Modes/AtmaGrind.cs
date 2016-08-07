using System;
using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.NeoProfiles;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Modes
{
    internal static class AtmaGrind
    {
        private const uint WeaponSlotId = 0;
        private const uint ShieldSlotId = 1;

        private const uint PaladinWeapon = 6257;
        private const uint PaladinShield = 6266;
        private const uint MonkWeapon = 6258;
        private const uint WarriorWeapon = 6259;
        private const uint DragoonWeapon = 6260;
        private const uint BardWeapon = 6261;
        private const uint BlackMageWeapon = 6263;
        private const uint SummonerWeapon = 6264;
        private const uint ScholarWeapon = 6265;
        private const uint WhiteMageWeapon = 6262;
        private const uint NinjaWeapon = 9250;

        private static async Task EquipPaladinZenithOffhand()
        {
            if (Core.Player.CurrentJob != ClassJobType.Paladin || Core.Player.CurrentJob != ClassJobType.Gladiator)
            {
                Logger.SendErrorLog("Attempted to equip the Paladin Zenith Relic Shield when we're not a Gladiator/Paladin.");
                return;
            }

            var shieldSlot = InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == ShieldSlotId);
            if (shieldSlot != null && shieldSlot.RawItemId != PaladinShield)
            {
                if (!ConditionParser.HasItem(PaladinShield))
                {
                    Logger.SendErrorLog("You do not have the Paladin Zenith Relic Shield.");
                    OracleBot.StopOracle("Character does not have a Zenith Shield for the current class.");
                    return;
                }

                Logger.SendLog("Equipping the Paladin Zenith Relic Shield.");
                var watchItemSlot = InventoryManager.FilledInventoryAndArmory.FirstOrDefault(bs => bs.RawItemId == PaladinShield);
                if (watchItemSlot != null)
                {
                    Logger.SendDebugLog("Paladin Zenith Relic Shield is in bag: " + watchItemSlot.BagId + ", slot: " + watchItemSlot.Slot + ".");
                    watchItemSlot.Move(shieldSlot);
                }
            }
        }

        private static async Task EquipZenithWeapon()
        {
            var classJobWeapon = GetClassJobZenithWeapon(Core.Player.CurrentJob);
            if (classJobWeapon == 0)
            {
                Logger.SendErrorLog("Could not determine the Zenith Relic Weapon for the current class, please let Kataera know on the Protosynth Discord.");
            }

            var weaponSlot = InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == WeaponSlotId);
            if (weaponSlot != null && weaponSlot.RawItemId != classJobWeapon)
            {
                if (!ConditionParser.HasItem(classJobWeapon))
                {
                    Logger.SendErrorLog("You do not have a Zenith Relic Weapon for " + OracleClassManager.GetClassJobName(Core.Player.CurrentJob) + ".");
                    OracleBot.StopOracle("Character does not have a Zenith Relic Weapon for the current class.");
                    return;
                }

                Logger.SendLog("Equipping the Zenith Relic Weapon.");
                var watchItemSlot = InventoryManager.FilledInventoryAndArmory.FirstOrDefault(bs => bs.RawItemId == classJobWeapon);
                if (watchItemSlot != null)
                {
                    Logger.SendDebugLog("Zenith Relic Weapon is in bag: " + watchItemSlot.BagId + ", slot: " + watchItemSlot.Slot + ".");
                    watchItemSlot.Move(weaponSlot);
                }
            }
        }

        private static uint GetClassJobZenithWeapon(ClassJobType classJob)
        {
            switch (classJob)
            {
                case ClassJobType.Adventurer:
                    return 0;
                case ClassJobType.Gladiator:
                    return PaladinWeapon;
                case ClassJobType.Pugilist:
                    return MonkWeapon;
                case ClassJobType.Marauder:
                    return WarriorWeapon;
                case ClassJobType.Lancer:
                    return DragoonWeapon;
                case ClassJobType.Archer:
                    return BardWeapon;
                case ClassJobType.Conjurer:
                    return WhiteMageWeapon;
                case ClassJobType.Thaumaturge:
                    return BlackMageWeapon;
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
                    return PaladinWeapon;
                case ClassJobType.Monk:
                    return MonkWeapon;
                case ClassJobType.Warrior:
                    return WarriorWeapon;
                case ClassJobType.Dragoon:
                    return DragoonWeapon;
                case ClassJobType.Bard:
                    return BardWeapon;
                case ClassJobType.WhiteMage:
                    return WhiteMageWeapon;
                case ClassJobType.BlackMage:
                    return BlackMageWeapon;
                case ClassJobType.Arcanist:
                    return SummonerWeapon;
                case ClassJobType.Summoner:
                    return SummonerWeapon;
                case ClassJobType.Scholar:
                    return ScholarWeapon;
                case ClassJobType.Rogue:
                    return NinjaWeapon;
                case ClassJobType.Ninja:
                    return NinjaWeapon;
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

            Logger.SendErrorLog("Atma grind mode is not yet implemented.");
            OracleBot.StopOracle("Atma grind mode is not yet implemented.");

            return true;
        }
    }
}