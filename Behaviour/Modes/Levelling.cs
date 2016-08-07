using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Behaviour.Tasks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    public class Levelling
    {
        public static async Task<bool> Main()
        {
            if (Poi.Current.Type == PoiType.Kill)
            {
                await CombatHandler.HandleCombat();
                return true;
            }

            if (OracleClassManager.FinishedLevelling())
            {
                Logger.SendLog("We've reached level " + ClassSettings.Instance.MaxLevel + " on our current class. Stopping Oracle.");
                OracleBot.StopOracle("We're done!");
                return true;
            }

            if (OracleClassManager.ZoneChangeNeeded())
            {
                if (Core.Player.InCombat)
                {
                    return true;
                }

                Logger.SendLog("Zone change is needed.");
                await ZoneChange.HandleZoneChange();
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