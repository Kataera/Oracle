using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Managers;

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

            if (Core.Player.Distance2D(OracleFateManager.GameFateData.Location) > OracleFateManager.GameFateData.Radius * 0.8)
            {
                await OracleNavigationManager.NavigateTo(OracleFateManager.GameFateData.Location);
            }

            return true;
        }
    }
}