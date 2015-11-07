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
    using System.Linq;
    using System.Threading.Tasks;

    using Buddy.Coroutines;

    using ff14bot;
    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Helpers;

    using TreeSharp;

    internal static class CombatHandler
    {
        public static Composite Behaviour
        {
            get
            {
                return CreateBehaviour();
            }
        }

        private static void SetCombatPoi()
        {
            var attackers = GameObjectManager.Attackers;

            if (attackers.Count > 0 && Poi.Current.Type != PoiType.Kill)
            {
                Poi.Current = new Poi(attackers.First(), PoiType.Kill);
            }
        }

        private static void ResetCombatPoi()
        {
            if (Poi.Current == null)
            {
                Logger.SendDebugLog("Combat routine ended.");
                Poi.Current = Tarot.CurrentPoi;
            }
            else if (Poi.Current.BattleCharacter != null)
            {
                if (!Poi.Current.BattleCharacter.IsAlive)
                {
                    Logger.SendDebugLog("Combat routine ended.");
                    Poi.Current = Tarot.CurrentPoi;
                }
            }
        }

        private static Composite CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new Action(action => SetCombatPoi()), RoutineManager.Current.HealBehavior,
                RoutineManager.Current.CombatBuffBehavior, RoutineManager.Current.CombatBehavior,
                new Action(action => ResetCombatPoi()),
                new ActionRunCoroutine(coroutine => Coroutine.Yield())
            };

            return new WhileLoop(check => Core.Player.InCombat, behaviours);
        }
    }
}