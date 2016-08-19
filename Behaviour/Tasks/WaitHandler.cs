using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Behaviour.Tasks.WaitTask;
using Oracle.Enumerations;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks
{
    internal static class WaitHandler
    {
        internal static async Task<bool> HandleWait()
        {
            if (OracleFateManager.IsPlayerBeingAttacked() && !Core.Player.IsMounted && (Poi.Current.Type != PoiType.Kill || Poi.Current.Type != PoiType.None))
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
                    await ReturnToAetheryte.HandleReturnToAetheryte();
                    return true;
                case FateWaitMode.MoveToWaitLocation:
                    await MoveToWaitLocation.HandleMoveToLocation();
                    return true;
                case FateWaitMode.GrindMobs:
                    await GrindMobs.HandleGrindMobs();
                    return true;
                case FateWaitMode.WaitInPlace:
                    await WaitInPlace.HandleWaitInPlace();
                    return true;
            }

            return false;
        }
    }
}