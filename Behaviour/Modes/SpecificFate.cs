using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Behaviour.Tasks;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    internal static class SpecificFate
    {
        public static async Task<bool> Main()
        {
            if (OracleSettings.Instance.SpecificFateName == string.Empty)
            {
                Logger.SendErrorLog("Please set a specific FATE before starting the bot.");
                TreeRoot.Stop("No FATE set.");
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