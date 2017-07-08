using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.ServiceClient;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal class OracleCombatManager
    {
        private static Stopwatch checkForTargetCooldown;

        internal static bool AnyViableFateTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableFateTarget).Any();
        }

        internal static async Task<BattleCharacter> GetGrindTarget()
        {
            if (OracleClassManager.ClassChangedTimer != null && OracleClassManager.ClassChangedTimer.Elapsed < TimeSpan.FromSeconds(30))
            {
                Logger.SendLog("Waiting for class change skill cooldown to expire before selecting a target.");
                return null;
            }

            if (checkForTargetCooldown != null && checkForTargetCooldown.Elapsed <= TimeSpan.FromSeconds(5))
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
            var navTask = Navigator.NavigationProvider.CanFullyNavigateTo(navRequest, Core.Player.Location, WorldManager.ZoneId);
            var navResults = await Coroutine.ExternalTask(navTask);

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
                    var battleCharacter = targets.FirstOrDefault(target => target.ObjectId == result.Id);
                    if (battleCharacter != null)
                    {
                        viableTargets.Add(battleCharacter, result.PathLength);
                    }
                }

                await Coroutine.Yield();
            }

            checkForTargetCooldown = Stopwatch.StartNew();
            return viableTargets.OrderBy(order => order.Value).FirstOrDefault().Key;
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

            if (OracleClassManager.GetTrueLevel() - WaitSettings.Instance.MobGrindMinLevelBelow > battleCharacter.ClassLevel)
            {
                return false;
            }

            if (OracleClassManager.GetTrueLevel() + WaitSettings.Instance.MobGrindMaxLevelAbove < battleCharacter.ClassLevel)
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

            if (BlacklistSettings.Instance.BlacklistedMobs.Contains(battleCharacter.NpcId))
            {
                return false;
            }

            if (GameObjectManager.Attackers.Contains(battleCharacter))
            {
                return true;
            }

            return true;
        }

        private static bool IsNpcAttackingPlayer(BattleCharacter attacker)
        {
            if (!attacker.IsValid)
            {
                return false;
            }

            if (attacker.IsFateGone)
            {
                return false;
            }

            if (!attacker.HasTarget)
            {
                return false;
            }

            if (attacker.CurrentTargetId == Core.Player.ObjectId)
            {
                return true;
            }

            if (ChocoboManager.Object != null && ChocoboManager.Object.IsValid && attacker.CurrentTargetId == ChocoboManager.Object.ObjectId)
            {
                return true;
            }

            return false;
        }

        internal static bool IsPlayerBeingAttacked()
        {
            if (GameObjectManager.Attackers == null)
            {
                return Core.Player.InCombat;
            }

            return GameObjectManager.Attackers.Any(IsNpcAttackingPlayer);
        }

        internal static bool IsViableFateTarget(BattleCharacter target)
        {
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == OracleFateManager.CurrentFateId;
        }

        internal static void SelectFateTarget()
        {
            if (OracleClassManager.ClassChangedTimer != null && OracleClassManager.ClassChangedTimer.Elapsed < TimeSpan.FromSeconds(30))
            {
                Logger.SendLog("Waiting for class change skill cooldown to expire before selecting a target.");
                return;
            }

            var oracleFate = OracleFateManager.FateDatabase.GetFateFromId(OracleFateManager.CurrentFateId);
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
                target = GameObjectManager.Attackers.Any()
                             ? CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault()
                             : CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault(bc => bc.IsFate);
            }

            if (target != null)
            {
                Logger.SendLog("Selected " + target.Name + " (" + target.ObjectId.ToString("X") + ") as the next target to kill.");
                Poi.Current = new Poi(target, PoiType.Kill);
            }
        }

        internal static async Task<bool> SelectGrindTarget()
        {
            var target = await GetGrindTarget();
            if (target == null)
            {
                return false;
            }

            Logger.SendLog("Selecting " + target.Name + " (" + target.ObjectId.ToString("X") + ") as the next target to kill.", true);
            Poi.Current = new Poi(target, PoiType.Kill);
            return false;
        }
    }
}