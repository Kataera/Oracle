/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

namespace Tarot.Behaviour.Tasks.Hooks
{
    using ff14bot.Behavior;

    using global::Tarot.Behaviour.Tasks.Selectors;

    using TreeSharp;

    internal static class SelectPoiType
    {
        public static Composite Behaviour
        {
            get
            {
                return CreateBehaviour();
            }
        }

        private static Composite CreateBehaviour()
        {
            var setFatePoi = new ActionRunCoroutine(coroutine => FateSelector.Task());
            Composite[] composites =
            {
                new HookExecutor("SetDeathPoi"), new HookExecutor("SetCombatPoi"),
                new HookExecutor("SetFatePoi", "A hook that selects a viable FATE and assigns it as the Poi.", setFatePoi)
            };

            return new PrioritySelector(composites);
        }
    }
}