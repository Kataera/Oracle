using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

using TreeSharp;

namespace Oracle.Behaviour.Hooks
{
    internal static class Rest
    {
        internal static Composite Behaviour => CreateBehaviour();

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        internal static async Task<bool> Main()
        {
            if (Poi.Current.Type != PoiType.Kill)
            {
                return false;
            }

            OracleFateManager.ForceUpdateGameCache();
            if (!Poi.Current.BattleCharacter.IsValid || Poi.Current.BattleCharacter.IsDead)
            {
                OracleFateManager.ClearPoi("Mob is "
                                           + "no longer"
                                           + " valid.",
                                           false);
                return false;
            }

            if (Core.Player.CurrentHealthPercent < MainSettings.Instance.RestHealthPercent)
            {
                WaitForPlayerRegeneration();
                return true;
            }

            if (OracleClassManager.IsTankClassJob(Core.Player.CurrentJob) || OracleClassManager.IsMeleeDpsClassJob(Core.Player.CurrentJob)
                || OracleClassManager.IsRangedDpsClassJob(Core.Player.CurrentJob))
            {
                if (Core.Player.CurrentTPPercent < MainSettings.Instance.RestTPManaPercent)
                {
                    WaitForPlayerRegeneration();
                    return true;
                }
            }

            if (OracleClassManager.IsCasterClassJob(Core.Player.CurrentJob) || OracleClassManager.IsHealerClassJob(Core.Player.CurrentJob))
            {
                if (Core.Player.CurrentManaPercent < MainSettings.Instance.RestTPManaPercent)
                {
                    WaitForPlayerRegeneration();
                    return true;
                }
            }

            return false;
        }

        private static void WaitForPlayerRegeneration()
        {
            if (MovementManager.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }

            Logger.SendLog("Resting until HP is over " + MainSettings.Instance.RestHealthPercent + "% and mana/TP is over "
                           + MainSettings.Instance.RestTPManaPercent
                           + "%.");
        }
    }
}