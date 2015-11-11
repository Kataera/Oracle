﻿/*
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

namespace Tarot.Behaviour
{
    using ff14bot.Managers;

    using global::Tarot.Behaviour.Tasks;
    using global::Tarot.Behaviour.Tasks.Handlers;

    using TreeSharp;

    internal static class Main
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
            Composite[] composites =
            {
                new Decorator(
                    check => GameObjectManager.Attackers.Count >= 1,
                    new ActionRunCoroutine(coroutine => CombatHandler.Task())),
                new ActionRunCoroutine(coroutine => MainWorker.Task())
            };
            return new PrioritySelector(composites);
        }
    }
}