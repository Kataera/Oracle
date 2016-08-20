using System;
using System.Linq;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

using Oracle.Helpers;

namespace Oracle.Managers
{
    public class OracleCombatManager
    {
        internal static bool AnyViableFateTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableFateTarget).Any();
        }

        internal static bool IsViableFateTarget(BattleCharacter target)
        {
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == OracleFateManager.CurrentFateId;
        }

        internal static void SelectFateTarget()
        {
            if (OracleClassManager.ClassChangedTimer != null && OracleClassManager.ClassChangedTimer.Elapsed < TimeSpan.FromSeconds(30))
            {
                Logger.SendLog("Waiting for class change skill cooldown to expire before selecting a target.");
                return;
            }

            var oracleFate = OracleFateManager.FateDatabase.GetFateFromId(OracleFateManager.CurrentFateId);
            BattleCharacter target = null;

            if (oracleFate.PreferredTargetId.Any())
            {
                var targets = GameObjectManager.GetObjectsByNPCIds<BattleCharacter>(oracleFate.PreferredTargetId.ToArray());
                target = targets.OrderBy(bc => bc.Distance(Core.Player)).FirstOrDefault(bc => bc.IsValid && bc.IsAlive);

                if (target == null)
                {
                    Logger.SendDebugLog("Could not find any mobs with the preferred targets' NPC id.");
                }
                else
                {
                    Logger.SendDebugLog("Found preferred target '" + target.Name + "' (" + target.NpcId + ").");
                }
            }

            if (target == null)
            {
                target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
            }

            if (target != null)
            {
                Poi.Current = new Poi(target, PoiType.Kill);
            }
        }
    }
}