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
        internal static async Task<bool> HandleZoneChange()
        {
            const uint idyllshireAetheryte = 75;
            MovementSettings.Instance.ZoneLevels.TryGetValue(OracleClassManager.GetTrueLevel(), out uint aetheryteId);

            // Ensure we have no FATE selected to prevent null references.
            await OracleFateManager.ClearCurrentFate("Zone change needed.", false);

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
            Logger.SendLog("Character is level " + OracleClassManager.GetTrueLevel() + ", teleporting to " + zoneName + ".");
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

        internal static async Task<bool> HandleZoneChange(uint zoneId, bool bind)
        {
            const ushort dravanianHinterlands = 399;
            const uint idyllshireAetheryte = 75;
            uint aetheryteId;

            // Ensure we have no FATE selected to prevent null references.
            await OracleFateManager.ClearCurrentFate("Zone change needed.", false);

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

            if (bind && MovementSettings.Instance.BindHomePoint)
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