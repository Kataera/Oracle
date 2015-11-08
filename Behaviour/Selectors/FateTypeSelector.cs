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

namespace Tarot.Behaviour.Selectors
{
    using global::Tarot.Behaviour.Coroutines;

    using TreeSharp;

    internal static class FateTypeSelector
    {
        public static Composite Behaviour
        {
            get
            {
                return CreateBehaviour();
            }
        }

        private static bool IsKillFate()
        {
            // TODO: Implement.
            return true;
        }

        private static bool IsCollectFate()
        {
            // TODO: Implement.
            return true;
        }

        private static bool IsEscortFate()
        {
            // TODO: Implement.
            return true;
        }

        private static bool IsDefenceFate()
        {
            // TODO: Implement.
            return true;
        }

        private static bool IsBossFate()
        {
            // TODO: Implement.
            return true;
        }

        private static bool IsMegaBossFate()
        {
            // TODO: Implement.
            return true;
        }

        private static Composite CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new Decorator(check => IsKillFate(), KillFate.Coroutine),
                new Decorator(check => IsCollectFate(), CollectFate.Coroutine),
                new Decorator(check => IsEscortFate(), EscortFate.Coroutine),
                new Decorator(check => IsDefenceFate(), DefenceFate.Coroutine),
                new Decorator(check => IsBossFate(), BossFate.Coroutine),
                new Decorator(check => IsMegaBossFate(), MegaBossFate.Coroutine)
            };
            return new PrioritySelector(behaviours);
        }
    }
}