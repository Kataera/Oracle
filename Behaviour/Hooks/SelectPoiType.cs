using ff14bot.Behavior;

using Oracle.Managers;

using TreeSharp;

namespace Oracle.Behaviour.Hooks
{
    internal static class SelectPoiType
    {
        internal static Composite Behaviour => CreateBehaviour();

        private static Composite CreateBehaviour()
        {
            var setCombatPoi = new ActionRunCoroutine(coroutine => SetCombatPoi.Main());
            var setFatePoi = new ActionRunCoroutine(coroutine => SetFatePoi.Main());
            var setWaitPoi = new ActionRunCoroutine(coroutine => SetWaitPoi.Main());

            Composite[] composites =
            {
                new HookExecutor("SetDeathPoi"),
                new HookExecutor("SetCombatPoi", "A hook to help defend the character when their Chocobo is under attack.", setCombatPoi),
                new Decorator(check => !OracleFateManager.PausePoiSetting,
                              new HookExecutor("SetFatePoi",
                                               "A hook that selects a viable FATE based in user settings and assigns it as the Poi.",
                                               setFatePoi)),
                new Decorator(check => !OracleFateManager.PausePoiSetting,
                              new HookExecutor("SetWaitPoi", "A hook that sets the correct wait Poi based on user settings.", setWaitPoi))
            };

            return new PrioritySelector(composites);
        }
    }
}