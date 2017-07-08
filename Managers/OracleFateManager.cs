using System.Globalization;
using System.Linq;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleFateManager
    {
        public static uint CurrentFate { get; private set; }

        public static void ClearFate()
        {
            CurrentFate = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                Poi.Clear("FATE cleared.");
            }
        }

        public static bool IsFateAvailable()
        {
            return FateManager.AllFates.Any(ViableFate);
        }

        public static bool IsFateSet()
        {
            return CurrentFate != 0;
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