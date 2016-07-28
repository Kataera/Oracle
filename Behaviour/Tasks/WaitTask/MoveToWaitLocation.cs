using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class MoveToWaitLocation
    {
        public static async Task<bool> Main()
        {
            if (Core.Player.Location.Distance(Poi.Current.Location) < 15f)
            {
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