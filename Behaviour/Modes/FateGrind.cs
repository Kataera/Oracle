﻿using System.Threading.Tasks;

using ff14bot.Helpers;

using Oracle.Behaviour.Tasks;
using Oracle.Managers;

namespace Oracle.Behaviour.Modes
{
    public class FateGrind
    {
        public static async Task<bool> Main()
        {
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