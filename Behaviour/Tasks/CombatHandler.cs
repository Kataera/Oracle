using System;
using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks
{
    internal static class CombatHandler
    {
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

                if (OracleSettings.Instance.FateWaitMode == FateWaitMode.GrindMobs)
                {
                    var target = await SelectGrindTarget.Main();
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

                if (!LevelSync.IsLevelSyncNeeded(fate))
                {
                    return true;
                }

                if (fate.Status != FateStatus.NOTACTIVE)
                {
                    if (fate.Within2D(Core.Player.Location))
                    {
                        await LevelSync.SyncLevel(fate);
                    }
                    else if (GameObjectManager.Attackers.Contains(Poi.Current.BattleCharacter))
                    {
                        await MoveToFate.MoveToCurrentFate(true);
                        await LevelSync.SyncLevel(fate);
                    }
                }
            }

            return true;
        }
    }
}