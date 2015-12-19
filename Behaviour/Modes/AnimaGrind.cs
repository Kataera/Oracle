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

using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;

using Oracle.Behaviour.Tasks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;

namespace Oracle.Behaviour.Modes
{
    internal static class AnimaGrind
    {
        public static async Task<bool> Main()
        {
            const int animaQuest = 67748;
            const int animaQuestStep = 255;
            const uint iceCrystal = 13569;
            const uint windCrystal = 13570;
            const uint fireCrystal = 13571;
            const uint earthCrystal = 13572;
            const uint lightningCrystal = 13573;
            const uint waterCrystal = 13574;
            const ushort coerthasWesternHighlands = 397;
            const ushort seaOfClouds = 401;
            const ushort azysLla = 402;
            const ushort dravanianForelands = 398;
            const ushort churningMists = 400;
            const ushort dravanianHinterlands = 399;

            if (!ConditionParser.HasQuest(animaQuest))
            {
                Logger.SendErrorLog("You do not have the quest 'Soul Without Life', which is required to run in Anima grind mode.");

                TreeRoot.Stop("Required quest is not picked up.");
                return true;
            }

            if (ConditionParser.GetQuestStep(animaQuest) != animaQuestStep)
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

            if (!ConditionParser.HasAtLeast(iceCrystal, 3))
            {
                if (WorldManager.ZoneId != coerthasWesternHighlands)
                {
                    Logger.SendLog("We need more Luminous Ice Crystals. Teleporting to Coerthas Western Highlands.");
                    await ZoneChangeHandler.HandleZoneChange(coerthasWesternHighlands);
                }
            }

            else if (!ConditionParser.HasAtLeast(windCrystal, 3))
            {
                if (WorldManager.ZoneId != seaOfClouds)
                {
                    Logger.SendLog("We need more Luminous Wind Crystals. Teleporting to The Sea of Clouds.");
                    await ZoneChangeHandler.HandleZoneChange(seaOfClouds);
                }
            }

            else if (!ConditionParser.HasAtLeast(fireCrystal, 3))
            {
                if (WorldManager.ZoneId != azysLla)
                {
                    Logger.SendLog("We need more Luminous Fire Crystals. Teleporting to Azys Lla.");
                    await ZoneChangeHandler.HandleZoneChange(azysLla);
                }
            }

            else if (!ConditionParser.HasAtLeast(earthCrystal, 3))
            {
                if (WorldManager.ZoneId != dravanianForelands)
                {
                    Logger.SendLog("We need more Luminous Earth Crystals. Teleporting to The Dravanian Forelands.");
                    await ZoneChangeHandler.HandleZoneChange(dravanianForelands);
                }
            }

            else if (!ConditionParser.HasAtLeast(lightningCrystal, 3))
            {
                if (WorldManager.ZoneId != churningMists)
                {
                    Logger.SendLog("We need more Luminous Lightning Crystals. Teleporting to The Churning Mists.");
                    await ZoneChangeHandler.HandleZoneChange(churningMists);
                }
            }

            else if (!ConditionParser.HasAtLeast(waterCrystal, 3))
            {
                if (WorldManager.ZoneId != dravanianHinterlands)
                {
                    Logger.SendLog("We need more Luminous Water Crystals. Teleporting to The Dravanian Hinterlands.");
                    await ZoneChangeHandler.HandleZoneChange(dravanianHinterlands);
                }
            }

            else if (!Core.Player.InCombat)
            {
                Logger.SendLog("You have gathered all the crystals needed! Teleporting to Limsa Lominsa and stopping Oracle.");
                await Teleport.TeleportToAetheryte(8);

                TreeRoot.Stop("We are done!");
                return true;
            }

            switch (Poi.Current.Type)
            {
                case PoiType.Fate:
                    await FateHandler.HandleFate();
                    break;
                case PoiType.Wait:
                    await WaitHandler.HandleWait();
                    break;
            }

            return true;
        }
    }
}