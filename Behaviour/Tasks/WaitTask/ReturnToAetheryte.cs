using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class ReturnToAetheryte
    {
        public static async Task<bool> Main()
        {
            if (Core.Player.Location.Distance2D(Poi.Current.Location) < 15f)
            {
                return true;
            }

            if (!OracleMovementManager.IsFlightMeshLoaded())
            {
                await OracleMovementManager.LoadFlightMeshIfAvailable();

                if (!OracleMovementManager.IsFlightMeshLoaded())
                {
                    await OracleMovementManager.NavigateToLocation(Poi.Current.Location, 15f);
                    return true;
                }
            }

            await OracleMovementManager.FlyToLocation(Poi.Current.Location, 15f, true);
            return true;
        }
    }
}