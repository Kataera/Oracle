using System.Threading.Tasks;

using ff14bot.Helpers;

namespace Oracle.Behaviour.BotBase.Handlers
{
    internal static class WaitHandler
    {
        internal static async Task<bool> Main()
        {
            if (Poi.Current.Type != PoiType.Wait)
            {
                return true;
            }

            return true;
        }
    }
}