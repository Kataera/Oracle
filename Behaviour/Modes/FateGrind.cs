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

using Oracle.Behaviour.Tasks;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    public class FateGrind
    {
        public static async Task<bool> Main()
        {
            if (OracleSettings.Instance.ChangeZonesEnabled && OracleManager.ZoneChangeNeeded())
            {
                if (Core.Player.InCombat)
                {
                    return true;
                }

                Logger.SendLog("Zone change is needed.");
                await ZoneChangeHandler.HandleZoneChange();
                return true;
            }

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
    }
}