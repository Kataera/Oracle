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

using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks
{
    internal static class ZoneChangeHandler
    {
        private const ushort DravanianHinterlands = 399;
        private const uint IdyllshireAetheryte = 75;

        public static async Task<bool> HandleZoneChange()
        {
            uint aetheryteId;
            OracleSettings.Instance.ZoneLevels.TryGetValue(Core.Player.ClassLevel, out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                Logger.SendErrorLog("Can't find requested teleport destination, make sure you've unlocked it.");
                TreeRoot.Stop("Cannot teleport to destination.");
                return false;
            }

            if (!WorldManager.CanTeleport())
            {
                return false;
            }

            var zoneName = WorldManager.AvailableLocations.FirstOrDefault(teleport => teleport.AetheryteId == aetheryteId).Name;
            Logger.SendLog("Character is level " + Core.Player.ClassLevel + ", teleporting to " + zoneName + ".");
            await Teleport.TeleportToAetheryte(aetheryteId);

            if (WorldManager.ZoneId != WorldManager.GetZoneForAetheryteId(aetheryteId))
            {
                return true;
            }

            if (OracleSettings.Instance.BindHomePoint)
            {
                await BindHomePoint.Main(aetheryteId);
            }

            if (aetheryteId == IdyllshireAetheryte)
            {
                await MoveOutOfIdyllshire();
            }

            return true;
        }

        public static async Task<bool> HandleZoneChange(uint zoneId)
        {
            uint aetheryteId;

            if (zoneId == DravanianHinterlands)
            {
                aetheryteId = IdyllshireAetheryte;
            }
            else
            {
                var aetheryte = WorldManager.AetheryteIdsForZone(zoneId).FirstOrDefault();
                if (aetheryte == null)
                {
                    Logger.SendErrorLog("There's no aetherytes for this zone.");
                    TreeRoot.Stop("Cannot teleport to destination.");
                    return false;
                }

                aetheryteId = aetheryte.Item1;
            }

            if (!WorldManager.HasAetheryteId(aetheryteId))
            {
                Logger.SendErrorLog("Can't find requested teleport destination, make sure you've unlocked it.");
                TreeRoot.Stop("Cannot teleport to destination.");
                return false;
            }

            if (!WorldManager.CanTeleport())
            {
                return false;
            }

            var zoneName = WorldManager.AvailableLocations.FirstOrDefault(teleport => teleport.AetheryteId == aetheryteId).Name;
            Logger.SendLog("Teleporting to " + zoneName + ".");
            await Teleport.TeleportToAetheryte(aetheryteId);

            if (WorldManager.ZoneId != WorldManager.GetZoneForAetheryteId(aetheryteId))
            {
                return true;
            }

            if (OracleSettings.Instance.BindHomePoint)
            {
                await BindHomePoint.Main(aetheryteId);
            }

            if (aetheryteId == IdyllshireAetheryte)
            {
                await MoveOutOfIdyllshire();
            }

            return true;
        }

        private static async Task<bool> MoveOutOfIdyllshire()
        {
            Logger.SendLog("We're in Idyllshire, moving to The Dravanian Hinterlands.");
            await Mount.MountUp();

            var location = new Vector3(142.6006f, 207f, 114.136f);
            while (Core.Player.Distance(location) > 5f)
            {
                Navigator.MoveTo(location, "Leaving Idyllshire");
                await Coroutine.Yield();
            }
            Navigator.Stop();
            Core.Player.SetFacing(0.9709215f);
            MovementManager.MoveForwardStart();

            await Coroutine.Sleep(TimeSpan.FromSeconds(2));
            await Coroutine.Wait(TimeSpan.MaxValue, () => !CommonBehaviors.IsLoading);
            await Coroutine.Sleep(TimeSpan.FromSeconds(2));

            return true;
        }
    }
}