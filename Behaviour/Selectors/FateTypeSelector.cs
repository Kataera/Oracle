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

    internal sealed class FateTypeSelector
    {
        private static readonly object SyncRootObject = new object();

        private static volatile FateTypeSelector instance;

        private FateTypeSelector() {}

        public static FateTypeSelector Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRootObject)
                    {
                        if (instance == null)
                        {
                            instance = new FateTypeSelector();
                            instance.CreateBehaviour();
                        }
                    }
                }

                return instance;
            }
        }

        public Composite Behaviour { get; private set; }

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

        // TODO: Consider changing to switch composite.
        private void CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new Decorator(check => IsKillFate(), KillFate.Instance.Coroutine),
                new Decorator(check => IsCollectFate(), CollectFate.Instance.Coroutine),
                new Decorator(check => IsEscortFate(), EscortFate.Instance.Coroutine),
                new Decorator(check => IsDefenceFate(), DefenceFate.Instance.Coroutine),
                new Decorator(check => IsBossFate(), BossFate.Instance.Coroutine),
                new Decorator(check => IsMegaBossFate(), MegaBossFate.Instance.Coroutine)
            };
            this.Behaviour = new PrioritySelector(behaviours);
        }
    }
}