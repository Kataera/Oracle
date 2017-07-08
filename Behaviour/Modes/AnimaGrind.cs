using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

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

        private const ushort ZoneMorDhona = 156;
        private const ushort ZoneAzysLla = 402;
        private const ushort ZoneChurningMists = 400;
        private const ushort ZoneCoerthasWesternHighlands = 397;
        private const ushort ZoneDravanianForelands = 398;
        private const ushort ZoneDravanianHinterlands = 399;
        private const ushort ZoneSeaOfClouds = 401;

        internal static async Task<bool> HandleAnimaGrind()
        {
            if (!ConditionParser.HasQuest(AnimaQuest))
            {
                Logger.SendErrorLog("You do not have the quest 'Soul Without Life', which is required to run in Anima grind mode.");
                OracleBot.StopOracle("Required quest is not picked up.");
                return true;
            }

            if (ConditionParser.GetQuestStep(AnimaQuest) != AnimaQuestStep)
            {
                OracleFateManager.PausePoiSetting = true;

                if (WorldManager.ZoneId != ZoneMorDhona)
                {
                    await ZoneChange.HandleZoneChange(ZoneMorDhona, false);
                }

                // Step 1: Talk to Rowena.
                if (ConditionParser.GetQuestStep(AnimaQuest) == 1)
                {
                    var rowenaLocation = new Vector3(25.65759f, 29f, -822.5876f);
                    if (Core.Player.Distance(rowenaLocation) > 10f)
                    {
                        await OracleMovementManager.NavigateToLocation(rowenaLocation, 2f, false);
                    }

                    const uint rowenaNpcId = 1001304;
                    var rowenaGameObject = GameObjectManager.GameObjects.FirstOrDefault(npc => npc.NpcId == rowenaNpcId);
                    if (rowenaGameObject != null)
                    {
                        if (Core.Player.Distance2D(rowenaGameObject.Location) > 4f)
                        {
                            await OracleMovementManager.NavigateToLocation(rowenaGameObject.Location, 4f, false);
                        }

                        rowenaGameObject.Interact();
                        await Coroutine.Sleep(MainSettings.Instance.ActionDelay);
                        await SkipDialogue.Main();
                    }
                }

                // Step 2: Talk to Syndony.
                if (ConditionParser.GetQuestStep(AnimaQuest) == 2)
                {
                    var syndonyLocation = new Vector3(56.6797f, 50f, -777.5304f);
                    if (Core.Player.Distance(syndonyLocation) > 10f)
                    {
                        await OracleMovementManager.NavigateToLocation(syndonyLocation, 2f, false);
                    }

                    const uint syndonyNpcId = 1016289;
                    var syndonyGameObject = GameObjectManager.GameObjects.FirstOrDefault(npc => npc.NpcId == syndonyNpcId);
                    if (syndonyGameObject != null)
                    {
                        if (Core.Player.Distance2D(syndonyGameObject.Location) > 4f)
                        {
                            await OracleMovementManager.NavigateToLocation(syndonyGameObject.Location, 4f, false);
                        }

                        syndonyGameObject.Interact();
                        await Coroutine.Sleep(MainSettings.Instance.ActionDelay);
                        await SkipDialogue.Main();
                    }
                }
            }
            else
            {
                if (OracleFateManager.PausePoiSetting)
                {
                    OracleFateManager.PausePoiSetting = false;
                }
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

                var amountNeeded = ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousIceCrystal);
                if (amountNeeded == 1)
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Ice Crystal.");
                }
                else
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Ice Crystals.");
                }

                await ZoneChange.HandleZoneChange(ZoneCoerthasWesternHighlands, true);
            }
            else if (!ConditionParser.HasAtLeast(LuminousWindCrystal, ModeSettings.Instance.AnimaCrystalsToFarm))
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

                var amountNeeded = ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousWindCrystal);
                if (amountNeeded == 1)
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Wind Crystal.");
                }
                else
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Wind Crystals.");
                }

                await ZoneChange.HandleZoneChange(ZoneSeaOfClouds, true);
            }
            else if (!ConditionParser.HasAtLeast(LuminousEarthCrystal, ModeSettings.Instance.AnimaCrystalsToFarm))
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

                var amountNeeded = ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousEarthCrystal);
                if (amountNeeded == 1)
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Earth Crystal.");
                }
                else
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Earth Crystals.");
                }

                await ZoneChange.HandleZoneChange(ZoneDravanianForelands, true);
            }
            else if (!ConditionParser.HasAtLeast(LuminousLightningCrystal, ModeSettings.Instance.AnimaCrystalsToFarm))
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

                var amountNeeded = ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousLightningCrystal);
                if (amountNeeded == 1)
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Lightning Crystal.");
                }
                else
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Lightning Crystals.");
                }

                await ZoneChange.HandleZoneChange(ZoneChurningMists, true);
            }
            else if (!ConditionParser.HasAtLeast(LuminousWaterCrystal, ModeSettings.Instance.AnimaCrystalsToFarm))
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

                var amountNeeded = ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousWaterCrystal);
                if (amountNeeded == 1)
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Water Crystal.");
                }
                else
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Water Crystals.");
                }

                await ZoneChange.HandleZoneChange(ZoneDravanianHinterlands, true);
            }
            else if (!ConditionParser.HasAtLeast(LuminousFireCrystal, ModeSettings.Instance.AnimaCrystalsToFarm))
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

                var amountNeeded = ModeSettings.Instance.AnimaCrystalsToFarm - OracleInventoryManager.GetItemAmount(LuminousFireCrystal);
                if (amountNeeded == 1)
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Fire Crystal.");
                }
                else
                {
                    Logger.SendLog("We need " + amountNeeded + " more Luminous Fire Crystals.");
                }

                await ZoneChange.HandleZoneChange(ZoneAzysLla, true);
            }
            else if (!Core.Player.InCombat)
            {
                Logger.SendLog("We have collected " + ModeSettings.Instance.AnimaCrystalsToFarm + " of every crystal! Stopping Oracle.");

                await OracleTeleportManager.TeleportToClosestCity();

                if (OracleTeleportManager.InCity())
                {
                    OracleBot.StopOracle("We are done!");
                }
            }

            return true;
        }
    }
}