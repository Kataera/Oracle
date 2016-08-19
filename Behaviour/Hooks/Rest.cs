using System;
using System.Threading.Tasks;

using Buddy.Coroutines;

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
        public static Composite Behaviour => CreateBehaviour();

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        internal static async Task<bool> Main()
        {
            if (Core.Player.CurrentHealthPercent >= MainSettings.Instance.RestHealthPercent)
            {
                return false;
            }

            if (OracleClassManager.IsTankClassJob(Core.Player.CurrentJob) || OracleClassManager.IsMeleeDpsClassJob(Core.Player.CurrentJob)
                || OracleClassManager.IsRangedDpsClassJob(Core.Player.CurrentJob))
            {
                if (Core.Player.CurrentTPPercent >= MainSettings.Instance.RestTPManaPercent)
                {
                    return false;
                }
            }

            if (OracleClassManager.IsCasterClassJob(Core.Player.CurrentJob) || OracleClassManager.IsHealerClassJob(Core.Player.CurrentJob))
            {
                if (Core.Player.CurrentManaPercent >= MainSettings.Instance.RestTPManaPercent)
                {
                    return false;
                }
            }

            if (Poi.Current.Type != PoiType.Kill)
            {
                return false;
            }

            await RefreshObjectCache();
            if (!Poi.Current.BattleCharacter.IsValid || Poi.Current.BattleCharacter.IsDead)
            {
                OracleFateManager.ClearPoi("Mob is no longer valid.", false);
                return false;
            }

            if (MovementManager.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }

            Logger.SendLog("Resting until HP is over " + MainSettings.Instance.RestHealthPercent + "% and mana is over "
                           + MainSettings.Instance.RestTPManaPercent
                           + "%.");
            return true;
        }

        private static async Task RefreshObjectCache()
        {
            await Coroutine.Sleep(TimeSpan.FromMilliseconds(500));
            GameObjectManager.Clear();
            GameObjectManager.Update();
        }
    }
}