using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleClassManager
    {
        public static Stopwatch ClassChangedTimer { get; set; }

        public static IEnumerable<ClassJobType> CombatClassJobs
        {
            get
            {
                var combatClassJobs = new List<ClassJobType>();
                combatClassJobs.AddRange(HealerClassJobs);
                combatClassJobs.AddRange(TankClassJobs);
                combatClassJobs.AddRange(DpsClassJobs);

                return combatClassJobs;
            }
        }

        public static IEnumerable<ClassJobType> DpsClassJobs
        {
            get
            {
                var dpsClassJobs = new List<ClassJobType>();
                dpsClassJobs.AddRange(MeleeDpsClassJobs);
                dpsClassJobs.AddRange(RangedDpsClassJobs);

                return dpsClassJobs;
            }
        }

        public static IEnumerable<ClassJobType> HealerClassJobs => new List<ClassJobType>
        {
            ClassJobType.Astrologian,
            ClassJobType.Conjurer,
            ClassJobType.Scholar,
            ClassJobType.WhiteMage
        };

        public static IEnumerable<ClassJobType> MeleeDpsClassJobs => new List<ClassJobType>
        {
            ClassJobType.Dragoon,
            ClassJobType.Lancer,
            ClassJobType.Monk,
            ClassJobType.Ninja,
            ClassJobType.Pugilist,
            ClassJobType.Rogue
        };

        public static IEnumerable<ClassJobType> RangedDpsClassJobs => new List<ClassJobType>
        {
            ClassJobType.Arcanist,
            ClassJobType.Archer,
            ClassJobType.Bard,
            ClassJobType.BlackMage,
            ClassJobType.Machinist,
            ClassJobType.Summoner,
            ClassJobType.Thaumaturge
        };

        public static IEnumerable<ClassJobType> TankClassJobs => new List<ClassJobType>
        {
            ClassJobType.DarkKnight,
            ClassJobType.Gladiator,
            ClassJobType.Marauder,
            ClassJobType.Paladin,
            ClassJobType.Warrior
        };

        public static bool ClassChangeNeeded()
        {
            if (ModeSettings.Instance.OracleOperationMode != OracleOperationMode.MultiLevelMode)
            {
                return false;
            }

            if (!IsClassJobEnabled(Core.Player.CurrentJob))
            {
                return true;
            }

            switch (ClassSettings.Instance.ClassLevelMode)
            {
                case ClassLevelMode.Concurrent:
                    return ConcurrentChangeNeeded();
                case ClassLevelMode.Sequential:
                    return SequentialChangeNeeded();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool ClassJobCanFarmAtma(ClassJobType classJob)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(classJob), classJob, null);
            }
        }

        private static bool ConcurrentChangeNeeded()
        {
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
                Logger.SendDebugLog("There are no classes that are lower level than our highest level class.");
                return false;
            }

            var lowerLevels = enabledLevels.Where(kvp => kvp.Value < highestLevel).ToArray();
            return lowerLevels.Any(kvp => kvp.Key != Core.Player.CurrentJob && kvp.Value < GetTrueLevel());
        }

        public static bool FinishedLevelling()
        {
            switch (ModeSettings.Instance.OracleOperationMode)
            {
                case OracleOperationMode.MultiLevelMode:
                    var enabledLevels = Core.Player.Levels.OrderBy(kvp => kvp.Value).Where(kvp => IsClassJobEnabled(kvp.Key)).ToArray();
                    return enabledLevels.All(kvp => kvp.Value >= ClassSettings.Instance.MaxLevel);
                case OracleOperationMode.LevelMode:
                    return GetTrueLevel() >= ClassSettings.Instance.MaxLevel;
                case OracleOperationMode.FateGrind:
                    Logger.SendWarningLog("Checking whether or not we've finished levelling in FATE grind mode.");
                    return false;
                case OracleOperationMode.SpecificFates:
                    Logger.SendWarningLog("Checking whether or not we've finished levelling in specific FATE mode.");
                    return false;
                case OracleOperationMode.AtmaGrind:
                    Logger.SendWarningLog("Checking whether or not we've finished levelling in Atma grind mode.");
                    return false;
                case OracleOperationMode.AnimusGrind:
                    Logger.SendWarningLog("Checking whether or not we've finished levelling in Animus grind mode.");
                    return false;
                case OracleOperationMode.AnimaGrind:
                    Logger.SendWarningLog("Checking whether or not we've finished levelling in Anima grind mode.");
                    return false;
                case OracleOperationMode.YokaiWatchGrind:
                    Logger.SendWarningLog("Checking whether or not we've finished levelling in Yo-kai Watch grind mode.");
                    return false;
                default:
                    Logger.SendWarningLog("Checking whether or not we've finished levelling in a non-levelling mode.");
                    return false;
            }
        }

        public static string GetClassJobName(ClassJobType job)
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

        public static ClassJobType GetLowestLevelClassJob()
        {
            var enabledLevels = Core.Player.Levels.OrderBy(kvp => kvp.Value).Where(kvp => IsClassJobEnabled(kvp.Key)).ToArray();
            return enabledLevels.FirstOrDefault().Key;
        }

        public static uint GetTrueLevel()
        {
            var baseClass = OracleFateManager.GetBaseClass(Core.Player.CurrentJob);
            var trueLevel = Core.Player.Levels.FirstOrDefault(kvp => kvp.Key == baseClass).Value;

            return trueLevel != 0 ? trueLevel : Core.Player.ClassLevel;
        }

        public static bool IsClassJobEnabled(ClassJobType job)
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

        public static bool IsCombatClassJob(ClassJobType job)
        {
            return CombatClassJobs.Contains(job);
        }

        public static bool IsDpsClassJob(ClassJobType job)
        {
            return DpsClassJobs.Contains(job);
        }

        public static bool IsHealerClassJob(ClassJobType job)
        {
            return HealerClassJobs.Contains(job);
        }

        public static bool IsTankClassJob(ClassJobType job)
        {
            return TankClassJobs.Contains(job);
        }

        public static bool NoClassesEnabled()
        {
            return !ClassSettings.Instance.LevelArcanist && !ClassSettings.Instance.LevelArcher && !ClassSettings.Instance.LevelAstrologian
                   && !ClassSettings.Instance.LevelConjurer && !ClassSettings.Instance.LevelDarkKnight && !ClassSettings.Instance.LevelGladiator
                   && !ClassSettings.Instance.LevelLancer && !ClassSettings.Instance.LevelMachinist && !ClassSettings.Instance.LevelMarauder
                   && !ClassSettings.Instance.LevelPugilist && !ClassSettings.Instance.LevelRogue && !ClassSettings.Instance.LevelThaumaturge;
        }

        private static bool SequentialChangeNeeded()
        {
            return GetTrueLevel() >= ClassSettings.Instance.MaxLevel;
        }

        public static bool ZoneChangeNeeded()
        {
            const ushort dravanianHinterlands = 399;

            if (ModeSettings.Instance.OracleOperationMode != OracleOperationMode.LevelMode
                && ModeSettings.Instance.OracleOperationMode != OracleOperationMode.MultiLevelMode)
            {
                return false;
            }

            if (Core.Player.IsLevelSynced || Core.Player.IsDead)
            {
                return false;
            }

            if (Poi.Current.Type == PoiType.Kill || Poi.Current.Type == PoiType.Fate || OracleFateManager.CurrentFateId != 0)
            {
                return false;
            }

            uint aetheryteId;
            MovementSettings.Instance.ZoneLevels.TryGetValue(GetTrueLevel(), out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                return false;
            }

            if (WorldManager.GetZoneForAetheryteId(aetheryteId) == WorldManager.ZoneId)
            {
                return false;
            }

            // Handle Idyllshire.
            if (aetheryteId == 75 && WorldManager.ZoneId == dravanianHinterlands)
            {
                return false;
            }

            return true;
        }
    }
}