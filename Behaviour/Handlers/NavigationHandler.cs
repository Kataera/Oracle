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

    using Clio.Utilities;

    using ff14bot;
    using ff14bot.Behavior;
    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Helpers;

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

        private Composite MoveBehaviour;

        public Composite Behaviour { get; private set; }

        private static async Task<bool> CheckLocation()
        {
            Logger.SendDebugLog("Check location called.");

            // Get current FATE from main behaviour.
            var rbFate = MainBehaviour.Instance.CurrentRbFate;

            // Are we already inside the FATE area?
            if (rbFate != null)
            {
                return rbFate.Location.Distance2D(Core.Player.Location) <= rbFate.HotSpot.Radius * 0.95;
            }

            return false;
        }

        private static async Task<bool> HandleCustomWaypoints()
        {
            Logger.SendDebugLog("Custom waypoints called.");
            // TODO: Handle waypoints. For now always return false.
            return false;
        }

        private PrioritySelector CreateMoveToFateBehaviour()
        {
            // TODO: Finish.
            Composite[] behaviours =
            {
                new ActionRunCoroutine(coroutine => CheckLocation()),
                new ActionRunCoroutine(coroutine => HandleCustomWaypoints())
            };
            return new PrioritySelector();
        }

        private PrioritySelector CreateMoveToIdleBehaviour()
        {

            // TODO: Finish.
            Composite[] behaviours =
            {
                new ActionRunCoroutine(coroutine => CheckLocation()),
                new ActionRunCoroutine(coroutine => HandleCustomWaypoints())
            };
            return new PrioritySelector();
        }

        private void CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new ActionRunCoroutine(coroutine => CheckLocation()),
                new ActionRunCoroutine(coroutine => HandleCustomWaypoints()),
                new Decorator(check => Poi.Current.Type == PoiType.Fate, this.CreateMoveToFateBehaviour()),
                new Decorator(check => Poi.Current.Type == PoiType.None, this.CreateMoveToIdleBehaviour())
            };
            this.Behaviour = new PrioritySelector(behaviours);
        }
    }
}