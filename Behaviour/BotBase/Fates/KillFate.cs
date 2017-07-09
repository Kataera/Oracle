using System.Threading.Tasks;

using ff14bot.Enums;
using ff14bot.Helpers;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.BotBase.Fates
{
    internal static class KillFate
    {
        internal static async Task<bool> Main()
        {
            if (!OracleFateManager.IsCurrentFateValid())
            {
                OracleFateManager.ClearFate();
                return true;
            }

            if (OracleFateManager.GameFateData.Status != FateStatus.NOTACTIVE && OracleTargetManager.AnyViableFateTargets())
            {
                var target = await OracleTargetManager.GetFateTarget();
                if (target != null)
                {
                    Logger.SendLog($"Selecting {target.Name} as the next target to kill.");
                    Poi.Current = new Poi(target, PoiType.Kill);
                }
            }

            return false;
        }
    }
}