using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Settings;

using Newtonsoft.Json;

using Oracle.Data;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleFateManager
    {
        public static uint CurrentFate { get; private set; }

        public static OracleFateData OracleFateData { get; private set; }

        public static void ClearFate()
        {
            CurrentFate = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                Poi.Clear("FATE cleared.");
            }
        }

        private static async Task<OracleFateData> DeserialiseOracleFateData()
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
                        var oracleDatabase = await Coroutine.ExternalTask(Task.Factory.StartNew(() => JsonConvert.DeserializeObject<OracleFateData>(json)));

                        return oracleDatabase;
                    }
                }
            }

            Logger.SendErrorLog("Could not find Oracle's FATE database. Oracle will likely not work correctly without it.");
            return null;
        }

        public static bool IsFateAvailable()
        {
            return FateManager.AllFates.Any(ViableFate);
        }

        public static bool IsFateSet()
        {
            return CurrentFate != 0;
        }

        public static async Task<bool> LoadFateData()
        {
            if (OracleFateData != null)
            {
                return true;
            }

            OracleFateData = await DeserialiseOracleFateData();
            return true;
        }

        public static void SelectFate()
        {
            Logger.SendLog("Selecting a FATE.");
            var fates = FateManager.AllFates.Where(ViableFate).OrderBy(f => Core.Player.Location.Distance2D(f.Location));

            if (fates.Any())
            {
                Logger.SendDebugLog("FATEs found:");
                foreach (var fate in fates)
                {
                    var fateDistance = Core.Player.Location.Distance2D(fate.Location).ToString(CultureInfo.InvariantCulture);
                    Logger.SendDebugLog($"\"{fate.Name}\" found at {fateDistance} yalms away.");
                }
            }

            // Select closest FATE.
            var potentialFate = fates.FirstOrDefault();
            if (potentialFate != null)
            {
                Logger.SendLog($"Selected \"{potentialFate.Name}\" as the next FATE.");
                CurrentFate = potentialFate.Id;
            }
        }

        private static bool ViableFate(FateData fate)
        {
            if (OracleSettings.Instance.BlacklistedFates.Contains(fate.Id))
            {
                return false;
            }

            return true;
        }
    }
}