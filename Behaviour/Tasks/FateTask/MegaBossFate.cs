﻿using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.FateTask
{
    internal static class MegaBossFate
    {
        public static async Task<bool> Main()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null || currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
            {
                await ClearFate();
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && currentFate.Progress < OracleSettings.Instance.MegaBossEngagePercentage)
            {
                if (!OracleSettings.Instance.WaitAtMegaBossFateForProgress)
                {
                    await OracleFateManager.ClearCurrentFate("Current FATE progress reset below minimum level.", false);
                }
                else
                {
                    Logger.SendLog(
                        "Current FATE progress is too low, waiting for it to reach "
                        + OracleSettings.Instance.MegaBossEngagePercentage + "%.");
                }

                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && AnyViableTargets())
            {
                SelectTarget();
            }

            return true;
        }

        private static bool AnyViableTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableTarget).Any();
        }

        private static async Task ClearFate()
        {
            await OracleFateManager.ClearCurrentFate("Current FATE is finished.");
        }

        private static bool IsViableTarget(BattleCharacter target)
        {
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == OracleFateManager.CurrentFateId;
        }

        private static void SelectTarget()
        {
            var oracleFate = OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.CurrentFateId);
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