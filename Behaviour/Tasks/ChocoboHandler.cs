using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;

using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

using TreeSharp;

namespace Oracle.Behaviour.Tasks
{
    internal class ChocoboHandler
    {
        internal static Composite Behaviour => CreateBehaviour();

        private static string ChocoboName
        {
            get
            {
                OracleFateManager.ForceUpdateGameCache();
                if (!PartyManager.IsInParty)
                {
                    return "Chocobo";
                }

                if (PartyManager.AllMembers == null)
                {
                    return "Chocobo";
                }

                PartyMember chocobo = null;
                foreach (var member in PartyManager.AllMembers)
                {
                    if (member.GameObject != null && member.GameObject.SummonerGameObject == Core.Player)
                    {
                        chocobo = member;
                    }
                }

                return chocobo == null ? "Chocobo" : chocobo.Name;
            }
        }

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => HandleChocobo());
        }

        internal static async Task<bool> HandleChocobo()
        {
            if (!MainSettings.Instance.ChocoboHandlingEnabled)
            {
                return false;
            }

            if (ModeSettings.Instance.OracleOperationMode == OracleOperationMode.YokaiWatchGrind)
            {
                return false;
            }

            if (Chocobo.BlockSummon)
            {
                return false;
            }

            if (Core.Player.IsMounted)
            {
                return false;
            }

            if (!Chocobo.Summoned && Chocobo.CanSummon)
            {
                // Check for whether or not the dead/dismissed/expired Chocobo is still in the party. If it is, game won't let us summon.
                if (PartyManager.IsInParty && PartyManager.AllMembers.Any(member => member.GameObject.SummonerGameObject == Core.Player))
                {
                    return false;
                }

                var summonResult = await SummonChocobo();
                if (summonResult == SummonChocoboResult.Success)
                {
                    Logger.SendLog(ChocoboName + " has been summoned successfully!");
                }

                return true;
            }

            // Safety checks for when the Chocobo may be summoned, but can't be accessed by RebornBuddy.
            if (!Chocobo.Summoned || Chocobo.Object == null || !Chocobo.Object.IsValid)
            {
                OracleFateManager.ForceUpdateGameCache();
                return false;
            }

            if (Core.Player.CurrentHealthPercent < MainSettings.Instance.ChocoboStancePlayerHealthThreshold)
            {
                await SetChocoboStance(CompanionStance.Healer);
            }
            else if (Chocobo.Object.CurrentHealthPercent < MainSettings.Instance.ChocoboStanceChocoboHealthThreshold)
            {
                await SetChocoboStance(CompanionStance.Healer);
            }
            else if (Core.Player.CurrentHealthPercent >= MainSettings.Instance.ChocoboStanceReturnToAttackThreshold)
            {
                await SetChocoboStance(CompanionStance.Attacker);
            }
            else if (Chocobo.Stance != CompanionStance.Attacker && Chocobo.Stance != CompanionStance.Healer)
            {
                await SetChocoboStance(CompanionStance.Attacker);
            }

            return false;
        }

        private static async Task<SetChocoboStanceResult> SetChocoboStance(CompanionStance stance)
        {
            if (Chocobo.Stance == stance)
            {
                return SetChocoboStanceResult.Success;
            }

            switch (stance)
            {
                case CompanionStance.Follow:
                    Logger.SendLog("Switching " + ChocoboName + " to follow stance.");
                    Chocobo.Follow();
                    break;
                case CompanionStance.Free:
                    Logger.SendLog("Switching " + ChocoboName + " to free stance.");
                    Chocobo.FreeStance();
                    break;
                case CompanionStance.Defender:
                    Logger.SendLog("Switching " + ChocoboName + " to defender stance.");
                    Chocobo.DefenderStance();
                    break;
                case CompanionStance.Attacker:
                    Logger.SendLog("Switching " + ChocoboName + " to attacker stance.");
                    Chocobo.AttackerStance();
                    break;
                case CompanionStance.Healer:
                    Logger.SendLog("Switching " + ChocoboName + " to healer stance.");
                    Chocobo.HealerStance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stance), stance, null);
            }

            return Chocobo.Stance == stance ? SetChocoboStanceResult.Success : SetChocoboStanceResult.Failure;
        }

        private static async Task<SummonChocoboResult> SummonChocobo()
        {
            if (Chocobo.BlockSummon)
            {
                return SummonChocoboResult.Disabled;
            }

            Logger.SendLog("Attempting to summon our Chocobo.");
            Navigator.Clear();
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));
            Chocobo.Summon();

            if (!Core.Player.InCombat)
            {
                await Coroutine.Wait(TimeSpan.FromSeconds(3), () => Chocobo.Summoned);
            }

            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));
            return Chocobo.Summoned ? SummonChocoboResult.Success : SummonChocoboResult.Failure;
        }
    }

    internal enum SummonChocoboResult
    {
        Success,

        Failure,

        Disabled
    }

    internal enum SetChocoboStanceResult
    {
        Success,

        Failure
    }
}