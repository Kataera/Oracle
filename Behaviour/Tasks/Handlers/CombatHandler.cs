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
                await new HookExecutor("SetCombatPoi").ExecuteCoroutine();
                await Coroutine.Sleep(500);
            }

            if (Core.Player.CurrentTarget != Poi.Current.Unit)
            {
                Poi.Current.Unit.Target();
            }

            await new HookExecutor("Pull").ExecuteCoroutine();
            await new HookExecutor("RoutineCombat").ExecuteCoroutine();

            // Reset Poi back to what it was when we're done.
            if (GameObjectManager.Attackers.Count == 0 && Tarot.CurrentPoi != null)
            {
                Poi.Current = Tarot.CurrentPoi;
            }
            return true;
        }
    }
}