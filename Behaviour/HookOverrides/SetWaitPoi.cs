using System.Threading.Tasks;

using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.HookOverrides
{
    internal static class SetWaitPoi
    {
        internal static async Task<bool> Main()
        {
            if (CommonBehaviors.IsLoading)
            {
                return false;
            }

            if (OracleFateManager.IsFateSet())
            {
                return false;
            }

            switch (OracleSettings.Instance.IdleBehaviour)
            {
                case IdleBehaviour.Aetheryte:
                    await SetAetherytePoi();
                    break;
                case IdleBehaviour.Location:
                    await SetLocationPoi();
                    break;
                case IdleBehaviour.WaitInPlace:
                    await SetWaitInPlacePoi();
                    break;
                case IdleBehaviour.Grind:
                    await SetGrindPoi();
                    break;
                default:
                    Logger.SendErrorLog("Could not determine idle behaviour setting. Defaulting to returning to the nearest aetheryte.");
                    await SetAetherytePoi();
                    break;
            }

            return false;
        }

        private static async Task<bool> SetAetherytePoi()
        {
            var aetheryte = await OracleNavigationManager.GetClosestAetheryte(WorldManager.ZoneId);
            if (aetheryte != null)
            {
                Logger.SendDebugLog($"Closest Aetheryte is at {aetheryte.Item2}");

                Poi.Current = new Poi(aetheryte.Item2, PoiType.Wait);
                return true;
            }

            Logger.SendLog($"Couldn't find a reachable aetheryte for this zone, staying where we are.");
            return await SetWaitInPlacePoi();
        }

        private static async Task<bool> SetGrindPoi()
        {
            var target = await OracleTargetManager.SelectGrindTarget();
            if (target == null)
            {
                return false;
            }

            Logger.SendLog($"Selecting {target.Name} as the next target to kill.");
            Poi.Current = new Poi(target, PoiType.Kill);
            return true;
        }

        private static async Task<bool> SetLocationPoi()
        {
            Vector3 location;
            OracleSettings.Instance.IdleLocations.TryGetValue(WorldManager.ZoneId, out location);

            if (location != Vector3.Zero)
            {
                Logger.SendDebugLog($"Found location for zone ID {WorldManager.ZoneId} - {location}.");

                Poi.Current = new Poi(location, PoiType.Wait);
                return true;
            }

            Logger.SendDebugLog($"Could not find location for zone ID {WorldManager.ZoneId}, moving to closest aetheryte instead.");
            return await SetAetherytePoi();
        }

        private static async Task<bool> SetWaitInPlacePoi()
        {
            Poi.Current = new Poi(Core.Player.Location, PoiType.Wait);
            return true;
        }
    }
}