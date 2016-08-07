using System.Threading.Tasks;

using ff14bot;

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
            if (!Core.Player.InCombat && OracleClassManager.FinishedLevelling())
            {
                Logger.SendLog("We've reached level " + ClassSettings.Instance.MaxLevel + " on our current class! Stopping Oracle.");
                await OracleTeleportManager.TeleportToClosestCity();
                OracleBot.StopOracle("We are done!");
            }
            else if (OracleClassManager.ZoneChangeNeeded())
            {
                if (Core.Player.InCombat)
                {
                    return true;
                }

                Logger.SendLog("Zone change is needed.");
                await ZoneChange.HandleZoneChange();
            }

            return true;
        }
    }
}