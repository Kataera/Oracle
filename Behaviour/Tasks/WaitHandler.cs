using System.Threading.Tasks;

using ff14bot;

using Oracle.Behaviour.Tasks.WaitTask;
using Oracle.Enumerations;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks
{
    internal static class WaitHandler
    {
        public static async Task<bool> HandleWait()
        {
            if (OracleFateManager.IsPlayerBeingAttacked() && !Core.Player.IsMounted)
            {
                OracleFateManager.ClearPoi("We're being attacked.", false);
                return true;
            }

            if (await OracleFateManager.AnyViableFates())
            {
                OracleFateManager.ClearPoi("Viable FATE detected.");
                return true;
            }

            return await RunWait();
        }

        private static async Task<bool> RunWait()
        {
            switch (WaitSettings.Instance.FateWaitMode)
            {
                case FateWaitMode.ReturnToAetheryte:
                    await ReturnToAetheryte.Main();
                    return true;
                case FateWaitMode.MoveToWaitLocation:
                    await MoveToWaitLocation.Main();
                    return true;
                case FateWaitMode.GrindMobs:
                    await GrindMobs.Main();
                    return true;
                case FateWaitMode.WaitInPlace:
                    await WaitInPlace.Main();
                    return true;
            }

            return false;
        }
    }
}