using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks
{
    internal static class CombatHandler
    {
        private static BattleCharacter mostRecentBc;
        private static uint lastHpValue;
        private static Stopwatch noDamageTimeout;

        public static async Task<bool> HandleCombat()
        {
            var currentBc = Poi.Current.BattleCharacter;

            if (currentBc == null)
            {
                return false;
            }

            if (!currentBc.IsValid)
            {
                OracleFateManager.ClearPoi("Targeted unit is not valid.", false);
                return true;
            }

            if (mostRecentBc == null || mostRecentBc != currentBc)
            {
                mostRecentBc = currentBc;
                lastHpValue = mostRecentBc.CurrentHealth;
                noDamageTimeout = Stopwatch.StartNew();
            }

            if (lastHpValue != mostRecentBc.CurrentHealth)
            {
                noDamageTimeout.Restart();
                lastHpValue = mostRecentBc.CurrentHealth;
            }

            if (ShouldBlacklistCombatPoi())
            {
                OracleFateManager.ClearPoi("Mob's HP has not changed in " + MainSettings.Instance.CombatNoDamageTimeout / 1000
                                           + " seconds, blacklisting and selecting a new mob.");
                Blacklist.Add(currentBc, BlacklistFlags.Combat, TimeSpan.FromMinutes(1), "No damage taken timeout triggered.");
                mostRecentBc = null;
                noDamageTimeout = null;

                Core.Player.ClearTarget();
                return true;
            }

            if (!currentBc.IsFate && !currentBc.IsDead && GameObjectManager.Attackers.All(mob => mob.ObjectId != currentBc.ObjectId)
                && OracleFateManager.CurrentFateId != 0)
            {
                OracleFateManager.ClearPoi("Targeted unit is not in combat with us, nor part of the current FATE.", false);
                return true;
            }

            // If target is not a FATE mob and is tapped by someone else.
            if (!currentBc.IsFate && currentBc.TappedByOther)
            {
                OracleFateManager.ClearPoi("Targeted unit is not a FATE mob and is tapped by someone else.");
                Blacklist.Add(currentBc, BlacklistFlags.Combat, TimeSpan.FromSeconds(30), "Tapped by another person.");
                Core.Player.ClearTarget();

                if (WaitSettings.Instance.FateWaitMode == FateWaitMode.GrindMobs)
                {
                    var target = await SelectGrindTarget.HandleSelectGrindTarget();
                    if (target == null)
                    {
                        return true;
                    }

                    Logger.SendLog("Selecting '" + target.Name + "' as the next target to kill.");
                    Poi.Current = new Poi(target, PoiType.Kill);
                }

                return true;
            }

            // If target is a FATE mob, we need to handle several potential issues.
            if (currentBc.IsFate)
            {
                var fate = FateManager.GetFateById(Poi.Current.BattleCharacter.FateId);

                if (fate == null)
                {
                    return true;
                }

                if (!OracleFateManager.IsLevelSyncNeeded(fate))
                {
                    return true;
                }

                if (fate.Status != FateStatus.NOTACTIVE)
                {
                    if (fate.Within2D(Core.Player.Location))
                    {
                        await OracleFateManager.SyncLevel(fate);
                    }
                    else if (GameObjectManager.Attackers.Contains(Poi.Current.BattleCharacter))
                    {
                        await OracleMovementManager.MoveToCurrentFate(true);
                        await OracleFateManager.SyncLevel(fate);
                    }
                }
            }

            return true;
        }

        private static bool ShouldBlacklistCombatPoi()
        {
            var currentBc = Poi.Current.BattleCharacter;

            if (noDamageTimeout.Elapsed <= TimeSpan.FromMilliseconds(MainSettings.Instance.CombatNoDamageTimeout))
            {
                return false;
            }

            if (currentBc.CurrentTargetId == Core.Player.ObjectId)
            {
                return false;
            }

            if (!currentBc.IsValid)
            {
                return false;
            }

            if (currentBc.IsDead)
            {
                return false;
            }

            if (GameObjectManager.Attackers.Contains(currentBc))
            {
                return false;
            }

            if (OracleClassManager.ClassChangedTimer != null && OracleClassManager.ClassChangedTimer.Elapsed < TimeSpan.FromSeconds(30))
            {
                return false;
            }

            return true;
        }
    }
}