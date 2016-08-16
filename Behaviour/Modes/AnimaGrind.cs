using System.Threading.Tasks;

using ff14bot;
using ff14bot.Managers;
using ff14bot.NeoProfiles;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    internal static class AnimaGrind
    {
        private const int AnimaQuest = 67748;
        private const int AnimaQuestStep = 255;

        private const uint LuminousEarthCrystal = 13572;
        private const uint LuminousFireCrystal = 13571;
        private const uint LuminousIceCrystal = 13569;
        private const uint LuminousLightningCrystal = 13573;
        private const uint LuminousWaterCrystal = 13574;
        private const uint LuminousWindCrystal = 13570;

        private const ushort ZoneAzysLla = 402;
        private const ushort ZoneChurningMists = 400;
        private const ushort ZoneCoerthasWesternHighlands = 397;
        private const ushort ZoneDravanianForelands = 398;
        private const ushort ZoneDravanianHinterlands = 399;
        private const ushort ZoneSeaOfClouds = 401;

        public static async Task<bool> HandleAnimaGrind()
        {
            if (!ConditionParser.HasQuest(AnimaQuest))
            {
                Logger.SendErrorLog("You do not have the quest 'Soul Without Life', which is required to run in Anima grind mode.");

                OracleBot.StopOracle("Required quest is not picked up.");
                return true;
            }

            if (ConditionParser.GetQuestStep(AnimaQuest) != AnimaQuestStep)
            {
                Logger.SendErrorLog("You are not at the correct step of 'Soul Without Life'. You must be at the objective that says "
                                    + "\"Deliver the astral nodule and umbral nodule to Ardashir in Azys Lla.\" to run in Anima grind mode.");

                OracleBot.StopOracle("Not at required step of quest.");
                return true;
            }

            if (!ConditionParser.HasAtLeast(LuminousIceCrystal, ModeSettings.Instance.AnimaCrystalsToFarm))
            {
                if (OracleClassManager.GetTrueLevel() < 50)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in Coerthas Western Highlands.");
                    OracleBot.StopOracle("Too low level to continue.");
                    return true;
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == ZoneCoerthasWesternHighlands)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousIceCrystal))
                               + " more Luminous Ice Crystals.");
                await ZoneChange.HandleZoneChange(ZoneCoerthasWesternHighlands);
            }
            else if (!ConditionParser.HasAtLeast(LuminousWindCrystal, 3))
            {
                if (OracleClassManager.GetTrueLevel() < 50)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in The Sea of Clouds.");
                    OracleBot.StopOracle("Too low level to continue.");
                    return true;
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == ZoneSeaOfClouds)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousWindCrystal))
                               + " more Luminous Wind Crystals.");
                await ZoneChange.HandleZoneChange(ZoneSeaOfClouds);
            }
            else if (!ConditionParser.HasAtLeast(LuminousEarthCrystal, 3))
            {
                if (OracleClassManager.GetTrueLevel() < 50)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in The Dravanian Forelands.");
                    OracleBot.StopOracle("Too low level to continue.");
                    return true;
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == ZoneDravanianForelands)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousEarthCrystal))
                               + " more Luminous Earth Crystals.");
                await ZoneChange.HandleZoneChange(ZoneDravanianForelands);
            }
            else if (!ConditionParser.HasAtLeast(LuminousLightningCrystal, 3))
            {
                if (OracleClassManager.GetTrueLevel() < 52)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in The Churning Mists.");
                    OracleBot.StopOracle("Too low level to continue.");
                    return true;
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == ZoneChurningMists)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousLightningCrystal))
                               + " more Luminous Lightning Crystals.");
                await ZoneChange.HandleZoneChange(ZoneChurningMists);
            }
            else if (!ConditionParser.HasAtLeast(LuminousWaterCrystal, 3))
            {
                if (OracleClassManager.GetTrueLevel() < 54)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in The Dravanian Hinterlands.");
                    OracleBot.StopOracle("Too low level to continue.");
                    return true;
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == ZoneDravanianHinterlands)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousWaterCrystal))
                               + " more Luminous Water Crystals.");
                await ZoneChange.HandleZoneChange(ZoneDravanianHinterlands);
            }
            else if (!ConditionParser.HasAtLeast(LuminousFireCrystal, 3))
            {
                if (OracleClassManager.GetTrueLevel() < 55)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in Azys Lla.");
                    OracleBot.StopOracle("Too low level to continue.");
                    return true;
                }

                if (Core.Player.InCombat || !WorldManager.CanTeleport() || WorldManager.ZoneId == ZoneAzysLla)
                {
                    return true;
                }

                Logger.SendLog("We need " + (ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousFireCrystal))
                               + " more Luminous Fire Crystals.");
                await ZoneChange.HandleZoneChange(ZoneAzysLla);
            }
            else if (!Core.Player.InCombat)
            {
                Logger.SendLog("We have collected " + ModeSettings.Instance.AnimaCrystalsToFarm + " of every crystal! Stopping Oracle.");
                await OracleTeleportManager.TeleportToClosestCity();
                OracleBot.StopOracle("We are done!");
            }

            return true;
        }
    }
}