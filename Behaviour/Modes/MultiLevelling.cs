using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

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

            if (OracleClassManager.NoClassesEnabled())
            {
                Logger.SendErrorLog("You haven't enabled any classes for levelling. Ensure at least one class is enabled and then restart Oracle.");
                OracleBot.StopOracle("No classes enabled.");
            }

            if (OracleClassManager.FinishedLevelling())
            {
                Logger.SendLog("All enabled classes have reached level " + ClassSettings.Instance.MaxLevel + ". Stopping Oracle.");
                OracleBot.StopOracle("We're done!");
            }

            if (OracleClassManager.ClassChangeNeeded())
            {
                if (Core.Player.InCombat || GameObjectManager.Attackers.Any())
                {
                    return true;
                }

                Logger.SendLog("Class change is needed.");
                var changeClassResult = await ChangeClass.Main(OracleClassManager.GetLowestLevelClassJob());
                if (changeClassResult == ChangeClassResult.NoGearset || changeClassResult == ChangeClassResult.NonCombatClass)
                {
                    OracleBot.StopOracle("Problem swapping classes.");
                }

                return true;
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