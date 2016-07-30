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
        public static Composite Behaviour => CreateBehaviour();

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        private static void RefreshObjectCache()
        {
            GameObjectManager.Clear();
            GameObjectManager.Update();
        }

        internal static async Task<bool> Main()
        {
            if (Core.Player.CurrentHealthPercent >= MainSettings.Instance.RestHealthPercent
                && Core.Player.CurrentManaPercent >= MainSettings.Instance.RestManaPercent)
            {
                return false;
            }

            if (Poi.Current.Type != PoiType.Kill)
            {
                return false;
            }

            RefreshObjectCache();
            if (!Poi.Current.BattleCharacter.IsValid || Poi.Current.BattleCharacter.IsDead)
            {
                OracleFateManager.ClearPoi("Mob is no longer valid.", false);
                return false;
            }

            if (MovementManager.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }

            Logger.SendLog("Resting until HP is over " + MainSettings.Instance.RestHealthPercent + "% and mana is over " + MainSettings.Instance.RestManaPercent
                           + "%.");
            return true;
        }
    }
}