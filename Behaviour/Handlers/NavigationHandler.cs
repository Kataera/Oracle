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

namespace Tarot.Behaviour.Handlers
{
    using System.Threading.Tasks;

    using ff14bot;
    using ff14bot.Managers;

    using TreeSharp;

    internal sealed class NavigationHandler
    {
        private static readonly object SyncRootObject = new object();

        private static volatile NavigationHandler instance;

        private NavigationHandler() {}

        public static NavigationHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRootObject)
                    {
                        if (instance == null)
                        {
                            instance = new NavigationHandler();
                            instance.CreateBehaviour();
                        }
                    }
                }

                return instance;
            }
        }

        public Composite Behaviour { get; private set; }

        private static async Task<bool> CheckLocation()
        {
            // Get current FATE from main behaviour.
            var rbFate = MainBehaviour.Instance.CurrentRbFate;

            // Are we already inside the FATE area?
            return rbFate.Location.Distance2D(Core.Player.Location) <= rbFate.HotSpot.Radius * 0.95;
        }

        private static async Task<bool> HandleCustomWaypoints()
        {
            // TODO: Handle waypoints. For now always return false.
            return false;
        }

        private static bool IsFateNavigable()
        {
            // TODO: Write navigable check.
            return true;
        }

        private static void NavigateToFate()
        {
            // TODO: Write fate navigation logic.
        }

        private void CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new ActionRunCoroutine(coroutine => CheckLocation()),
                new ActionRunCoroutine(coroutine => HandleCustomWaypoints()),
                new Decorator(check => IsFateNavigable(), new Action(action => NavigateToFate()))
            };
            this.Behaviour = new PrioritySelector(behaviours);
        }
    }
}