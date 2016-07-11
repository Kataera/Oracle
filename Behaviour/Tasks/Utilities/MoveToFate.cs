using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Managers;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class MoveToFate
    {
        public static async Task<bool> MoveToCurrentFate(bool ignoreCombat)
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (!ignoreCombat && GameObjectManager.Attackers.Any(attacker => attacker.IsValid) && !Core.Player.IsMounted)
            {
                return false;
            }

            await OracleMovementManager.LoadFlightMeshIfAvailable();

            if (!ignoreCombat && OracleSettings.Instance.TeleportIfQuicker && currentFate.IsValid)
            {
                if (await Teleport.FasterToTeleport(currentFate))
                {
                    await Coroutine.Wait(TimeSpan.FromSeconds(10), WorldManager.CanTeleport);
                    if (WorldManager.CanTeleport())
                    {
                        Logger.SendLog("Teleporting to the closest aetheryte crystal to the FATE.");
                        await Teleport.TeleportToClosestAetheryte(currentFate);

                        if (GameObjectManager.Attackers.Any(attacker => attacker.IsValid))
                        {
                            OracleFateManager.ClearPoi("We're under attack and can't teleport.");
                            return false;
                        }
                    }
                    else
                    {
                        Logger.SendErrorLog("Timed out trying to teleport, running to FATE instead.");
                    }
                }
            }

            var distanceToFateBoundary = Core.Player.Location.Distance2D(currentFate.Location) - currentFate.Radius;
            if (!ignoreCombat && OracleMovementManager.IsMountNeeded(distanceToFateBoundary) && !Core.Player.IsMounted
                && currentFate.IsValid)
            {
                await Mount.MountUp();
            }

            if (!ignoreCombat && WorldManager.CanFly && OracleMovementManager.ZoneFlightMesh != null)
            {
                await OracleMovementManager.FlyToCurrentFate();
            }
            else
            {
                await OracleMovementManager.NavigateToCurrentFate(ignoreCombat);
            }

            return true;
        }
    }
}