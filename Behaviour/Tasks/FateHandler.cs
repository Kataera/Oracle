using System;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks.FateTask;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks
{
    internal static class FateHandler
    {
        internal static async Task<bool> HandleFate()
        {
            var currentFate = OracleFateManager.GetCurrentFateData();

            if (currentFate == null)
            {
                Logger.SendDebugLog("Current FATE could not be found, assuming it's finished.");
                OracleFateManager.ClearCurrentFate("FATE is no longer active.");
                return false;
            }

            if (currentFate.Status == FateStatus.NOTACTIVE)
            {
                OracleFateManager.ClearCurrentFate("FATE is no longer active.");
                return false;
            }

            if (Core.Player.Distance(currentFate.Location) > currentFate.Radius * 1.05f)
            {
                await OracleMovementManager.MoveToCurrentFate(false);

                if (OracleFateManager.CurrentFateId == 0)
                {
                    return true;
                }
            }

            if (OracleCombatManager.IsPlayerBeingAttacked() && !Core.Player.IsMounted && Poi.Current.Type != PoiType.Kill && Poi.Current.Type != PoiType.None)
            {
                OracleFateManager.ClearPoi("We're being attacked.", false);
                return true;
            }

            if (OracleFateManager.IsLevelSyncNeeded(currentFate))
            {
                await OracleFateManager.SyncLevel(currentFate);
                return true;
            }

            return await RunFate();
        }

        private static async Task<bool> RunFate()
        {
            var oracleFate = OracleFateManager.FateDatabase.GetFateFromId(OracleFateManager.CurrentFateId);

            switch (oracleFate.Type)
            {
                case FateType.Kill:
                    await KillFate.HandleKillFate();
                    return true;
                case FateType.Collect:
                    await CollectFate.HandleCollectFate();
                    return true;
                case FateType.Escort:
                    await EscortFate.HandleEscortFate();
                    return true;
                case FateType.Defence:
                    await DefenceFate.HandleDefenceFate();
                    return true;
                case FateType.Boss:
                    await BossFate.HandleBossFate();
                    return true;
                case FateType.MegaBoss:
                    await MegaBossFate.HandleMegaBossFate();
                    return true;
                case FateType.Null:
                    Logger.SendWarningLog("Cannot find FATE in database, using Rebornbuddy's FATE type identifier.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var currentFate = OracleFateManager.GetCurrentFateData();
            if (currentFate == null)
            {
                OracleFateManager.ClearCurrentFate("Cannot determine FATE type and FateData is null");
                return true;
            }

            switch (currentFate.Icon)
            {
                case FateIconType.Battle:
                    await KillFate.HandleKillFate();
                    return true;
                case FateIconType.Boss:
                    Logger.SendWarningLog("Cannot determine if FATE is a regular or mega-boss, assuming regular.");
                    await BossFate.HandleBossFate();
                    return true;
                case FateIconType.KillHandIn:
                    await CollectFate.HandleCollectFate();
                    return true;
                case FateIconType.ProtectNPC:
                    await EscortFate.HandleEscortFate();
                    return true;
                case FateIconType.ProtectNPC2:
                    await DefenceFate.HandleDefenceFate();
                    return true;
                default:
                    Logger.SendDebugLog("Cannot determine FATE type, blacklisting until end of session.");
                    Blacklist.Add(currentFate.Id, BlacklistFlags.Node, TimeSpan.MaxValue, "Cannot determine FATE type.");
                    OracleFateManager.ClearCurrentFate("Cannot determine FATE type.");
                    return false;
            }
        }
    }
}