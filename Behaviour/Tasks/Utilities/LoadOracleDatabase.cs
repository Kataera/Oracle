using System.Threading.Tasks;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class LoadOracleDatabase
    {
        public static async Task<bool> Main()
        {
            // Make sure we actually need to populate the data, since XML parsing is very expensive.
            if (OracleFateManager.OracleDatabase != null)
            {
                return true;
            }

            Logger.SendLog("Loading Oracle's FATE database.");
            OracleFateManager.OracleDatabase = await XmlParser.GetFateDatabase(true);
            Logger.SendLog("Oracle's FATE database loaded successfully.");

            return true;
        }
    }
}