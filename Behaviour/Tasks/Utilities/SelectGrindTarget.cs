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

using NeoGaia.ConnectionHandler;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    public class SelectGrindTarget
    {
        private static Stopwatch checkForTargetCooldown;

        public static async Task<BattleCharacter> Main()
        {
            if (checkForTargetCooldown == null || checkForTargetCooldown.Elapsed > TimeSpan.FromSeconds(5))
            {
                var targets = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(MobFilter).OrderBy(bc => bc.Distance()).Take(8);

                if (!targets.Any())
                {
                    return null;
                }

                if (!WorldManager.CanFly)
                {
                    var navRequest = targets.Select(target => new CanFullyNavigateTarget {Id = target.ObjectId, Position = target.Location});
                    var navResults =
                        await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

                    var viableTargets = new Dictionary<BattleCharacter, float>();
                    foreach (var result in navResults)
                    {
                        if (result.CanNavigate == 0)
                        {
                            var blacklistEntry = Blacklist.GetEntry(result.Id);
                            if (blacklistEntry == null)
                            {
                                var mob = GameObjectManager.GetObjectByObjectId(result.Id);
                                Logger.SendDebugLog("Blacklisting " + mob.Name + " (" + mob.ObjectId.ToString("X")
                                                    + "). It can't be navigated to.");
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
                else
                {
                    var viableTargets = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                                                         .Where(MobFilter)
                                                         .ToDictionary(result => result, result => result.Distance());

                    checkForTargetCooldown = Stopwatch.StartNew();
                    return viableTargets.OrderBy(order => order.Value).FirstOrDefault().Key;
                }
            }

            return null;
        }

        private static bool MobFilter(BattleCharacter battleCharacter)
        {
            if (!battleCharacter.IsValid || battleCharacter.IsDead || !battleCharacter.IsVisible
                || battleCharacter.CurrentHealthPercent <= 0f)
            {
                return false;
            }

            if (battleCharacter.TappedByOther)
            {
                return false;
            }

            if (Core.Player.ClassLevel - OracleSettings.Instance.MobMinimumLevelBelow > battleCharacter.ClassLevel)
            {
                return false;
            }

            if (Core.Player.ClassLevel + OracleSettings.Instance.MobMaximumLevelAbove < battleCharacter.ClassLevel)
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