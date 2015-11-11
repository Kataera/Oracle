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

    using ff14bot;
    using ff14bot.Behavior;
    using ff14bot.Helpers;
    using ff14bot.Managers;

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

            await new HookExecutor("Pull").ExecuteCoroutine();
            await new HookExecutor("RoutineCombat").ExecuteCoroutine();

            if (Poi.Current.BattleCharacter.IsDead)
            {
                Poi.Clear("Targeted unit is dead, clearing Poi and carrying on!");
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
            return true;
        }
    }
}