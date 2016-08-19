using System;
using System.Collections.Generic;
using System.Linq;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

using Oracle.Enumerations;
using Oracle.Managers;
using Oracle.Settings;
using Oracle.Structs;

namespace Oracle.Providers
{
    public class OracleCombatTargetingProvider : ITargetingProvider
    {
        public List<BattleCharacter> GetObjectsByWeight()
        {
            if (ReadyToTurnIn())
            {
                return new List<BattleCharacter>();
            }

            var allTargets = GameObjectManager.GetObjectsOfType<BattleCharacter>().ToArray();
            return allTargets.Where(bc => Filter(Core.Player.InCombat, bc)).OrderByDescending(GetWeight).ToList();
        }

        private static bool Filter(bool inCombat, BattleCharacter battleCharacter)
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
            var blacklistEntry = Blacklist.GetEntry(battleCharacter);

            if (!battleCharacter.IsValid || battleCharacter.IsDead || !battleCharacter.IsVisible || battleCharacter.CurrentHealthPercent <= 0f)
            {
                return false;
            }

            if (!battleCharacter.CanAttack)
            {
                return false;
            }

            if (blacklistEntry != null)
            {
                return false;
            }

            if (battleCharacter.IsFateGone)
            {
                return false;
            }

            if (IsLevelSyncNeeded(battleCharacter) && !FateManager.GetFateById(battleCharacter.FateId).Within2D(battleCharacter.Location))
            {
                return false;
            }

            if (GameObjectManager.Attackers.Contains(battleCharacter))
            {
                return true;
            }

            if (!battleCharacter.IsFate && currentFate != null)
            {
                return false;
            }

            if (currentFate != null && battleCharacter.FateId != currentFate.Id)
            {
                return false;
            }

            if (currentFate == null || !currentFate.IsValid)
            {
                return false;
            }

            return !inCombat;
        }

        private static double GetWeight(BattleCharacter battleCharacter)
        {
            var weight = 1800f;
            var currentFate = OracleFateManager.GetCurrentFateData();
            var oracleFate = new Fate();

            if (currentFate != null)
            {
                oracleFate = OracleFateManager.FateDatabase.GetFateFromFateData(currentFate);
            }

            // If FATE has a preferred target, prioritise it if we're out of combat.
            if (oracleFate.PreferredTargetId != null && oracleFate.PreferredTargetId.Contains(battleCharacter.NpcId) && !Core.Player.InCombat)
            {
                weight += 20000;
            }

            if (battleCharacter.Pointer == Core.Player.PrimaryTargetPtr)
            {
                weight += 150;
            }

            if (battleCharacter.HasTarget && battleCharacter.CurrentTargetId == Core.Player.ObjectId)
            {
                weight += 750;
            }

            if (Chocobo.Object != null && battleCharacter.HasTarget && battleCharacter.CurrentTargetId == Chocobo.Object.ObjectId)
            {
                weight += 400;
            }

            if (!battleCharacter.TappedByOther)
            {
                weight += 200;
            }

            if (battleCharacter.CurrentTargetId == Core.Player.ObjectId)
            {
                weight += 1000 / Convert.ToSingle(battleCharacter.CurrentHealth) * 3000;
            }

            if (!battleCharacter.InCombat)
            {
                weight += 130;
            }

            // Prefer nearer targets in combat if melee, and always out of combat.
            if (PlayerIsMelee() || !Core.Player.InCombat)
            {
                weight -= battleCharacter.Distance(Core.Player) * 30;
            }

            return weight;
        }

        private static bool IsLevelSyncNeeded(GameObject battleCharacter)
        {
            if (battleCharacter.FateId == 0)
            {
                return false;
            }

            return FateManager.GetFateById(battleCharacter.FateId).MaxLevel < OracleClassManager.GetTrueLevel();
        }

        private static bool PlayerIsMelee()
        {
            switch (Core.Player.CurrentJob)
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
                    return false;
                case ClassJobType.Conjurer:
                    return false;
                case ClassJobType.Thaumaturge:
                    return false;
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
                    return false;
                case ClassJobType.WhiteMage:
                    return false;
                case ClassJobType.BlackMage:
                    return false;
                case ClassJobType.Arcanist:
                    return false;
                case ClassJobType.Summoner:
                    return false;
                case ClassJobType.Scholar:
                    return false;
                case ClassJobType.Rogue:
                    return true;
                case ClassJobType.Ninja:
                    return true;
                case ClassJobType.Machinist:
                    return false;
                case ClassJobType.DarkKnight:
                    return true;
                case ClassJobType.Astrologian:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool ReadyToTurnIn()
        {
            if (Core.Player.InCombat)
            {
                return false;
            }

            var currentFate = OracleFateManager.GetCurrentFateData();
            if (currentFate == null)
            {
                return false;
            }

            var oracleFate = OracleFateManager.FateDatabase.GetFateFromId(currentFate.Id);
            if (oracleFate.Type != FateType.Collect)
            {
                return false;
            }

            if (currentFate.Status == FateStatus.NOTACTIVE)
            {
                return false;
            }

            var fateItemCount = ConditionParser.ItemCount(oracleFate.ItemId);
            return fateItemCount >= FateSettings.Instance.CollectTurnInAmount;
        }
    }
}