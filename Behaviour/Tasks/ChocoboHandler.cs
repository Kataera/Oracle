using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;

using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

using TreeSharp;

namespace Oracle.Behaviour.Tasks
{
    public class ChocoboHandler
    {
        public static Composite Behaviour => CreateBehaviour();

        private static string ChocoboName
        {
            get
            {
                OracleFateManager.ForceUpdateGameCache();
                if (!PartyManager.IsInParty)
                {
                    return "Chocobo";
                }

                var chocobo = PartyManager.AllMembers.FirstOrDefault(member => member.GameObject.SummonerGameObject == Core.Player);
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
                var summonResult = await SummonChocobo();
                if (summonResult == SummonChocoboResult.Success)
                {
                    Logger.SendLog(ChocoboName + " has been summoned successfully!");
                }

                return true;
            }

            if (Core.Player.CurrentHealthPercent < MainSettings.Instance.ChocoboHealerStanceThreshold)
            {
                await SetChocoboStance(CompanionStance.Healer);
            }
            else
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
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));
            Chocobo.Summon();
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay));

            return Chocobo.Summoned ? SummonChocoboResult.Success : SummonChocoboResult.Failure;
        }
    }

    public enum SummonChocoboResult
    {
        Success,

        Failure,

        Disabled
    }

    public enum SetChocoboStanceResult
    {
        Success,

        Failure
    }
}