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
                Logger.SendErrorLog(
                                    "Attempting to equip the Paladin Zenith shield when we're not a Gladiator/Paladin, please let Kataera know on the Protosynth Discord.");
            }

            var shieldSlot = InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == ShieldSlotId);
            if (shieldSlot != null && shieldSlot.RawItemId != PaladinShield)
            {
                if (!ConditionParser.HasItem(PaladinShield))
                {
                    Logger.SendErrorLog("You do not have the Zenith relic shield for " + OracleClassManager.GetClassJobName(Core.Player.CurrentJob) + ".");
                    OracleBot.StopOracle("Character does not have a Zenith shield for the current class.");
                    return;
                }

                Logger.SendLog("Equipping the Zenith shield.");
                var watchItemSlot = InventoryManager.FilledInventoryAndArmory.FirstOrDefault(bs => bs.RawItemId == PaladinShield);
                if (watchItemSlot != null)
                {
                    Logger.SendDebugLog("Zenith weapon is in bag: " + watchItemSlot.BagId + ", slot: " + watchItemSlot.Slot + ".");
                    watchItemSlot.Move(shieldSlot);
                }
            }
        }

        private static async Task EquipZenithWeapon()
        {
            var classJobWeapon = GetClassJobZenithWeapon(Core.Player.CurrentJob);
            if (classJobWeapon == 0)
            {
                Logger.SendErrorLog("Could not determine the Zenith relic weapon for the current class, please let Kataera know on the Protosynth Discord.");
            }

            var weaponSlot = InventoryManager.EquippedItems.FirstOrDefault(bs => bs.Slot == WeaponSlotId);
            if (weaponSlot != null && weaponSlot.RawItemId != classJobWeapon)
            {
                if (!ConditionParser.HasItem(classJobWeapon))
                {
                    Logger.SendErrorLog("You do not have a Zenith relic weapon for " + OracleClassManager.GetClassJobName(Core.Player.CurrentJob) + ".");
                    OracleBot.StopOracle("Character does not have a Zenith weapon for the current class.");
                    return;
                }

                Logger.SendLog("Equipping the Zenith weapon.");
                var watchItemSlot = InventoryManager.FilledInventoryAndArmory.FirstOrDefault(bs => bs.RawItemId == classJobWeapon);
                if (watchItemSlot != null)
                {
                    Logger.SendDebugLog("Zenith weapon is in bag: " + watchItemSlot.BagId + ", slot: " + watchItemSlot.Slot + ".");
                    watchItemSlot.Move(weaponSlot);
                }
            }
        }

        private static uint GetClassJobZenithWeapon(ClassJobType classJob)
        {
            switch (classJob)
            {
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
                case ClassJobType.Summoner:
                    return SummonerWeapon;
                case ClassJobType.Scholar:
                    return ScholarWeapon;
                case ClassJobType.Ninja:
                    return NinjaWeapon;
                default:
                    Logger.SendErrorLog("Current class cannot farm Atma, please make sure you have your job soul crystal equipped.");
                    return 0;
            }
        }

        public static async Task<bool> Main()
        {
            if (!OracleClassManager.ClassJobCanFarmAtma(Core.Player.CurrentJob))
            {
                OracleBot.StopOracle("Current class cannot farm Atma.");
                return true;
            }

            Logger.SendErrorLog("Atma grind mode is not yet implemented.");
            OracleBot.StopOracle("Atma grind mode is not yet implemented.");

            return true;
        }
    }
}