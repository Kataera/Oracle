using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.ServiceClient;
using ff14bot.Settings;

using Newtonsoft.Json;

using Oracle.Data;
using Oracle.Data.Structs;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleFateManager
    {
        internal static Fate CurrentFate
        {
            get
            {
                if (OracleFateData == null || CurrentFateId == 0)
                {
                    return new Fate();
                }

                return OracleFateData.GetFateData(CurrentFateId);
            }
        }

        internal static uint CurrentFateId { get; private set; }

        internal static FateData GameFateData => CurrentFateId == 0 ? null : FateManager.GetFateById(CurrentFateId);

        internal static OracleFateData OracleFateData { get; private set; }

        internal static void ClearFate()
        {
            CurrentFateId = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                Poi.Clear("FATE cleared.");
            }
        }

        internal static bool CurrentFateHasPreferredTarget()
        {
            return CurrentFate.PreferredTargetId != 0;
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

        internal static void FlushOracleFateData()
        {
            OracleFateData = null;
        }

        internal static bool IsCurrentFateValid()
        {
            if (CurrentFateId == 0)
            {
                return false;
            }

            var fateData = FateManager.GetFateById(CurrentFateId);
            if (fateData == null)
            {
                return false;
            }

            if (!fateData.IsValid && fateData.Status != FateStatus.PREPARING)
            {
                return false;
            }

            if (fateData.Progress >= 100f && CurrentFate.Type != FateType.Collect)
            {
                return false;
            }

            return true;
        }

        internal static bool IsFateAvailable()
        {
            return FateManager.AllFates.Any(ViableFate);
        }

        internal static bool IsFateSet()
        {
            return CurrentFateId != 0;
        }

        public static bool IsLevelSyncNeeded(uint fateId)
        {
            var fateData = FateManager.GetFateById(fateId);

            if (fateData == null || !fateData.IsValid)
            {
                return false;
            }

            return OracleClassManager.GetTrueLevel() > fateData.MaxLevel;
        }

        internal static async Task<bool> LoadFateData()
        {
            if (OracleFateData != null)
            {
                return true;
            }

            Logger.SendLog("Loading Oracle's FATE data, this may take a few seconds.");
            OracleFateData = await DeserialiseOracleFateData();

            if (OracleFateData == null)
            {
                return false;
            }

            Logger.SendLog($"Successfully loaded data on fates.");
            return true;
        }

        internal static async Task SelectFate()
        {
            Logger.SendLog("Selecting a FATE.");
            var fates = FateManager.AllFates.Where(ViableFate).OrderBy(f => Core.Player.Location.Distance2D(f.Location));

            var navRequest = fates.Select(fate => new CanFullyNavigateTarget
            {
                Id = fate.Id,
                Position = fate.Location
            }).ToList();
            var navResults = await OracleNavigationManager.AreLocationsNavigable(navRequest);

            var viableFates = fates.ToList();
            foreach (var fate in fates)
            {
                var result = navResults.Find(r => r.Id == fate.Id);
                if (result != null && result.CanNavigate == 0)
                {
                    viableFates.Remove(fate);
                }
            }

            if (viableFates.Any())
            {
                Logger.SendDebugLog("FATEs found:");
                foreach (var fate in viableFates)
                {
                    var fateDistance = Core.Player.Location.Distance2D(fate.Location).ToString(CultureInfo.InvariantCulture);
                    Logger.SendDebugLog($"\"{fate.Name}\" found at {fateDistance} yalms away.");
                }
            }

            // Select closest FATE.
            var potentialFate = viableFates.FirstOrDefault();
            if (potentialFate != null)
            {
                Logger.SendLog($"Selected \"{potentialFate.Name}\" as the next FATE.");
                CurrentFateId = potentialFate.Id;
            }
        }

        private static bool ViableFate(FateData fate)
        {
            if (OracleSettings.Instance.BlacklistedFates.Contains(fate.Id))
            {
                return false;
            }

            if (!fate.IsValid && fate.Status != FateStatus.PREPARING)
            {
                return false;
            }

            if (fate.Status == FateStatus.COMPLETE)
            {
                return false;
            }

            if (fate.Status == FateStatus.NOTACTIVE)
            {
                return false;
            }

            // Is FATE finished?
            if (Math.Abs(fate.Progress - 100f) < 0.1)
            {
                return false;
            }

            return true;
        }
    }
}