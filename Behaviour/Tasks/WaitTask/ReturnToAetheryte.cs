using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class ReturnToAetheryte
    {
        public static async Task<bool> Main()
        {
            if (Core.Player.Location.Distance2D(Poi.Current.Location) < 15f)
            {
                if (GameObjectManager.Attackers.Any(bc => !bc.IsFateGone))
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
                    await OracleMovementManager.NavigateToLocation(Poi.Current.Location, 15f, true);
                    return true;
                }
            }

            await OracleMovementManager.FlyToLocation(Poi.Current.Location, 15f, true, true);
            return true;
        }
    }
}