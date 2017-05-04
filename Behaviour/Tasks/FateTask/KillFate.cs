using System.Threading.Tasks;

using ff14bot.Enums;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.FateTask
{
    internal static class KillFate
    {
        internal static async Task<bool> HandleKillFate()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null || currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
            {
                OracleFateManager.ClearCurrentFate("Current FATE is finished.");
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