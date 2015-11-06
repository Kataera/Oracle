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
    using System.Threading.Tasks;

    using TreeSharp;

    internal sealed class NavigationSelector
    {
        private static readonly object SyncRootObject = new object();

        private static volatile NavigationSelector instance;

        private NavigationSelector() {}

        public static NavigationSelector Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRootObject)
                    {
                        if (instance == null)
                        {
                            instance = new NavigationSelector();
                            instance.CreateBehaviour();
                        }
                    }
                }

                return instance;
            }
        }

        public Composite Behaviour { get; private set; }

        private static async Task<bool> CheckNavigator()
        {
            // TODO: Write navigator check.
            return true;
        }

        private static bool IsFlightNeeded()
        {
            // TODO: Write Heavensward zone and flight setting check.
            return true;
        }

        private static void SetFlightNavigator()
        {
            // TODO: Set flight navigation.
        }

        private static void SetNormalNavigator()
        {
            // TODO: Set normal navigation.
        }

        private void CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new ActionRunCoroutine(coroutine => CheckNavigator()),
                new Decorator(
                    check => IsFlightNeeded(),
                    new Action(action => SetFlightNavigator())),
                new Action(action => SetNormalNavigator())
            };
            this.Behaviour = new PrioritySelector(behaviours);
        }
    }
}