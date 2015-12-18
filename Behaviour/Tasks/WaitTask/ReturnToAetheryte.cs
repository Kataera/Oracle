﻿/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Navigation;
using ff14bot.Settings;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class ReturnToAetheryte
    {
        public static async Task<bool> Main()
        {
            if (Poi.Current.Type != PoiType.Wait)
            {
                return false;
            }

            if (await OracleFateManager.AnyViableFates())
            {
                OracleBehaviour.ClearPoi("Found a FATE.");
                return true;
            }

            if (!(Core.Player.Distance2D(Poi.Current.Location) > 15f))
            {
                return true;
            }

            while (Core.Player.Distance2D(Poi.Current.Location) > 15f)
            {
                // Check if a FATE popped while we're moving.
                if (await OracleFateManager.AnyViableFates())
                {
                    Navigator.Stop();
                    OracleBehaviour.ClearPoi("Found a FATE.");
                    return true;
                }

                // Check we're still mounted.
                if (!Core.Player.IsMounted
                    && Core.Player.Distance(Poi.Current.Location) > CharacterSettings.Instance.MountDistance)
                {
                    Navigator.PlayerMover.MoveStop();

                    // Exit behaviour if we're in combat.
                    if (Core.Player.InCombat)
                    {
                        return false;
                    }

                    await CommonBehaviors.CreateMountBehavior().ExecuteCoroutine();
                }

                Navigator.MoveToPointWithin(Poi.Current.Location, 15f, "Moving to Aetheryte");
                await Coroutine.Yield();
            }

            Navigator.PlayerMover.MoveStop();

            return true;
        }
    }
}