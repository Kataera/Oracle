using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;

using Oracle.Enumerations;
using Oracle.Enumerations.TaskResults;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleClassManager
    {
        internal static IEnumerable<ClassJobType> CasterDpsClassJobs => new List<ClassJobType>
        {
            ClassJobType.Arcanist,
            ClassJobType.BlackMage,
            ClassJobType.Summoner,
            ClassJobType.Thaumaturge,
            ClassJobType.RedMage
        };

        internal static Stopwatch ClassChangedTimer { get; set; }

        internal static IEnumerable<ClassJobType> CombatClassJobs
        {
            get
            {
                var combatClassJobs = new List<ClassJobType>();
                combatClassJobs.AddRange(DpsClassJobs);
                combatClassJobs.AddRange(HealerClassJobs);
                combatClassJobs.AddRange(TankClassJobs);

                return combatClassJobs;
            }
        }

        internal static IEnumerable<ClassJobType> DpsClassJobs
        {
            get
            {
                var dpsClassJobs = new List<ClassJobType>();
                dpsClassJobs.AddRange(CasterDpsClassJobs);
                dpsClassJobs.AddRange(MeleeDpsClassJobs);
                dpsClassJobs.AddRange(RangedDpsClassJobs);

                return dpsClassJobs;
            }
        }

        internal static IEnumerable<ClassJobType> HealerClassJobs => new List<ClassJobType>
        {
            ClassJobType.Astrologian,
            ClassJobType.Conjurer,
            ClassJobType.Scholar,
            ClassJobType.WhiteMage
        };

        internal static IEnumerable<ClassJobType> MeleeDpsClassJobs => new List<ClassJobType>
        {
            ClassJobType.Dragoon,
            ClassJobType.Lancer,
            ClassJobType.Monk,
            ClassJobType.Ninja,
            ClassJobType.Pugilist,
            ClassJobType.Rogue,
            ClassJobType.Samurai
        };

        internal static IEnumerable<ClassJobType> RangedDpsClassJobs => new List<ClassJobType>
        {
            ClassJobType.Archer,
            ClassJobType.Bard,
            ClassJobType.Machinist
        };

        internal static IEnumerable<ClassJobType> TankClassJobs => new List<ClassJobType>
        {
            ClassJobType.DarkKnight,
            ClassJobType.Gladiator,
            ClassJobType.Marauder,
            ClassJobType.Paladin,
            ClassJobType.Warrior
        };

        internal static async Task<ChangeClassResult> ChangeClassJob(ClassJobType job)
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

            if (!OracleSettings.Instance.ClassGearsets.ContainsValue(job))
            {
                Logger.SendErrorLog("Attempted to swap to a class with no assigned gearset.");
                return ChangeClassResult.NoGearset;
            }

            var previousJob = Core.Player.CurrentJob;
            var gearSet = OracleSettings.Instance.ClassGearsets.FirstOrDefault(kvp => kvp.Value.Equals(job)).Key;

            Logger.SendLog($"Changing class from {GetClassJobName(previousJob)} to {GetClassJobName(job)}. Waiting 10 seconds to ensure game will let us change.");
            await Coroutine.Wait(TimeSpan.FromSeconds(10), () => GameObjectManager.Attackers.Any());

            if (GameObjectManager.Attackers.Any())
            {
                Logger.SendLog("We're in combat, cancelling class change.");
                return ChangeClassResult.Failed;
            }

            ChatManager.SendChat("/gs change " + gearSet);
            await Coroutine.Wait(TimeSpan.FromSeconds(2), () => Core.Player.CurrentJob == job);

            if (GetBaseClass(Core.Player.CurrentJob) == GetBaseClass(previousJob))
            {
                Logger.SendDebugLog($"Class did not change from {GetClassJobName(Core.Player.CurrentJob)}, likely caused by the game refusing to allow a gearset change.");
                return ChangeClassResult.Failed;
            }

            if (GetBaseClass(Core.Player.CurrentJob) != GetBaseClass(job))
            {
                Logger.SendErrorLog($"Class change failed, current class or job is {GetClassJobName(Core.Player.CurrentJob)} when we were expecting {GetClassJobName(job)}.");
                return ChangeClassResult.WrongClass;
            }

            Logger.SendLog($"Successfully changed to {GetClassJobName(job)}.");
            ClassChangedTimer = Stopwatch.StartNew();
            return ChangeClassResult.Succeeded;
        }

        internal static bool ClassChangeNeeded()
        {
            if (OracleSettings.Instance.OracleFateMode != OracleFateMode.MultiLevelMode)
            {
                return false;
            }

            if (!IsClassJobEnabled(Core.Player.CurrentJob))
            {
                return true;
            }

            var enabledLevels = Core.Player.Levels.OrderBy(kvp => kvp.Value).Where(kvp => IsClassJobEnabled(kvp.Key)).ToArray();
            var highestLevel = ushort.MinValue;

            foreach (var classLevel in enabledLevels)
            {
                if (classLevel.Value > highestLevel)
                {
                    highestLevel = classLevel.Value;
                }
            }

            if (!enabledLevels.Any(kvp => kvp.Value < highestLevel))
            {
                return false;
            }

            var lowerLevels = enabledLevels.Where(kvp => kvp.Value < highestLevel).ToArray();
            return lowerLevels.Any(kvp => kvp.Key != Core.Player.CurrentJob && kvp.Value < GetTrueLevel());
        }

        internal static bool ClassJobCanFarmAtma(ClassJobType classJob)
        {
            switch (classJob)
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
                    return false;
                case ClassJobType.Summoner:
                    return true;
                case ClassJobType.Scholar:
                    return true;
                case ClassJobType.Rogue:
                    return true;
                case ClassJobType.Ninja:
                    return true;
                case ClassJobType.Machinist:
                    return false;
                case ClassJobType.DarkKnight:
                    return false;
                case ClassJobType.Astrologian:
                    return false;
                case ClassJobType.Samurai:
                    return false;
                case ClassJobType.RedMage:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(classJob), classJob, null);
            }
        }

        internal static bool FinishedLevelling()
        {
            if (OracleSettings.Instance.OracleFateMode == OracleFateMode.MultiLevelMode)
            {
                var enabledLevels = Core.Player.Levels.OrderBy(kvp => kvp.Value).Where(kvp => IsClassJobEnabled(kvp.Key)).ToArray();
                return enabledLevels.All(kvp => kvp.Value >= OracleSettings.Instance.MaxLevel);
            }

            if (OracleSettings.Instance.OracleFateMode == OracleFateMode.LevelMode)
            {
                return GetTrueLevel() >= OracleSettings.Instance.MaxLevel;
            }

            Logger.SendErrorLog("Checking whether or not we've finished levelling in a non-levelling mode.");
            return false;
        }

        internal static ClassJobType GetBaseClass(ClassJobType job)
        {
            switch (job)
            {
                case ClassJobType.Adventurer:
                    return ClassJobType.Adventurer;
                case ClassJobType.Gladiator:
                    return ClassJobType.Gladiator;
                case ClassJobType.Pugilist:
                    return ClassJobType.Pugilist;
                case ClassJobType.Marauder:
                    return ClassJobType.Marauder;
                case ClassJobType.Lancer:
                    return ClassJobType.Lancer;
                case ClassJobType.Archer:
                    return ClassJobType.Archer;
                case ClassJobType.Conjurer:
                    return ClassJobType.Conjurer;
                case ClassJobType.Thaumaturge:
                    return ClassJobType.Thaumaturge;
                case ClassJobType.Carpenter:
                    return ClassJobType.Carpenter;
                case ClassJobType.Blacksmith:
                    return ClassJobType.Blacksmith;
                case ClassJobType.Armorer:
                    return ClassJobType.Armorer;
                case ClassJobType.Goldsmith:
                    return ClassJobType.Goldsmith;
                case ClassJobType.Leatherworker:
                    return ClassJobType.Leatherworker;
                case ClassJobType.Weaver:
                    return ClassJobType.Weaver;
                case ClassJobType.Alchemist:
                    return ClassJobType.Alchemist;
                case ClassJobType.Culinarian:
                    return ClassJobType.Culinarian;
                case ClassJobType.Miner:
                    return ClassJobType.Miner;
                case ClassJobType.Botanist:
                    return ClassJobType.Botanist;
                case ClassJobType.Fisher:
                    return ClassJobType.Fisher;
                case ClassJobType.Paladin:
                    return ClassJobType.Gladiator;
                case ClassJobType.Monk:
                    return ClassJobType.Pugilist;
                case ClassJobType.Warrior:
                    return ClassJobType.Marauder;
                case ClassJobType.Dragoon:
                    return ClassJobType.Lancer;
                case ClassJobType.Bard:
                    return ClassJobType.Archer;
                case ClassJobType.WhiteMage:
                    return ClassJobType.Conjurer;
                case ClassJobType.BlackMage:
                    return ClassJobType.Thaumaturge;
                case ClassJobType.Arcanist:
                    return ClassJobType.Arcanist;
                case ClassJobType.Summoner:
                    return ClassJobType.Arcanist;
                case ClassJobType.Scholar:
                    return ClassJobType.Arcanist;
                case ClassJobType.Rogue:
                    return ClassJobType.Rogue;
                case ClassJobType.Ninja:
                    return ClassJobType.Rogue;
                case ClassJobType.Machinist:
                    return ClassJobType.Machinist;
                case ClassJobType.DarkKnight:
                    return ClassJobType.DarkKnight;
                case ClassJobType.Astrologian:
                    return ClassJobType.Astrologian;
                case ClassJobType.Samurai:
                    return ClassJobType.Samurai;
                case ClassJobType.RedMage:
                    return ClassJobType.RedMage;
                default:
                    throw new ArgumentOutOfRangeException(nameof(job), job, null);
            }
        }

        internal static string GetClassJobName(ClassJobType job)
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
                case ClassJobType.Samurai:
                    return "Samurai";
                case ClassJobType.RedMage:
                    return "Red Mage";
                default:
                    throw new ArgumentOutOfRangeException(nameof(job), job, null);
            }
        }

        internal static ClassJobType GetLowestLevelClassJob()
        {
            var enabledLevels = Core.Player.Levels.OrderBy(kvp => kvp.Value).Where(kvp => IsClassJobEnabled(kvp.Key)).ToArray();
            return enabledLevels.FirstOrDefault().Key;
        }

        internal static uint GetTrueLevel()
        {
            var baseClass = GetBaseClass(Core.Player.CurrentJob);
            var trueLevel = Core.Player.Levels.FirstOrDefault(kvp => kvp.Key == baseClass).Value;

            return trueLevel != 0 ? trueLevel : Core.Player.ClassLevel;
        }

        internal static bool IsCasterClassJob(ClassJobType job)
        {
            return CasterDpsClassJobs.Contains(job);
        }

        internal static bool IsClassJobEnabled(ClassJobType job)
        {
            switch (job)
            {
                case ClassJobType.Adventurer:
                    return false;
                case ClassJobType.Gladiator:
                    return OracleSettings.Instance.LevelGladiator;
                case ClassJobType.Pugilist:
                    return OracleSettings.Instance.LevelPugilist;
                case ClassJobType.Marauder:
                    return OracleSettings.Instance.LevelMarauder;
                case ClassJobType.Lancer:
                    return OracleSettings.Instance.LevelLancer;
                case ClassJobType.Archer:
                    return OracleSettings.Instance.LevelArcher;
                case ClassJobType.Conjurer:
                    return OracleSettings.Instance.LevelConjurer;
                case ClassJobType.Thaumaturge:
                    return OracleSettings.Instance.LevelThaumaturge;
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
                    return OracleSettings.Instance.LevelGladiator;
                case ClassJobType.Monk:
                    return OracleSettings.Instance.LevelPugilist;
                case ClassJobType.Warrior:
                    return OracleSettings.Instance.LevelMarauder;
                case ClassJobType.Dragoon:
                    return OracleSettings.Instance.LevelLancer;
                case ClassJobType.Bard:
                    return OracleSettings.Instance.LevelArcher;
                case ClassJobType.WhiteMage:
                    return OracleSettings.Instance.LevelConjurer;
                case ClassJobType.BlackMage:
                    return OracleSettings.Instance.LevelThaumaturge;
                case ClassJobType.Arcanist:
                    return OracleSettings.Instance.LevelArcanist;
                case ClassJobType.Summoner:
                    return OracleSettings.Instance.LevelArcanist;
                case ClassJobType.Scholar:
                    return OracleSettings.Instance.LevelArcanist;
                case ClassJobType.Rogue:
                    return OracleSettings.Instance.LevelRogue;
                case ClassJobType.Ninja:
                    return OracleSettings.Instance.LevelRogue;
                case ClassJobType.Machinist:
                    return OracleSettings.Instance.LevelMachinist;
                case ClassJobType.DarkKnight:
                    return OracleSettings.Instance.LevelDarkKnight;
                case ClassJobType.Astrologian:
                    return OracleSettings.Instance.LevelAstrologian;
                case ClassJobType.Samurai:
                    return OracleSettings.Instance.LevelSamurai;
                case ClassJobType.RedMage:
                    return OracleSettings.Instance.LevelRedMage;
                default:
                    throw new ArgumentOutOfRangeException(nameof(job), job, null);
            }
        }

        internal static bool IsCombatClassJob(ClassJobType job)
        {
            return CombatClassJobs.Contains(job);
        }

        internal static bool IsDpsClassJob(ClassJobType job)
        {
            return DpsClassJobs.Contains(job);
        }

        internal static bool IsHealerClassJob(ClassJobType job)
        {
            return HealerClassJobs.Contains(job);
        }

        internal static bool IsMeleeDpsClassJob(ClassJobType job)
        {
            return MeleeDpsClassJobs.Contains(job);
        }

        internal static bool IsOnClassChangeCooldown()
        {
            return ClassChangedTimer != null && ClassChangedTimer.Elapsed < TimeSpan.FromSeconds(30);
        }

        internal static bool IsRangedDpsClassJob(ClassJobType job)
        {
            return RangedDpsClassJobs.Contains(job);
        }

        internal static bool IsTankClassJob(ClassJobType job)
        {
            return TankClassJobs.Contains(job);
        }
    }
}