/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

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

using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Settings;

using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.WaitTask
{
    internal static class ReturnToAetheryte
    {
        public static async Task<bool> Main()
        {
            if (Core.Player.Location.Distance2D(Poi.Current.Location) < 15f)
            {
                return true;
            }

            // Support for ExBuddy's flight navigator, which was having problems with MoveToPointWithin.
            if (PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                await MoveCloseToAetheryte();
            }
            else
            {
                await MoveToRandomPointNearAetheryte();
            }

            Navigator.Clear();
            return true;
        }

        private static async Task<bool> MoveCloseToAetheryte()
        {
            while (Core.Player.Location.Distance2D(Poi.Current.Location) > 15f)
            {
                if (await OracleManager.AnyViableFates())
                {
                    Navigator.Stop();
                    OracleManager.ClearPoi("Found a FATE.");
                    return true;
                }

                if (!Core.Player.IsMounted
                    && Core.Player.Distance(Poi.Current.Location) > CharacterSettings.Instance.MountDistance)
                {
                    Navigator.PlayerMover.MoveStop();
                    if (Core.Player.InCombat)
                    {
                        return false;
                    }

                    await CommonBehaviors.CreateMountBehavior().ExecuteCoroutine();
                }

                Navigator.MoveTo(Poi.Current.Location, "Moving to Aetheryte");
                await Coroutine.Yield();
            }

            Navigator.Clear();
            return true;
        }

        private static async Task<bool> MoveToRandomPointNearAetheryte()
        {
            var result = Navigator.MoveToPointWithin(Poi.Current.Location, 15f, "Moving to Aetheryte");
            while (result != MoveResult.Done || result != MoveResult.ReachedDestination)
            {
                if (await OracleManager.AnyViableFates())
                {
                    Navigator.Stop();
                    OracleManager.ClearPoi("Found a FATE.");
                    return true;
                }

                if (!Core.Player.IsMounted
                    && Core.Player.Distance(Poi.Current.Location) > CharacterSettings.Instance.MountDistance)
                {
                    Navigator.PlayerMover.MoveStop();
                    if (Core.Player.InCombat)
                    {
                        return false;
                    }

                    await CommonBehaviors.CreateMountBehavior().ExecuteCoroutine();
                }

                result = Navigator.MoveToPointWithin(Poi.Current.Location, 15f, "Moving to Aetheryte");
                await Coroutine.Yield();
            }

            Navigator.Clear();
            return true;
        }
    }
}