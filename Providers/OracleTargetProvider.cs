using System;
using System.Collections.Generic;
using System.Linq;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

using Oracle.Managers;

namespace Oracle.Providers
{
    public class OracleCombatTargetingProvider : ITargetingProvider
    {
        public List<BattleCharacter> GetObjectsByWeight()
        {
            var allTargets = GameObjectManager.GetObjectsOfType<BattleCharacter>();
            return allTargets.Where(Filter).OrderByDescending(GetWeight).ToList();
        }

        private static bool Filter(BattleCharacter battleCharacter)
        {
            var blacklistEntry = Blacklist.GetEntry(battleCharacter);

            if (!battleCharacter.IsValid || battleCharacter.IsDead || !battleCharacter.IsVisible)
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

            if (battleCharacter.HasTarget && battleCharacter.CurrentTargetId == Core.Player.ObjectId)
            {
                return true;
            }

            if (ChocoboManager.Object != null && battleCharacter.HasTarget && battleCharacter.CurrentTargetId == ChocoboManager.Object.ObjectId)
            {
                return true;
            }

            if (OracleFateManager.IsFateSet() && !battleCharacter.IsFate)
            {
                return false;
            }

            if (OracleFateManager.IsFateSet() && battleCharacter.FateId != OracleFateManager.GameFateData.Id)
            {
                return false;
            }

            return !Core.Player.InCombat;
        }

        private static double GetWeight(BattleCharacter battleCharacter)
        {
            var weight = 1800f;

            // If FATE has a preferred target, prioritise it if we're out of combat.
            if (OracleFateManager.CurrentFateHasPreferredTarget() && OracleFateManager.CurrentFate.PreferredTargetId == battleCharacter.NpcId
                && !Core.Player.InCombat)
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

            if (ChocoboManager.Object != null && battleCharacter.HasTarget && battleCharacter.CurrentTargetId == ChocoboManager.Object.ObjectId)
            {
                weight += 400;
            }

            // Prefer untapped mobs, avoid tapped non-FATE mobs.
            if (!battleCharacter.TappedByOther)
            {
                weight += 200;
            }
            else if (!OracleFateManager.IsFateSet())
            {
                weight -= 30000;
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
            if (OracleClassManager.IsMeleeDpsClassJob(Core.Player.CurrentJob) || OracleClassManager.IsTankClassJob(Core.Player.CurrentJob)
                || !Core.Player.InCombat)
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
    }
}