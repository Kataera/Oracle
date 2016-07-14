using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;

using Oracle.Behaviour.Tasks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;

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

        public static async Task<bool> Main()
        {
            if (!ConditionParser.HasQuest(AnimaQuest))
            {
                Logger.SendErrorLog("You do not have the quest 'Soul Without Life', which is required to run in Anima grind mode.");

                TreeRoot.Stop("Required quest is not picked up.");
                return true;
            }

            if (ConditionParser.GetQuestStep(AnimaQuest) != AnimaQuestStep)
            {
                Logger.SendErrorLog(
                    "You are not at the correct step of 'Soul Without Life'. You must be at the objective that says " +
                    "\"Deliver the astral nodule and umbral nodule to Ardashir in Azys Lla.\" to run in Anima grind mode.");

                TreeRoot.Stop("Not at required step of quest.");
                return true;
            }

            if (Poi.Current.Type == PoiType.Kill)
            {
                await CombatHandler.HandleCombat();
                return true;
            }

            if (!ConditionParser.HasAtLeast(LuminousIceCrystal, 3))
            {
                if (Core.Player.ClassLevel < 50)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in Coerthas Western Highlands.");
                    TreeRoot.Stop("Too low level to continue.");
                    return true;
                }

                if (WorldManager.ZoneId != ZoneCoerthasWesternHighlands)
                {
                    Logger.SendLog("We need more Luminous Ice Crystals. Teleporting to Coerthas Western Highlands.");
                    await ZoneChangeHandler.HandleZoneChange(ZoneCoerthasWesternHighlands);
                }
            }

            else if (!ConditionParser.HasAtLeast(LuminousWindCrystal, 3))
            {
                if (Core.Player.ClassLevel < 50)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in The Sea of Clouds.");
                    TreeRoot.Stop("Too low level to continue.");
                    return true;
                }

                if (WorldManager.ZoneId != ZoneSeaOfClouds)
                {
                    Logger.SendLog("We need more Luminous Wind Crystals. Teleporting to The Sea of Clouds.");
                    await ZoneChangeHandler.HandleZoneChange(ZoneSeaOfClouds);
                }
            }

            else if (!ConditionParser.HasAtLeast(LuminousEarthCrystal, 3))
            {
                if (Core.Player.ClassLevel < 50)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in The Dravanian Forelands.");
                    TreeRoot.Stop("Too low level to continue.");
                    return true;
                }

                if (WorldManager.ZoneId != ZoneDravanianForelands)
                {
                    Logger.SendLog("We need more Luminous Earth Crystals. Teleporting to The Dravanian Forelands.");
                    await ZoneChangeHandler.HandleZoneChange(ZoneDravanianForelands);
                }
            }

            else if (!ConditionParser.HasAtLeast(LuminousLightningCrystal, 3))
            {
                if (Core.Player.ClassLevel < 52)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in The Churning Mists.");
                    TreeRoot.Stop("Too low level to continue.");
                    return true;
                }

                if (WorldManager.ZoneId != ZoneChurningMists)
                {
                    Logger.SendLog("We need more Luminous Lightning Crystals. Teleporting to The Churning Mists.");
                    await ZoneChangeHandler.HandleZoneChange(ZoneChurningMists);
                }
            }

            else if (!ConditionParser.HasAtLeast(LuminousWaterCrystal, 3))
            {
                if (Core.Player.ClassLevel < 54)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in The Dravanian Hinterlands.");
                    TreeRoot.Stop("Too low level to continue.");
                    return true;
                }

                if (WorldManager.ZoneId != ZoneDravanianHinterlands)
                {
                    Logger.SendLog("We need more Luminous Water Crystals. Teleporting to The Dravanian Hinterlands.");
                    await ZoneChangeHandler.HandleZoneChange(ZoneDravanianHinterlands);
                }
            }

            else if (!ConditionParser.HasAtLeast(LuminousFireCrystal, 3))
            {
                if (Core.Player.ClassLevel < 55)
                {
                    Logger.SendErrorLog("You are too low level to run FATEs in Azys Lla.");
                    TreeRoot.Stop("Too low level to continue.");
                    return true;
                }

                if (WorldManager.ZoneId != ZoneAzysLla)
                {
                    Logger.SendLog("We need more Luminous Fire Crystals. Teleporting to Azys Lla.");
                    await ZoneChangeHandler.HandleZoneChange(ZoneAzysLla);
                }
            }

            else if (!Core.Player.InCombat)
            {
                Logger.SendLog("You have gathered all the crystals needed! Teleporting to Limsa Lominsa and stopping Oracle.");
                await Teleport.TeleportToAetheryte(8);

                TreeRoot.Stop("We are done!");
                return true;
            }

            if (Poi.Current.Type == PoiType.Fate || OracleFateManager.CurrentFateId != 0)
            {
                await FateHandler.HandleFate();
            }

            else if (Poi.Current.Type == PoiType.Wait)
            {
                await WaitHandler.HandleWait();
            }

            return true;
        }
    }
}