using System;
using System.IO;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Settings;

using Newtonsoft.Json;

using Oracle.Data;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class LoadOracleDatabase
    {
        private static async Task<OracleDatabase> DeserialiseFateData()
        {
            var path = GlobalSettings.Instance.BotBasePath + @"\Oracle\Data\Fates\FateData.json";

            if (File.Exists(path))
            {
                using (var databaseFileStream = new FileStream(path, FileMode.Open))
                {
                    using (var sr = new StreamReader(databaseFileStream))
                    {
                        Logger.SendDebugLog("Reading the database file.");
                        var json = await Coroutine.ExternalTask(sr.ReadToEndAsync());

                        Logger.SendDebugLog("Deserialising the database file.");
                        var oracleDatabase = await Coroutine.ExternalTask(Task.Factory.StartNew(() => JsonConvert.DeserializeObject<OracleDatabase>(json)));

                        return oracleDatabase;
                    }
                }
            }

            Logger.SendErrorLog("Could not find Oracle's FATE database. Oracle will likely not work correctly without it.");
            return null;
        }

        public static async Task<bool> Main()
        {
            if (OracleFateManager.OracleDatabase != null)
            {
                return true;
            }

            Logger.SendLog("Loading Oracle's FATE database.");
            OracleFateManager.OracleDatabase = await DeserialiseFateData();
            Logger.SendLog("Oracle's FATE database loaded successfully.");

            return true;
        }

        private static async Task SerialiseFateData()
        {
            Logger.SendDebugLog("Parsing XML data for conversion.");
            var fateDb = await XmlParser.GetFateDatabase();

            try
            {
                Logger.SendDebugLog("Generating FateData.json");
                var json = JsonConvert.SerializeObject(fateDb, Formatting.None);
                File.WriteAllText(GlobalSettings.Instance.BotBasePath + @"\Oracle\Data\Fates\FateData.json", json);
            }
            catch (Exception e)
            {
                Logger.SendErrorLog("Could not save data. Exception:\n" + e);
            }
        }
    }
}