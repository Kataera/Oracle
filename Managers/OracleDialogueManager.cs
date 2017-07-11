using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleDialogueManager
    {
        internal static async Task<bool> HandOverFateItems()
        {
            if (Talk.DialogOpen || Talk.ConvoLock)
            {
                await SkipDialogue();
            }

            if (!Request.IsOpen)
            {
                return false;
            }

            if (GameObjectManager.Attackers.Any())
            {
                return false;
            }

            var itemId = OracleFateManager.CurrentFate.ItemId;
            var turnInBagSlot = OracleInventoryManager.GetBagSlotFromItemId(itemId);

            if (turnInBagSlot == null)
            {
                return false;
            }

            Logger.SendLog("Attempting to hand over " + turnInBagSlot.Count + " of the item '" + turnInBagSlot.Name + "'.");
            turnInBagSlot.Handover();

            await Coroutine.Sleep(500);
            if (!Request.IsOpen || !Request.HandOverButtonClickable)
            {
                Logger.SendErrorLog("Hand over failed.");
                Request.Cancel();

                return false;
            }

            Logger.SendDebugLog("Pressing 'Hand Over' button.");
            Request.HandOver();
            await Coroutine.Sleep(500);

            return true;
        }

        internal static async Task<bool> SkipDialogue()
        {
            if (!Talk.DialogOpen && !Talk.ConvoLock)
            {
                return false;
            }

            Logger.SendDebugLog("Skipping dialogue.");
            var timeout = Stopwatch.StartNew();

            while (Talk.DialogOpen && timeout.Elapsed < TimeSpan.FromSeconds(10))
            {
                if (GameObjectManager.Attackers.Any())
                {
                    return false;
                }

                if (!Core.Player.HasTarget)
                {
                    return false;
                }

                Talk.Next();
                await Coroutine.Yield();
            }

            await Coroutine.Sleep(500);
            return timeout.Elapsed <= TimeSpan.FromSeconds(10);
        }

        private static async Task StartFateInteraction(GameObject npc)
        {
            // Navigate to the NPC.
            await OracleNavigationManager.NavigateToLocation(npc.Location, 2f);

            // Interact with the NPC.
            await Coroutine.Sleep(500);
            npc.Interact();
            await Coroutine.Sleep(500);

            while (Talk.DialogOpen)
            {
                await SkipDialogue();
                await Coroutine.Sleep(500);
                await Coroutine.Yield();
            }

            // Click yes.
            SelectYesno.ClickYes();
            await Coroutine.Sleep(500);

            while (Talk.DialogOpen)
            {
                await SkipDialogue();
                await Coroutine.Sleep(500);
                await Coroutine.Yield();
            }
        }

        internal static async Task<bool> StartInactiveFate()
        {
            if (OracleFateManager.GameFateData == null)
            {
                return false;
            }

            if (OracleFateManager.GameFateData.Status != FateStatus.PREPARING)
            {
                return false;
            }

            Logger.SendLog($"\"{OracleFateManager.GameFateData.Name}\" is currently inactive, attempting to start it.");

            if (OracleFateManager.CurrentFate.StartNpc != 0)
            {
                var npc = GameObjectManager.GetObjectByNPCId(OracleFateManager.CurrentFate.StartNpc);
                if (npc == null)
                {
                    Logger.SendErrorLog("Could not find the FATE start NPC. Blacklisting and selecting another.");
                    Blacklist.Add(OracleFateManager.GameFateData.Id, TimeSpan.FromMinutes(5), "Could not find start NPC.");
                    OracleFateManager.ClearFate();

                    return false;
                }

                await StartFateInteraction(npc);

                if (OracleFateManager.GameFateData.Status == FateStatus.ACTIVE)
                {
                    return true;
                }
            }
            else
            {
                var npcs = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                                            .Where(b => b.IsFate && !b.CanAttack & !b.IsDead && b.FateId == OracleFateManager.CurrentFateId).ToList();
                if (npcs.FirstOrDefault() == null)
                {
                    Logger.SendErrorLog("Could not find the FATE start NPC. Blacklisting and selecting another.");
                    Blacklist.Add(OracleFateManager.GameFateData.Id, TimeSpan.FromMinutes(5), "Could not find start NPC.");
                    OracleFateManager.ClearFate();

                    return false;
                }

                foreach (var npc in npcs)
                {
                    await StartFateInteraction(npc);

                    if (OracleFateManager.GameFateData.Status == FateStatus.ACTIVE)
                    {
                        break;
                    }
                }
            }

            return true;
        }
    }
}