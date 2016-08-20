﻿using System.Threading.Tasks;

using ff14bot.Enums;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.FateTask
{
    internal static class MegaBossFate
    {
        private static async Task ClearFate()
        {
            await OracleFateManager.ClearCurrentFate("Current FATE is finished.");
        }

        internal static async Task<bool> HandleMegaBossFate()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null || currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
            {
                await ClearFate();
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && currentFate.Progress < FateSettings.Instance.MegaBossEngagePercentage
                && !OracleFateManager.PreviousFateChained())
            {
                if (!FateSettings.Instance.WaitAtMegaBossForProgress)
                {
                    await OracleFateManager.ClearCurrentFate("Current FATE progress reset below minimum level.", false);
                }
                else
                {
                    Logger.SendLog("Current FATE progress is too low, waiting for it to reach " + FateSettings.Instance.MegaBossEngagePercentage + "%.");
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