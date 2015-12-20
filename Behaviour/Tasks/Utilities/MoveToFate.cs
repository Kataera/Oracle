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
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Settings;

using Oracle.Data;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class MoveToFate
    {
        public static async Task<bool> Main(bool ignoreCombat)
        {
            var currentFate = OracleManager.GetCurrentFateData();

            if (!ignoreCombat && GameObjectManager.Attackers.Any(attacker => attacker.IsValid) && !Core.Player.IsMounted)
            {
                return false;
            }

            if (!ignoreCombat && OracleSettings.Instance.TeleportIfQuicker && currentFate.IsValid)
            {
                if (await Teleport.FasterToTeleport(currentFate) && WorldManager.CanTeleport())
                {
                    Logger.SendLog("Teleporting to the closest aetheryte crystal to the FATE.");
                    await Teleport.TeleportToClosestAetheryte(currentFate);

                    if (GameObjectManager.Attackers.Any(attacker => attacker.IsValid))
                    {
                        OracleManager.ClearPoi("We're under attack and can't teleport.");
                        return false;
                    }
                }
            }

            if (!ignoreCombat && IsMountNeeded() && !Core.Player.IsMounted && currentFate.IsValid)
            {
                await Mount.MountUp();
            }

            await Move(ignoreCombat);
            return true;
        }

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
            foreach (var waypoint in oracleFate.CustomWaypoints)
            {
                await MoveToWaypoint(waypoint, false);

                if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                {
                    Logger.SendLog("FATE ended, going back through previous waypoints.");
                    waypointsNavigated.Reverse();

                    // We may not be able to navigate back from current position, so follow the waypoints in the reverse direction.
                    foreach (var reverseWaypoint in waypointsNavigated)
                    {
                        await MoveToWaypoint(reverseWaypoint, true);
                    }

                    return true;
                }

                waypointsNavigated.Add(waypoint);
            }

            return true;
        }

        public static async Task<bool> MoveThroughWaypointsReversed(uint fateId)
        {
            var oracleFate = OracleManager.OracleDatabase.GetFateFromId(fateId);
            var reverseWaypoints = oracleFate.CustomWaypoints;
            reverseWaypoints.Reverse();

            foreach (var waypoint in reverseWaypoints)
            {
                await MoveToWaypoint(waypoint, true);
            }

            return true;
        }

        private static async Task ClearFate()
        {
            OracleManager.SetDoNotWaitFlag(true);
            await OracleManager.ClearCurrentFate("FATE ended before we got there.", false);
            Navigator.Stop();
        }

        private static bool IsMountNeeded()
        {
            var currentFate = OracleManager.GetCurrentFateData();

            if (currentFate == null || !currentFate.IsValid)
            {
                return false;
            }

            var distanceToFateBoundary = Core.Player.Distance(currentFate.Location) - currentFate.Radius;
            return distanceToFateBoundary > CharacterSettings.Instance.MountDistance;
        }

        private static async Task<bool> Move(bool ignoreCombat)
        {
            var currentFate = OracleManager.GetCurrentFateData();
            if (currentFate == null || !currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE
                || currentFate.Status == FateStatus.NOTACTIVE)
            {
                await ClearFate();
                return true;
            }

            var oracleFate = OracleManager.OracleDatabase.GetFateFromFateData(currentFate);
            if (oracleFate.CustomWaypoints.Any() && !ignoreCombat)
            {
                await MoveThroughWaypoints();
            }

            if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
            {
                await ClearFate();
                return true;
            }

            if (WorldManager.CanFly && PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                while (!FateManager.WithinFate)
                {
                    if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                    {
                        await ClearFate();
                        Navigator.Stop();
                        return true;
                    }

                    if (!Core.Player.IsMounted && IsMountNeeded() && Actionmanager.AvailableMounts.Any())
                    {
                        Navigator.Stop();
                        if (!ignoreCombat && Core.Player.InCombat)
                        {
                            return true;
                        }

                        await Mount.MountUp();
                    }

                    Navigator.MoveToPointWithin(currentFate.Location, currentFate.Radius * 0.5f, currentFate.Name);
                    await Coroutine.Yield();
                }
            }
            else
            {
                while (Core.Player.Distance(currentFate.Location) > currentFate.Radius * 0.75f)
                {
                    if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                    {
                        await ClearFate();
                        Navigator.Stop();
                        return true;
                    }

                    if (!Core.Player.IsMounted && IsMountNeeded() && Actionmanager.AvailableMounts.Any())
                    {
                        Navigator.Stop();
                        if (!ignoreCombat && Core.Player.InCombat)
                        {
                            return true;
                        }

                        await Mount.MountUp();
                    }

                    Navigator.MoveToPointWithin(currentFate.Location, currentFate.Radius * 0.5f, currentFate.Name);
                    await Coroutine.Yield();
                }
            }

            Navigator.Stop();
            return true;
        }

        private static async Task<bool> MoveToWaypoint(Waypoint waypoint, bool ignoreExpiredFate)
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

            var location = waypoint.Location;
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

            return true;
        }
    }
}