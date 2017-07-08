using System;
using System.Diagnostics;
using System.Linq;

using ff14bot;
using ff14bot.Enums;

namespace Oracle.Managers
{
    internal static class OracleClassManager
    {
        internal static Stopwatch ClassChangedTimer { get; set; }

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

        internal static uint GetTrueLevel()
        {
            var baseClass = GetBaseClass(Core.Player.CurrentJob);
            var trueLevel = Core.Player.Levels.FirstOrDefault(kvp => kvp.Key == baseClass).Value;

            return trueLevel != 0 ? trueLevel : Core.Player.ClassLevel;
        }
    }
}