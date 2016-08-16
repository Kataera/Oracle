﻿using System.Threading.Tasks;

using ff14bot.Behavior;

using Oracle.Behaviour.Hooks.WaitSelect;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Hooks
{
    internal static class SetWaitPoi
    {
        public static async Task<bool> Main()
        {
            if (CommonBehaviors.IsLoading)
            {
                return false;
            }

            if (OracleClassManager.ZoneChangeNeeded())
            {
                return false;
            }

            OracleFateManager.DoNotWaitBeforeMovingFlag = false;
            switch (WaitSettings.Instance.FateWaitMode)
            {
                case FateWaitMode.ReturnToAetheryte:
                    await ReturnToAetheryte.Main();
                    break;

                case FateWaitMode.MoveToWaitLocation:
                    await MoveToWaitLocation.Main();
                    break;

                case FateWaitMode.GrindMobs:
                    await GrindMobs.Main();
                    break;

                case FateWaitMode.WaitInPlace:
                    await WaitInPlace.Main();
                    break;

                default:
                    Logger.SendDebugLog("Cannot determine idle strategy, defaulting to 'Return to Aetheryte'.");
                    await ReturnToAetheryte.Main();
                    break;
            }

            return false;
        }
    }
}