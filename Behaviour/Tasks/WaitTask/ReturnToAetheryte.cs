﻿using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class ReturnToAetheryte
    {
        internal static async Task<bool> HandleReturnToAetheryte()
        {
            if (Core.Player.Location.Distance2D(Poi.Current.Location) < 15f)
            {
                if (OracleCombatManager.IsPlayerBeingAttacked() && Poi.Current.Type != PoiType.Kill && Poi.Current.Type != PoiType.None)
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