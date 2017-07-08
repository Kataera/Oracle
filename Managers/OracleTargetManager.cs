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
    internal class OracleTargetManager
    {
        private static Stopwatch targetThrottleTimer;

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

        public static async Task<BattleCharacter> SelectGrindTarget()
        {
            if (OracleClassManager.ClassChangedTimer != null && OracleClassManager.ClassChangedTimer.Elapsed < TimeSpan.FromSeconds(30))
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

            targetThrottleTimer = Stopwatch.StartNew();
            return viableTargets.OrderBy(order => order.Value).FirstOrDefault().Key;
        }
    }
}