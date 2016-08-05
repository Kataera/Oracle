using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Modes
{
    public class MultiLevelling
    {
        public static async Task<bool> Main()
        {
            if (Poi.Current.Type == PoiType.Kill)
            {
                await CombatHandler.HandleCombat();
                return true;
            }

            if (OracleClassManager.ClassChangeNeeded())
            {
                if (Core.Player.InCombat || GameObjectManager.Attackers.Any())
                {
                    return true;
                }

                Logger.SendLog("Class change is needed.");
                if (await ChangeClass.Main(OracleClassManager.GetLowestLevelClassJob()) != ChangeClassResult.Success)
                {
                    OracleBot.StopOracle("Problem swapping classes.");
                }
            }

            if (OracleClassManager.ZoneChangeNeeded())
            {
                if (Core.Player.InCombat || GameObjectManager.Attackers.Any())
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