using System.Linq;
using System.Threading.Tasks;

using ff14bot.Managers;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class ZoneChange
    {
        public static async Task<bool> HandleZoneChange()
        {
            const uint idyllshireAetheryte = 75;
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
            await OracleTeleportManager.TeleportToAetheryte(aetheryteId);

            if (WorldManager.ZoneId != WorldManager.GetZoneForAetheryteId(aetheryteId))
            {
                return true;
            }

            if (MovementSettings.Instance.BindHomePoint)
            {
                await BindHomePoint.Main(aetheryteId);
            }

            if (aetheryteId == idyllshireAetheryte)
            {
                await OracleMovementManager.MoveOutOfIdyllshire();
            }

            return true;
        }

        public static async Task<bool> HandleZoneChange(uint zoneId)
        {
            const ushort dravanianHinterlands = 399;
            const uint idyllshireAetheryte = 75;
            uint aetheryteId;

            if (zoneId == dravanianHinterlands)
            {
                aetheryteId = idyllshireAetheryte;
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
            await OracleTeleportManager.TeleportToAetheryte(aetheryteId);

            if (WorldManager.ZoneId != WorldManager.GetZoneForAetheryteId(aetheryteId))
            {
                return true;
            }

            if (MovementSettings.Instance.BindHomePoint)
            {
                await BindHomePoint.Main(aetheryteId);
            }

            if (aetheryteId == idyllshireAetheryte)
            {
                await OracleMovementManager.MoveOutOfIdyllshire();
            }

            return true;
        }
    }
}