using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Managers;

using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Modes
{
    internal class MultiLevelling
    {
        internal static async Task<bool> HandleMultiLevelling()
        {
            if (!Core.Player.InCombat && OracleClassManager.NoClassesEnabled())
            {
                Logger.SendErrorLog("You haven't enabled any classes for levelling. Ensure at least one class is enabled and then restart Oracle.");
                OracleBot.StopOracle("No classes enabled.");
            }
            else if (!Core.Player.InCombat && OracleClassManager.FinishedLevelling())
            {
                Logger.SendLog("We've reached level " + ClassSettings.Instance.MaxLevel + " with all enabled classes! Stopping Oracle.");
                await OracleTeleportManager.TeleportToClosestCity();
                OracleBot.StopOracle("We are done!");
            }
            else if (OracleClassManager.ClassChangeNeeded())
            {
                if (Core.Player.InCombat || GameObjectManager.Attackers.Any())
                {
                    return true;
                }

                Logger.SendLog("Class change is needed.");
                var changeClassResult = await OracleClassManager.ChangeClassJob(OracleClassManager.GetLowestLevelClassJob());
                if (changeClassResult == ChangeClassResult.NoGearset || changeClassResult == ChangeClassResult.NonCombatClass)
                {
                    OracleBot.StopOracle("Problem swapping classes.");
                }
            }
            else if (OracleClassManager.ZoneChangeNeeded())
            {
                if (Core.Player.InCombat || GameObjectManager.Attackers.Any())
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