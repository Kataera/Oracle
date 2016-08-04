using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    public class ChangeClass
    {
        private static string GetClassJobName(ClassJobType job)
        {
            switch (job)
            {
                case ClassJobType.Adventurer:
                    return "Adventurer";
                case ClassJobType.Gladiator:
                    return "Gladiator";
                case ClassJobType.Pugilist:
                    return "Pugilist";
                case ClassJobType.Marauder:
                    return "Marauder";
                case ClassJobType.Lancer:
                    return "Lancer";
                case ClassJobType.Archer:
                    return "Archer";
                case ClassJobType.Conjurer:
                    return "Conjurer";
                case ClassJobType.Thaumaturge:
                    return "Thaumaturge";
                case ClassJobType.Carpenter:
                    return "Carpenter";
                case ClassJobType.Blacksmith:
                    return "Blacksmith";
                case ClassJobType.Armorer:
                    return "Armorer";
                case ClassJobType.Goldsmith:
                    return "Goldsmith";
                case ClassJobType.Leatherworker:
                    return "Leatherworker";
                case ClassJobType.Weaver:
                    return "Weaver";
                case ClassJobType.Alchemist:
                    return "Alchemist";
                case ClassJobType.Culinarian:
                    return "Culinarian";
                case ClassJobType.Miner:
                    return "Miner";
                case ClassJobType.Botanist:
                    return "Botanist";
                case ClassJobType.Fisher:
                    return "Fisher";
                case ClassJobType.Paladin:
                    return "Paladin (Gladiator)";
                case ClassJobType.Monk:
                    return "Monk (Pugilist)";
                case ClassJobType.Warrior:
                    return "Warrior (Marauder)";
                case ClassJobType.Dragoon:
                    return "Dragoon (Lancer)";
                case ClassJobType.Bard:
                    return "Bard (Archer)";
                case ClassJobType.WhiteMage:
                    return "White Mage (Conjurer)";
                case ClassJobType.BlackMage:
                    return "Black Mage (Thaumaturge)";
                case ClassJobType.Arcanist:
                    return "Arcanist";
                case ClassJobType.Summoner:
                    return "Summoner (Arcanist)";
                case ClassJobType.Scholar:
                    return "Scholar (Arcanist)";
                case ClassJobType.Rogue:
                    return "Rogue";
                case ClassJobType.Ninja:
                    return "Ninja (Rogue)";
                case ClassJobType.Machinist:
                    return "Machinist";
                case ClassJobType.DarkKnight:
                    return "Dark Knight";
                case ClassJobType.Astrologian:
                    return "Astrologian";
                default:
                    throw new ArgumentOutOfRangeException(nameof(job), job, null);
            }
        }

        private static bool IsClassJobEnabled(ClassJobType job)
        {
            switch (job)
            {
                case ClassJobType.Adventurer:
                    return false;
                case ClassJobType.Gladiator:
                    return ClassSettings.Instance.LevelGladiator;
                case ClassJobType.Pugilist:
                    return ClassSettings.Instance.LevelPugilist;
                case ClassJobType.Marauder:
                    return ClassSettings.Instance.LevelMarauder;
                case ClassJobType.Lancer:
                    return ClassSettings.Instance.LevelLancer;
                case ClassJobType.Archer:
                    return ClassSettings.Instance.LevelArcher;
                case ClassJobType.Conjurer:
                    return ClassSettings.Instance.LevelConjurer;
                case ClassJobType.Thaumaturge:
                    return ClassSettings.Instance.LevelThaumaturge;
                case ClassJobType.Carpenter:
                    return false;
                case ClassJobType.Blacksmith:
                    return false;
                case ClassJobType.Armorer:
                    return false;
                case ClassJobType.Goldsmith:
                    return false;
                case ClassJobType.Leatherworker:
                    return false;
                case ClassJobType.Weaver:
                    return false;
                case ClassJobType.Alchemist:
                    return false;
                case ClassJobType.Culinarian:
                    return false;
                case ClassJobType.Miner:
                    return false;
                case ClassJobType.Botanist:
                    return false;
                case ClassJobType.Fisher:
                    return false;
                case ClassJobType.Paladin:
                    return ClassSettings.Instance.LevelGladiator;
                case ClassJobType.Monk:
                    return ClassSettings.Instance.LevelPugilist;
                case ClassJobType.Warrior:
                    return ClassSettings.Instance.LevelMarauder;
                case ClassJobType.Dragoon:
                    return ClassSettings.Instance.LevelLancer;
                case ClassJobType.Bard:
                    return ClassSettings.Instance.LevelArcher;
                case ClassJobType.WhiteMage:
                    return ClassSettings.Instance.LevelConjurer;
                case ClassJobType.BlackMage:
                    return ClassSettings.Instance.LevelThaumaturge;
                case ClassJobType.Arcanist:
                    return ClassSettings.Instance.LevelArcanist;
                case ClassJobType.Summoner:
                    return ClassSettings.Instance.LevelArcanist;
                case ClassJobType.Scholar:
                    return ClassSettings.Instance.LevelArcanist;
                case ClassJobType.Rogue:
                    return ClassSettings.Instance.LevelRogue;
                case ClassJobType.Ninja:
                    return ClassSettings.Instance.LevelRogue;
                case ClassJobType.Machinist:
                    return ClassSettings.Instance.LevelMachinist;
                case ClassJobType.DarkKnight:
                    return ClassSettings.Instance.LevelDarkKnight;
                case ClassJobType.Astrologian:
                    return ClassSettings.Instance.LevelAstrologian;
                default:
                    throw new ArgumentOutOfRangeException(nameof(job), job, null);
            }
        }

        private static bool IsCombatClassJob(ClassJobType job)
        {
            switch (job)
            {
                case ClassJobType.Adventurer:
                    return false;
                case ClassJobType.Gladiator:
                    return true;
                case ClassJobType.Pugilist:
                    return true;
                case ClassJobType.Marauder:
                    return true;
                case ClassJobType.Lancer:
                    return true;
                case ClassJobType.Archer:
                    return true;
                case ClassJobType.Conjurer:
                    return true;
                case ClassJobType.Thaumaturge:
                    return true;
                case ClassJobType.Carpenter:
                    return false;
                case ClassJobType.Blacksmith:
                    return false;
                case ClassJobType.Armorer:
                    return false;
                case ClassJobType.Goldsmith:
                    return false;
                case ClassJobType.Leatherworker:
                    return false;
                case ClassJobType.Weaver:
                    return false;
                case ClassJobType.Alchemist:
                    return false;
                case ClassJobType.Culinarian:
                    return false;
                case ClassJobType.Miner:
                    return false;
                case ClassJobType.Botanist:
                    return false;
                case ClassJobType.Fisher:
                    return false;
                case ClassJobType.Paladin:
                    return true;
                case ClassJobType.Monk:
                    return true;
                case ClassJobType.Warrior:
                    return true;
                case ClassJobType.Dragoon:
                    return true;
                case ClassJobType.Bard:
                    return true;
                case ClassJobType.WhiteMage:
                    return true;
                case ClassJobType.BlackMage:
                    return true;
                case ClassJobType.Arcanist:
                    return true;
                case ClassJobType.Summoner:
                    return true;
                case ClassJobType.Scholar:
                    return true;
                case ClassJobType.Rogue:
                    return true;
                case ClassJobType.Ninja:
                    return true;
                case ClassJobType.Machinist:
                    return true;
                case ClassJobType.DarkKnight:
                    return true;
                case ClassJobType.Astrologian:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(job), job, null);
            }
        }

        public static async Task<ChangeClassResult> Main(ClassJobType job)
        {
            if (!IsCombatClassJob(job))
            {
                Logger.SendErrorLog("Attempted to swap to a non-combat class.");
                return ChangeClassResult.NonCombatClass;
            }

            if (!IsClassJobEnabled(job))
            {
                Logger.SendErrorLog("Attempted to swap to a disabled class.");
                return ChangeClassResult.ClassNotEnabled;
            }

            if (!ClassSettings.Instance.ClassGearsets.ContainsValue(job))
            {
                Logger.SendErrorLog("Attempted to swap to a class with no assigned gearset.");
                return ChangeClassResult.NoGearset;
            }

            Logger.SendLog("Changing class from " + GetClassJobName(Core.Player.CurrentJob) + " to " + GetClassJobName(job) + ".");

            var gearSet = ClassSettings.Instance.ClassGearsets.FirstOrDefault(kvp => kvp.Value.Equals(job)).Key;
            ChatManager.SendChat("/gs change " + gearSet);
            await Coroutine.Wait(TimeSpan.FromSeconds(5), () => Core.Player.CurrentJob == job);

            // Check whether or not the class change succeeded.
            if (Core.Player.CurrentJob != job)
            {
                Logger.SendErrorLog("Class change failed, we've changed to " + GetClassJobName(Core.Player.CurrentJob) + " when we expected " + GetClassJobName(job) + ".");
                return ChangeClassResult.WrongClass;
            }

            Logger.SendLog("Successfully change to " + GetClassJobName(job) + ".");
            return ChangeClassResult.Success;
        }
    }

    public enum ChangeClassResult
    {
        Success,

        NonCombatClass,

        NoGearset,

        ClassNotEnabled,

        WrongClass
    }
}