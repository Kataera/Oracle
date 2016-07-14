using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Helpers;

namespace Oracle.Behaviour.PoiHooks.WaitSelect
{
    internal static class WaitInPlace
    {
        public static async Task<bool> Main()
        {
            Logger.SendLog("Waiting in place for a viable FATE.");
            Poi.Current = new Poi(Core.Player.Location, PoiType.Wait);

            return true;
        }
    }
}