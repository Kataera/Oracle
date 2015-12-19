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

using System.Linq;
using System.Threading.Tasks;

using Clio.Utilities;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

using TreeSharp;

namespace Oracle.Behaviour
{
    internal static class OracleBehaviour
    {
        public static Composite Behaviour
        {
            get { return CreateBehaviour(); }
        }

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        private static async Task<bool> FateGrindMode()
        {
            if (OracleSettings.Instance.ChangeZonesEnabled && OracleManager.ZoneChangeNeeded())
            {
                if (Core.Player.InCombat)
                {
                    return true;
                }

                Logger.SendLog("Zone change is needed.");
                await HandleZoneChange();
                return true;
            }

            await OracleCore();
            return true;
        }

        private static async Task<bool> HandleZoneChange()
        {
            uint aetheryteId = 0;
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

            if (OracleSettings.Instance.BindHomePoint)
            {
                await BindHomePoint.Main();
            }

            return true;
        }

        private static async Task<bool> Main()
        {
            OracleManager.UpdateGameCache();

            if (OracleManager.OracleDatabase == null)
            {
                await BuildOracleDatabase.Main();
            }

            if (Poi.Current == null)
            {
                Poi.Current = new Poi(Vector3.Zero, PoiType.None);

                return false;
            }

            if (Poi.Current.Type == PoiType.Death || Oracle.DeathFlag)
            {
                if (Poi.Current.Type == PoiType.Death)
                {
                    Logger.SendLog("We died, attempting to recover.");
                    Oracle.DeathFlag = true;
                }
                else if (Oracle.DeathFlag)
                {
                    await DeathHandler.HandleDeath();
                    Oracle.DeathFlag = false;
                }

                return false;
            }

            switch (OracleSettings.Instance.OracleOperationMode)
            {
                case OracleOperationMode.FateGrind:
                    await FateGrindMode();
                    break;
                case OracleOperationMode.SpecificFate:
                    await SpecificFateMode();
                    break;
                case OracleOperationMode.AtmaGrind:

                    // TODO: Implement.
                    await FateGrindMode();
                    break;
                case OracleOperationMode.ZetaGrind:

                    // TODO: Implement.
                    await FateGrindMode();
                    break;
                case OracleOperationMode.AnimaGrind:

                    // TODO: Implement.
                    await FateGrindMode();
                    break;
            }

            // Always return false to not block the tree.
            return false;
        }

        private static async Task<bool> OracleCore()
        {
            switch (Poi.Current.Type)
            {
                case PoiType.Kill:
                    await CombatHandler.HandleCombat();
                    break;
                case PoiType.Fate:
                    await FateHandler.HandleFate();
                    break;
                case PoiType.Wait:
                    await WaitHandler.HandleWait();
                    break;
            }

            return true;
        }

        private static async Task<bool> SpecificFateMode()
        {
            await OracleCore();
            return true;
        }
    }
}