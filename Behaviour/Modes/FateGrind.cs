﻿using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Behaviour.Tasks;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Modes
{
    public class FateGrind
    {
        public static async Task<bool> Main()
        {
            if (OracleFateManager.ZoneChangeNeeded())
            {
                if (Core.Player.InCombat)
                {
                    return true;
                }

                Logger.SendLog("Zone change is needed.");
                await ZoneChangeHandler.HandleZoneChange();
                return true;
            }

            if (Poi.Current.Type == PoiType.Kill)
            {
                await CombatHandler.HandleCombat();
            }

            else if (Poi.Current.Type == PoiType.Fate || OracleFateManager.CurrentFateId != 0)
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