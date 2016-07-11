using System.Threading.Tasks;

using ff14bot;

using Oracle.Helpers;

namespace Oracle.Behaviour.Modes
{
    internal static class AnimusGrind
    {
        public static async Task<bool> Main()
        {
            Logger.SendErrorLog("Animus grind mode is not yet implemented.");
            TreeRoot.Stop("Animus grind mode is not yet implemented.");

            return true;
        }
    }
}