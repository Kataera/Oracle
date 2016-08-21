using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.FateTask
{
    internal static class EscortFate
    {
        private static TimeSpan? movementCooldown;
        private static Stopwatch movementTimer;

        private static async Task ClearFate()
        {
            await OracleFateManager.ClearCurrentFate("Current FATE is finished.");
        }

        private static TimeSpan GetRandomTimeSpan()
        {
            var rng = new Random();
            return TimeSpan.FromMilliseconds(rng.Next(500, 1500));
        }

        internal static async Task<bool> HandleEscortFate()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
            var oracleFate = OracleFateManager.FateDatabase.GetFateFromId(OracleFateManager.CurrentFateId);

            if (currentFate == null || currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
            {
                await ClearFate();
                return true;
            }

            if (movementTimer == null)
            {
                movementTimer = Stopwatch.StartNew();
            }

            if (movementCooldown == null)
            {
                movementCooldown = GetRandomTimeSpan();
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && OracleCombatManager.AnyViableFateTargets())
            {
                OracleCombatManager.SelectFateTarget();
            }
            else if (currentFate.Status != FateStatus.NOTACTIVE)
            {
                var escortNpc = GameObjectManager.GetObjectByNPCId(oracleFate.NpcId)
                                ?? GameObjectManager.GetObjectsOfType<BattleCharacter>().FirstOrDefault(IsEscortNpc);

                if (escortNpc == null || !currentFate.Within2D(Core.Player.Location))
                {
                    await MoveToFateCentre();
                    return true;
                }

                await MoveToNpc(escortNpc);
            }

            return true;
        }

        private static bool IsEscortNpc(BattleCharacter battleCharacter)
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
            var oracleFate = OracleFateManager.FateDatabase.GetFateFromId(OracleFateManager.CurrentFateId);

            if (oracleFate.NpcId == battleCharacter.NpcId)
            {
                return true;
            }

            if (!battleCharacter.IsFate)
            {
                return false;
            }

            if (battleCharacter.CanAttack)
            {
                return false;
            }

            if (battleCharacter.FateId != currentFate.Id)
            {
                return false;
            }

            if (!battleCharacter.IsVisible)
            {
                return false;
            }

            if (!battleCharacter.IsTargetable)
            {
                return false;
            }

            return true;
        }

        private static async Task<bool> MoveToFateCentre()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null)
            {
                await ClearFate();
                return true;
            }

            var cachedLocation = currentFate.Location;
            while (Core.Player.Distance2D(cachedLocation) > currentFate.Radius * 0.2f)
            {
                if (currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
                {
                    Navigator.Stop();
                    await ClearFate();
                    return true;
                }

                var distanceToFateBoundary = Core.Player.Location.Distance2D(cachedLocation) - currentFate.Radius;
                if (Actionmanager.CanMount == 0 && !Core.Player.IsMounted && OracleMovementManager.IsMountNeeded(distanceToFateBoundary) && Actionmanager.AvailableMounts.Any())
                {
                    Navigator.Stop();
                    if (Core.Player.InCombat)
                    {
                        return true;
                    }

                    await OracleMovementManager.MountUp();
                }

                // Throttle navigator path generation requests.
                if (cachedLocation.Distance2D(currentFate.Location) > 10)
                {
                    cachedLocation = currentFate.Location;
                }

                Navigator.MoveToPointWithin(cachedLocation, currentFate.Radius * 0.2f, "FATE centre");
                await Coroutine.Yield();
            }

            return true;
        }

        private static async Task<bool> MoveToNpc(GameObject npc)
        {
            if (movementTimer.Elapsed < movementCooldown)
            {
                return true;
            }

            if (!(Core.Player.Distance2D(npc.Location) > 7f))
            {
                return true;
            }

            // Find random point within 3 yards of NPC.
            var rng = new Random();
            const float radius = 3f;
            const float radiusSquared = radius * radius;
            var xOffset = Convert.ToSingle((2 * rng.NextDouble() - 1.0) * radius);
            var zOffset = Convert.ToSingle((2 * rng.NextDouble() - 1.0) * Math.Sqrt(radiusSquared - xOffset * xOffset));
            var location = new Vector3(npc.Location.X + xOffset, npc.Location.Y, npc.Location.Z + zOffset);

            Logger.SendDebugLog("NPC Location: " + npc.Location + ", Moving to: " + location);
            var timeout = new Stopwatch();
            timeout.Start();

            while (Core.Player.Distance2D(location) > 1f)
            {
                var currentFate = OracleFateManager.GetCurrentFateData();

                if (timeout.Elapsed > TimeSpan.FromSeconds(5))
                {
                    Navigator.PlayerMover.MoveStop();
                    timeout.Reset();

                    Logger.SendDebugLog("Timed out while attempting to move to random location, navigating back to FATE centre.");
                    await MoveToFateCentre();

                    return true;
                }

                if (currentFate == null || currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
                {
                    Navigator.PlayerMover.MoveStop();
                    timeout.Reset();
                    await ClearFate();

                    return true;
                }

                Navigator.PlayerMover.MoveTowards(location);
                await Coroutine.Yield();
            }

            Navigator.PlayerMover.MoveStop();
            movementTimer.Restart();
            movementCooldown = GetRandomTimeSpan();

            Logger.SendDebugLog("Waiting " + movementCooldown.Value.TotalMilliseconds + "ms before moving again.");
            return true;
        }
    }
}