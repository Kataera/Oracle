using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class MoveToWaitLocation
    {
        internal static async Task<bool> HandleMoveToLocation()
        {
            if (Core.Player.Location.Distance(Poi.Current.Location) < 15f)
            {
                if (GameObjectManager.Attackers.Any(bc => !bc.IsFateGone) && (Poi.Current.Type != PoiType.Kill || Poi.Current.Type != PoiType.None))
                {
                    OracleFateManager.ClearPoi("We're being attacked.", false);
                }

                return true;
            }

            if (!OracleMovementManager.IsFlightMeshLoaded())
            {
                await OracleMovementManager.LoadFlightMeshIfAvailable();

                if (!OracleMovementManager.IsFlightMeshLoaded())
                {
                    await OracleMovementManager.NavigateToLocation(Poi.Current.Location, 2f, true);
                    return true;
                }
            }

            await OracleMovementManager.FlyToLocation(Poi.Current.Location, 2f, false, true);
            return true;
        }
    }
}