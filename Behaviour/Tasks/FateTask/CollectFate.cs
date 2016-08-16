using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.FateTask
{
    internal static class CollectFate
    {
        private static bool AnyViableTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableTarget).Any();
        }

        private static async Task ClearFate()
        {
            // Band-aid fix to stop a bug where the bot waits after turning in last items when FATE ends.
            // TODO: Look into why this is happening and fix properly.
            await OracleFateManager.DesyncLevel();

            await OracleFateManager.ClearCurrentFate("Current FATE is ending or is finished.");
        }

        public static async Task<bool> HandleCollectFate()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
            var oracleFate = OracleFateManager.FateDatabase.GetFateFromId(OracleFateManager.CurrentFateId);
            var fateItemCount = ConditionParser.ItemCount(oracleFate.ItemId);

            if (currentFate == null)
            {
                await ClearFate();
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE)
            {
                if (GameObjectManager.GetObjectByNPCId(oracleFate.NpcId) != null)
                {
                    if (Core.Player.InCombat)
                    {
                        return false;
                    }

                    if (fateItemCount >= FateSettings.Instance.CollectTurnInAmount)
                    {
                        Logger.SendLog("Turning in what we've collected.");
                        await TurnInFateItems(GameObjectManager.GetObjectByNPCId(oracleFate.NpcId));
                    }
                }
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && currentFate.TimeLeft < TimeSpan.FromMinutes(1))
            {
                if (Core.Player.InCombat)
                {
                    return false;
                }

                if (fateItemCount >= 1)
                {
                    Logger.SendLog("FATE is ending, turning in remaining items.");
                    await TurnInFateItems(GameObjectManager.GetObjectByNPCId(oracleFate.NpcId));

                    if (fateItemCount >= 1)
                    {
                        return false;
                    }
                }

                await ClearFate();
                return true;
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && AnyViableTargets())
            {
                SelectTarget();
            }

            return true;
        }

        private static bool IsViableTarget(BattleCharacter target)
        {
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == OracleFateManager.CurrentFateId;
        }

        private static async Task<bool> MoveToTurnInNpc(GameObject turnInNpc)
        {
            Logger.SendLog("Moving to interact with " + turnInNpc.Name + ".");

            while (Core.Player.Distance2D(turnInNpc.Location) > 3f)
            {
                if (!Core.Player.IsMounted && OracleMovementManager.IsMountNeeded(Core.Player.Location.Distance(turnInNpc.Location))
                    && Actionmanager.AvailableMounts.Any())
                {
                    Navigator.Stop();
                    if (!Core.Player.InCombat)
                    {
                        await OracleMovementManager.MountUp();
                    }
                }

                Navigator.MoveTo(turnInNpc.Location, "Moving to NPC.");
                await Coroutine.Yield();
            }

            Navigator.Stop();
            return true;
        }

        private static void SelectTarget()
        {
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
                target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
            }

            if (target != null)
            {
                Poi.Current = new Poi(target, PoiType.Kill);
            }
        }

        private static async Task<bool> TurnInFateItems(GameObject turnInNpc)
        {
            if (Core.Player.Distance2D(turnInNpc.Location) > 4f)
            {
                await MoveToTurnInNpc(turnInNpc);
            }

            if (GameObjectManager.Attackers.Any())
            {
                return false;
            }

            turnInNpc.Interact();
            await Coroutine.Sleep(MainSettings.Instance.ActionDelay);
            var result = await SkipDialogue.Main() && await TurnInItem.Main() && await SkipDialogue.Main();

            if (result)
            {
                Logger.SendLog("Items have been handed over to " + turnInNpc.Name + ".");
            }

            return result;
        }
    }
}