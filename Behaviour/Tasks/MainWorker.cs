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

namespace Tarot.Behaviour.Tasks
{
    using System.Threading.Tasks;

    using ff14bot.Behavior;
    using ff14bot.Helpers;

    using global::Tarot.Behaviour.Tasks.Handlers;
    using global::Tarot.Behaviour.Tasks.Utilities;
    using global::Tarot.Helpers;

    internal static class MainWorker
    {
        public static async Task<bool> Task()
        {
            // Handle combat.
            if (Poi.Current != null && Poi.Current.Type == PoiType.Kill)
            {
                await new HookExecutor("Pull").ExecuteCoroutine();
                await new HookExecutor("RoutineCombat").ExecuteCoroutine();
            }

            // Check that the FATE database has been populated.
            if (Tarot.FateDatabase == null)
            {
                await BuildFateDatabase.Task();
            }

            // Handle FATE.
            if (Poi.Current != null && Poi.Current.Type == PoiType.Fate && Tarot.CurrentFate != null)
            {
                await FateHandler.Task();
            }

            // Handle idle.
            if (Poi.Current != null && Poi.Current.Type == PoiType.Wait && Tarot.CurrentFate == null)
            {
                await IdleHandler.Task();
            }

            // Always return false to not block the tree.
            return false;
        }
    }
}