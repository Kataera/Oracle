using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Behaviour.BotBase.Modes;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.BotBase.Handlers
{
    internal static class FateHandler
    {
        internal static async Task<bool> Main()
        {
            if (Poi.Current.Type != PoiType.Fate)
            {
                return true;
            }

            if (!OracleFateManager.IsCurrentFateValid())
            {
                OracleFateManager.ClearFate();
                return true;
            }

            // Move to FATE if we're too far away.
            if (Core.Player.Distance2D(OracleFateManager.GameFateData.Location) > OracleFateManager.GameFateData.Radius * 0.8)
            {
                await OracleNavigationManager.NavigateToCurrentFate();
            }

            // Navigation can clear our FATE if it expires, check to see if it's still active.
            if (Poi.Current.Type != PoiType.Fate)
            {
                return true;
            }

            await RunFateMode();
            return true;
        }

        private static async Task RunFateMode()
        {
            switch (OracleSettings.Instance.OracleFateMode)
            {
                case OracleFateMode.FateGrind:
                    await FateGrind.Main();
                    break;
                case OracleFateMode.LevelMode:
                    await Levelling.Main();
                    break;
                case OracleFateMode.MultiLevelMode:
                    await MultiLevelling.Main();
                    break;
                case OracleFateMode.SpecificFates:
                    await SpecificFates.Main();
                    break;
                case OracleFateMode.AtmaGrind:
                    await AtmaGrind.Main();
                    break;
                case OracleFateMode.AnimusGrind:
                    await AnimusGrind.Main();
                    break;
                case OracleFateMode.AnimaGrind:
                    await AnimaGrind.Main();
                    break;
                default:
                    Logger.SendErrorLog("Could not determine operating mode. Defaulting to FATE grinding.");
                    await FateGrind.Main();
                    break;
            }
        }
    }
}