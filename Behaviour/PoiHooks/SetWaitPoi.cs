using System.Threading.Tasks;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;

using Oracle.Behaviour.PoiHooks.WaitSelect;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.PoiHooks
{
    internal static class SetWaitPoi
    {
        public static async Task<bool> Main()
        {
            if (CommonBehaviors.IsLoading)
            {
                return false;
            }

            if (ZoneChangeNeeded())
            {
                return false;
            }

            OracleFateManager.DoNotWaitBeforeMovingFlag = false;
            switch (OracleSettings.Instance.FateWaitMode)
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

        private static bool ZoneChangeNeeded()
        {
            uint aetheryteId;
            OracleSettings.Instance.ZoneLevels.TryGetValue(Core.Player.ClassLevel, out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                return false;
            }

            if (WorldManager.GetZoneForAetheryteId(aetheryteId) == WorldManager.ZoneId)
            {
                return false;
            }

            return true;
        }
    }
}