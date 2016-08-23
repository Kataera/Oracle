using System.Threading.Tasks;

using ff14bot.Enums;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.FateTask
{
    internal static class BossFate
    {
        internal static async Task<bool> HandleBossFate()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null || currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
            {
                OracleFateManager.ClearCurrentFate("Current FATE is finished.");
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && currentFate.Progress < FateSettings.Instance.BossEngagePercentage
                && !OracleFateManager.PreviousFateChained())
            {
                if (!FateSettings.Instance.WaitAtBossForProgress)
                {
                    await OracleFateManager.ClearCurrentFate("Current FATE progress reset below minimum level.", false);
                }
                else
                {
                    Logger.SendLog("Current FATE progress is too low, waiting for it to reach " + FateSettings.Instance.BossEngagePercentage + "%.");
                }

                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && OracleCombatManager.AnyViableFateTargets())
            {
                OracleCombatManager.SelectFateTarget();
            }

            return true;
        }
    }
}