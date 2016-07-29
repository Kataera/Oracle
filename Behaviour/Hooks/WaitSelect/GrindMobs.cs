using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;

namespace Oracle.Behaviour.Hooks.WaitSelect
{
    internal static class GrindMobs
    {
        public static async Task<bool> Main()
        {
            Poi.Current = new Poi(Core.Player.Location, PoiType.Wait);
            return true;
        }
    }
}