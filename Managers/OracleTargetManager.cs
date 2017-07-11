﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.ServiceClient;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal class OracleTargetManager
    {
        private const uint ForlornMaidenNpcId = 6737;
        private const uint TheForlornNpcId = 6738;

        private static Stopwatch targetThrottleTimer;

        public static async Task<bool> AnyViableFateTargets()
        {
            var targets = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(FateMobFilter).OrderBy(bc => bc.Distance()).Take(8).ToList();
            if (!targets.Any())
            {
                return false;
            }

            var navRequest = targets.Select(target => new CanFullyNavigateTarget
            {
                Id = target.ObjectId,
                Position = target.Location
            }).ToList();
            var navResults = await OracleNavigationManager.AreLocationsNavigable(navRequest);

            return navResults.Any(result => result.CanNavigate == 1);
        }

        internal static bool FateMobFilter(BattleCharacter battleCharacter)
        {
            var blacklistEntry = Blacklist.GetEntry(battleCharacter);

            if (!battleCharacter.IsValid || battleCharacter.IsDead || !battleCharacter.IsVisible)
            {
                return false;
            }

            if (!battleCharacter.CanAttack)
            {
                return false;
            }

            if (blacklistEntry != null)
            {
                return false;
            }

            if (battleCharacter.IsFateGone)
            {
                return false;
            }

            if (OracleFateManager.IsLevelSyncNeeded(battleCharacter.FateId)
                && !FateManager.GetFateById(battleCharacter.FateId).Within2D(battleCharacter.Location))
            {
                return false;
            }

            if (GameObjectManager.Attackers.Contains(battleCharacter))
            {
                return true;
            }

            if (battleCharacter.HasTarget && battleCharacter.CurrentTargetId == Core.Player.ObjectId)
            {
                return true;
            }

            if (ChocoboManager.Object != null && battleCharacter.HasTarget && battleCharacter.CurrentTargetId == ChocoboManager.Object.ObjectId)
            {
                return true;
            }

            if (!battleCharacter.IsFate && OracleFateManager.GameFateData != null)
            {
                return false;
            }

            if (OracleFateManager.CurrentFateId != 0 && battleCharacter.FateId != OracleFateManager.CurrentFateId)
            {
                return false;
            }

            if (OracleFateManager.GameFateData != null && (OracleFateManager.CurrentFateId == 0 || !OracleFateManager.GameFateData.IsValid))
            {
                return false;
            }

            return !Core.Player.InCombat;
        }

        internal static BattleCharacter GetAttacker()
        {
            var attackers = GameObjectManager.Attackers;
            return attackers.All(a => a.IsFateGone || a.IsDead) ? null : attackers.FirstOrDefault(a => !a.IsFateGone && !a.IsDead);
        }

        internal static async Task<BattleCharacter> GetFateTarget()
        {
            if (OracleClassManager.IsOnClassChangeCooldown())
            {
                Logger.SendLog("Waiting for class change skill cooldown to expire before selecting a target.");
                return null;
            }

            if (targetThrottleTimer != null && targetThrottleTimer.Elapsed <= TimeSpan.FromSeconds(5))
            {
                return null;
            }

            var targets = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(FateMobFilter).OrderBy(bc => bc.Distance()).Take(8).ToList();
            if (!targets.Any())
            {
                return null;
            }

            var navRequest = targets.Select(target => new CanFullyNavigateTarget
            {
                Id = target.ObjectId,
                Position = target.Location
            }).ToList();
            var navResults = await OracleNavigationManager.AreLocationsNavigable(navRequest);

            var viableTargets = GetViableTargets(navResults);
            targetThrottleTimer = Stopwatch.StartNew();

            if (OracleSettings.Instance.PrioritiseTheForlorn && viableTargets.Keys.Any(bc => bc.NpcId == TheForlornNpcId))
            {
                Logger.SendLog("Detected The Forlorn, prioritising it.");
                return viableTargets.Keys.FirstOrDefault(bc => bc.NpcId == TheForlornNpcId);
            }

            if (OracleSettings.Instance.PrioritiseForlornMaiden && viableTargets.Keys.Any(bc => bc.NpcId == ForlornMaidenNpcId))
            {
                Logger.SendLog("Detected a Forlorn Maiden, prioritising it.");
                return viableTargets.Keys.FirstOrDefault(bc => bc.NpcId == ForlornMaidenNpcId);
            }

            return viableTargets.OrderBy(order => order.Value).FirstOrDefault().Key;
        }

        public static async Task<BattleCharacter> GetGrindTarget()
        {
            if (OracleClassManager.IsOnClassChangeCooldown())
            {
                Logger.SendLog("Waiting for class change skill cooldown to expire before selecting a target.");
                return null;
            }

            if (targetThrottleTimer != null && targetThrottleTimer.Elapsed <= TimeSpan.FromSeconds(5))
            {
                return null;
            }

            var targets = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(GrindMobFilter).OrderBy(bc => bc.Distance()).Take(8).ToList();
            if (!targets.Any())
            {
                return null;
            }

            var navRequest = targets.Select(target => new CanFullyNavigateTarget
            {
                Id = target.ObjectId,
                Position = target.Location
            }).ToList();
            var navResults = await OracleNavigationManager.AreLocationsNavigable(navRequest);

            var viableTargets = GetViableTargets(navResults);
            targetThrottleTimer = Stopwatch.StartNew();
            return viableTargets.OrderBy(order => order.Value).FirstOrDefault().Key;
        }

        private static Dictionary<BattleCharacter, float> GetViableTargets(IEnumerable<CanFullyNavigateResult> navResults)
        {
            var viableTargets = new Dictionary<BattleCharacter, float>();
            foreach (var result in navResults)
            {
                if (result.CanNavigate == 0)
                {
                    var blacklistEntry = Blacklist.GetEntry(result.Id);
                    if (blacklistEntry == null)
                    {
                        var mob = GameObjectManager.GetObjectByObjectId(result.Id);
                        Logger.SendDebugLog("Blacklisting " + mob.Name + " (" + mob.ObjectId.ToString("X") + "). It can't be navigated to.");
                        Blacklist.Add(mob, BlacklistFlags.Combat, TimeSpan.FromMinutes(15), "Can't navigate to mob.");
                    }
                }
                else
                {
                    var battleCharacter = GameObjectManager.GetObjectByObjectId(result.Id) as BattleCharacter;
                    if (battleCharacter != null)
                    {
                        viableTargets.Add(battleCharacter, result.PathLength);
                    }
                }
            }

            return viableTargets;
        }

        internal static bool GrindMobFilter(BattleCharacter battleCharacter)
        {
            if (!battleCharacter.IsValid || battleCharacter.IsDead || !battleCharacter.IsVisible || battleCharacter.CurrentHealthPercent <= 0f)
            {
                return false;
            }

            if (battleCharacter.TappedByOther)
            {
                return false;
            }

            if (OracleClassManager.GetTrueLevel() - OracleSettings.Instance.MobGrindMinLevel > battleCharacter.ClassLevel)
            {
                return false;
            }

            if (OracleClassManager.GetTrueLevel() + OracleSettings.Instance.MobGrindMaxLevel < battleCharacter.ClassLevel)
            {
                return false;
            }

            if (!battleCharacter.CanAttack)
            {
                return false;
            }

            if (Blacklist.Contains(battleCharacter.ObjectId, BlacklistFlags.Combat))
            {
                return false;
            }

            if (battleCharacter.IsFateGone)
            {
                return false;
            }

            if (battleCharacter.IsFate)
            {
                return false;
            }

            if (OracleSettings.Instance.BlacklistedMobs.Contains(battleCharacter.NpcId))
            {
                return false;
            }

            if (GameObjectManager.Attackers.Contains(battleCharacter))
            {
                return true;
            }

            return true;
        }
    }
}