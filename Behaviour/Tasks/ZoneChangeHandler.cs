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
using Oracle.Managers;
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
            MovementSettings.Instance.ZoneLevels.TryGetValue(OracleFateManager.GetTrueLevel(), out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                Logger.SendErrorLog("Can't find requested teleport destination, make sure you've unlocked it.");
                OracleBot.StopOracle("Cannot teleport to destination.");
                return false;
            }

            if (!WorldManager.CanTeleport())
            {
                return false;
            }

            var zoneName = WorldManager.AvailableLocations.FirstOrDefault(teleport => teleport.AetheryteId == aetheryteId).Name;
            Logger.SendLog("Character is level " + OracleFateManager.GetTrueLevel() + ", teleporting to " + zoneName + ".");
            await Teleport.TeleportToAetheryte(aetheryteId);

            if (WorldManager.ZoneId != WorldManager.GetZoneForAetheryteId(aetheryteId))
            {
                return true;
            }

            if (MovementSettings.Instance.BindHomePoint)
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
                var aetheryte = OracleFateManager.GetAetheryteIdsForZone(zoneId).FirstOrDefault();
                if (aetheryte == null)
                {
                    Logger.SendErrorLog("There's no aetherytes for this zone.");
                    OracleBot.StopOracle("Cannot teleport to destination.");
                    return false;
                }

                aetheryteId = aetheryte.Item1;
            }

            if (!WorldManager.HasAetheryteId(aetheryteId))
            {
                Logger.SendErrorLog("Can't find requested teleport destination, make sure you've unlocked it.");
                OracleBot.StopOracle("Cannot teleport to destination.");
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

            if (MovementSettings.Instance.BindHomePoint)
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