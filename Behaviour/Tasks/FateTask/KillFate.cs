using System.Threading.Tasks;

using ff14bot.Enums;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.FateTask
{
    internal static class KillFate
    {
        private static async Task ClearFate()
        {
            await OracleFateManager.ClearCurrentFate("Current FATE is finished.");
        }

        internal static async Task<bool> HandleKillFate()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null || currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
            {
                await ClearFate();
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