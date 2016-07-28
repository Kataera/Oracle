using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
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

        public static async Task<bool> Main()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
            var oracleFate = OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.CurrentFateId);

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

            if (currentFate.Status != FateStatus.NOTACTIVE && AnyViableTargets())
            {
                SelectTarget();
            }
            else if (currentFate.Status != FateStatus.NOTACTIVE)
            {
                var escortNpc = GameObjectManager.GetObjectByNPCId(oracleFate.NpcId)
                                ?? GameObjectManager.GetObjectsOfType<BattleCharacter>().FirstOrDefault(IsEscortNpc);

                if (escortNpc == null)
                {
                    await MoveToFateCentre();
                    return true;
                }
                await MoveToNpc(escortNpc);
            }

            return true;
        }

        private static bool AnyViableTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableTarget).Any();
        }

        private static async Task ClearFate()
        {
            await OracleFateManager.ClearCurrentFate("Current FATE is finished.");
        }

        private static TimeSpan GetRandomTimeSpan()
        {
            var rng = new Random();
            return TimeSpan.FromMilliseconds(rng.Next(500, 1500));
        }

        private static bool IsEscortNpc(BattleCharacter battleCharacter)
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
            var oracleFate = OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.CurrentFateId);

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

        private static bool IsViableTarget(BattleCharacter target)
        {
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == OracleFateManager.CurrentFateId;
        }

        private static async Task<bool> MoveToFateCentre()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null)
            {
                await ClearFate();
                return true;
            }

            while (Core.Player.Distance2D(currentFate.Location) > currentFate.Radius * 0.2f)
            {
                if (currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
                {
                    Navigator.Stop();
                    await ClearFate();
                    return true;
                }

                Navigator.MoveToPointWithin(currentFate.Location, currentFate.Radius * 0.2f, "FATE centre");
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
            var xOffset = Convert.ToSingle(((2 * rng.NextDouble()) - 1.0) * radius);
            var zOffset = Convert.ToSingle(((2 * rng.NextDouble()) - 1.0) * Math.Sqrt(radiusSquared - (xOffset * xOffset)));
            var location = new Vector3(npc.Location.X + xOffset, npc.Location.Y, npc.Location.Z + zOffset);

            Logger.SendDebugLog("NPC Location: " + npc.Location + ", Moving to: " + location);
            var timeout = new Stopwatch();
            timeout.Start();

            while (Core.Player.Distance2D(location) > 1f)
            {
                var currentFate = OracleFateManager.GetCurrentFateData();

                if (timeout.Elapsed > TimeSpan.FromSeconds(10))
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

        private static void SelectTarget()
        {
            var oracleFate = OracleFateManager.OracleDatabase.GetFateFromId(OracleFateManager.CurrentFateId);
            BattleCharacter target = null;

            if (oracleFate.PreferredTargetId.Any())
            {
                var targets = GameObjectManager.GetObjectsByNPCIds<BattleCharacter>(oracleFate.PreferredTargetId.ToArray());
                target = targets.OrderBy(bc => bc.Distance(Core.Player)).FirstOrDefault(bc => bc.IsValid && bc.IsAlive);

                if (target == null)
                {
                    Logger.SendDebugLog("Could not find any mobs with the preferred targets' NPC id.");
                }
                else
                {
                    Logger.SendDebugLog("Found preferred target '" + target.Name + "' (" + target.NpcId + ").");
                }
            }

            if (target == null)
            {
                target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
            }

            if (target != null)
            {
                Poi.Current = new Poi(target, PoiType.Kill);
            }
        }
    }
}