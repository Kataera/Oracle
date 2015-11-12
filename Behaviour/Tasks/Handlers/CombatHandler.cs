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

namespace Tarot.Behaviour.Tasks.Handlers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Buddy.Coroutines;

    using Clio.Utilities;

    using ff14bot;
    using ff14bot.Behavior;
    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Helpers;

    internal static class CombatHandler
    {
        public static async Task<bool> Task()
        {
            if (Poi.Current.Type != PoiType.Kill)
            {
                Poi.Clear("Clearing Poi while in combat.");
                Poi.Current = new Poi(GameObjectManager.Attackers.FirstOrDefault(), PoiType.Kill);
            }

            if (Core.Player.CurrentTarget != Poi.Current.Unit)
            {
                Poi.Current.Unit.Target();

                // Give time for the game to update.
                await Coroutine.Sleep(100);
            }

            // Support for Kupo, which will not move in range unless its pull behaviour is called.
            if (RoutineManager.Current.Name.Contains("Kupo"))
            {
                if (MovementNeeded())
                {
                    await RoutineManager.Current.PullBehavior.ExecuteCoroutine();
                }
                else
                {
                    await RoutineManager.Current.CombatBehavior.ExecuteCoroutine();
                }
            }
            else
            {
                await RoutineManager.Current.CombatBehavior.ExecuteCoroutine();
            }

            if (Poi.Current.BattleCharacter.IsDead)
            {
                Poi.Clear("Targeted unit is dead, clearing Poi and carrying on!");
                if (GameObjectManager.Attackers.Count >= 1)
                {
                    Poi.Current = new Poi(GameObjectManager.Attackers.FirstOrDefault(), PoiType.Kill);
                }
            }

            if (Poi.Current.BattleCharacter.IsFateGone)
            {
                Poi.Clear("Target is a FATE mob, and the FATE is gone.");
                if (GameObjectManager.Attackers.Count >= 1)
                {
                    Poi.Current = new Poi(GameObjectManager.Attackers.FirstOrDefault(), PoiType.Kill);
                }
            }

            // Reset Poi back to what it was when we're done.
            if (GameObjectManager.Attackers.Count == 0 && Tarot.CurrentPoi != null)
            {
                Poi.Current = Tarot.CurrentPoi;
            }
            else if(Tarot.CurrentPoi == null && Poi.Current.Type != PoiType.Kill)
            {
                Poi.Current = new Poi(Vector3.Zero, PoiType.None);
            }

            return true;
        }

        private static bool MovementNeeded()
        {
            var minimumDistance = Core.Player.CombatReach + RoutineManager.Current.PullRange
                                  + Poi.Current.BattleCharacter.CombatReach + 1;
            return Core.Player.Location.Distance(Poi.Current.BattleCharacter.Location) > minimumDistance;
        }
    }
}