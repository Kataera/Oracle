using ff14bot.Behavior;

using Oracle.Managers;

using TreeSharp;

namespace Oracle.Behaviour.PoiHooks
{
    internal static class SelectPoiType
    {
        public static Composite Behaviour
        {
            get { return CreateBehaviour(); }
        }

        private static Composite CreateBehaviour()
        {
            var setFatePoi = new ActionRunCoroutine(coroutine => SetFatePoi.Main());
            var setWaitPoi = new ActionRunCoroutine(coroutine => SetWaitPoi.Main());

            Composite[] composites =
            {
                new HookExecutor("SetDeathPoi"), new Decorator(check => !OracleFateManager.DeathFlag, new HookExecutor("SetCombatPoi")),
                new HookExecutor(
                    "SetFatePoi",
                    "A hook that selects a viable FATE based in user settings and assigns it as the Poi.",
                    setFatePoi),
                new HookExecutor(
                    "SetWaitPoi",
                    "A hook that sets the correct wait Poi based on user settings.",
                    setWaitPoi)
            };

            return new PrioritySelector(composites);
        }
    }
}