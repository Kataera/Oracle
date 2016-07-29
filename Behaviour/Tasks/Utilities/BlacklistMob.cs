using System;
using System.Diagnostics;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class BlacklistMob
    {
        private static Stopwatch blacklistTimer;
        private static uint currentTargetId;
        private static uint lastHealthLevel;

        private static GameObject GetTargetFromId(uint objectId)
        {
            return GameObjectManager.GetObjectByObjectId(objectId);
        }

        public static async Task<bool> IsBlacklistNeeded()
        {
            if (blacklistTimer == null)
            {
                blacklistTimer = new Stopwatch();
            }

            if (Poi.Current.BattleCharacter == null || !Poi.Current.BattleCharacter.IsValid)
            {
                OracleFateManager.ClearPoi("Target is no longer valid.", false);
                Core.Player.ClearTarget();
                return true;
            }

            if (currentTargetId == 0 || currentTargetId != Poi.Current.BattleCharacter.ObjectId)
            {
                currentTargetId = Poi.Current.BattleCharacter.ObjectId;
                lastHealthLevel = GetTargetFromId(currentTargetId).CurrentHealth;
                blacklistTimer.Restart();

                return false;
            }

            if (GetTargetFromId(currentTargetId).CurrentHealth < lastHealthLevel)
            {
                lastHealthLevel = GetTargetFromId(currentTargetId).CurrentHealth;
                blacklistTimer.Restart();

                return false;
            }

            if (blacklistTimer.Elapsed >= TimeSpan.FromSeconds(30))
            {
                var target = GetTargetFromId(currentTargetId);

                if (target == null)
                {
                    return true;
                }

                var blacklistEntry = Blacklist.GetEntry(target);

                if (blacklistEntry != null)
                {
                    return true;
                }

                Logger.SendLog("Current target's HP has not decreased in 30 seconds, blacklisting and moving on.");
                Blacklist.Add(target, BlacklistFlags.Combat, TimeSpan.FromMinutes(15), "Target's HP has not changed in too long.");
                OracleFateManager.ClearPoi("Target's HP has not changed in too long.");
                Core.Player.ClearTarget();
                blacklistTimer.Restart();
            }

            return true;
        }
    }
}