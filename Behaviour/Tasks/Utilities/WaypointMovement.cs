/*
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

using System.Collections.Generic;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Navigation;

using Oracle.Data;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class WaypointMovement
    {
        public static bool ReturnFlag { get; set; }

        public static async Task<bool> MoveThroughWaypoints()
        {
            var currentFate = OracleManager.GetCurrentFateData();
            if (currentFate == null || !currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE
                || currentFate.Status == FateStatus.NOTACTIVE)
            {
                return true;
            }

            var oracleFate = OracleManager.OracleDatabase.GetFateFromFateData(currentFate);
            var waypointsNavigated = new List<Waypoint>();
            var waypointCount = 1;
            foreach (var waypoint in oracleFate.CustomWaypoints)
            {
                if (waypointCount == 1)
                {
                    await MoveToWaypoint(waypoint, false, true);
                }
                else
                {
                    await MoveToWaypoint(waypoint, false, false);
                }

                if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                {
                    Logger.SendLog("FATE ended, going back through previous waypoints.");
                    waypointsNavigated.Reverse();

                    // We may not be able to navigate back from current position, so follow the waypoints in the reverse direction.
                    foreach (var reverseWaypoint in waypointsNavigated)
                    {
                        await MoveToWaypoint(reverseWaypoint, true, false);
                    }

                    return true;
                }

                waypointCount++;
                waypointsNavigated.Add(waypoint);
            }

            Navigator.PlayerMover.MoveStop();
            await CommonTasks.Land();
            ReturnFlag = true;

            return true;
        }

        public static async Task<bool> MoveThroughWaypointsReversed(uint fateId)
        {
            var oracleFate = OracleManager.OracleDatabase.GetFateFromId(fateId);
            var reverseWaypoints = oracleFate.CustomWaypoints;
            reverseWaypoints.Reverse();

            var waypointCount = 1;
            foreach (var waypoint in reverseWaypoints)
            {
                if (waypointCount == 1)
                {
                    await MoveToWaypoint(waypoint, false, true);
                }
                else
                {
                    await MoveToWaypoint(waypoint, false, false);
                }

                waypointCount++;
            }

            ReturnFlag = false;
            Navigator.PlayerMover.MoveStop();
            return true;
        }

        private static async Task<bool> MoveToWaypoint(Waypoint waypoint, bool ignoreExpiredFate, bool useNavServer)
        {
            if (!ignoreExpiredFate)
            {
                var currentFate = OracleManager.GetCurrentFateData();
                if (currentFate == null || !currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE
                    || currentFate.Status == FateStatus.NOTACTIVE)
                {
                    return true;
                }
            }

            if (!Core.Player.IsMounted)
            {
                await Mount.MountUp();
            }

            Logger.SendLog("Moving to waypoint " + waypoint.Order + ".");
            var location = waypoint.Location;
            if (useNavServer)
            {
                while (Navigator.MoveTo(location, "Waypoint " + waypoint.Order) != MoveResult.Done)
                {
                    if (!ignoreExpiredFate)
                    {
                        var currentFate = OracleManager.GetCurrentFateData();
                        if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                        {
                            Navigator.Stop();
                            return true;
                        }
                    }

                    await Coroutine.Yield();
                }
            }
            else
            {
                while (Core.Player.Distance(location) > 2f)
                {
                    if (!ignoreExpiredFate)
                    {
                        var currentFate = OracleManager.GetCurrentFateData();
                        if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                        {
                            Navigator.PlayerMover.MoveStop();
                            return true;
                        }
                    }

                    Navigator.PlayerMover.MoveTowards(location);
                    await Coroutine.Yield();
                }
            }

            return true;
        }
    }
}