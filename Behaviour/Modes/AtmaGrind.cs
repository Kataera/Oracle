using System.Threading.Tasks;

using ff14bot;

using Oracle.Helpers;

namespace Oracle.Behaviour.Modes
{
    internal static class AtmaGrind
    {
        public static async Task<bool> Main()
        {
            Logger.SendErrorLog("Atma grind mode is not yet implemented.");
            TreeRoot.Stop("Atma grind mode is not yet implemented.");

            return true;
        }
    }
}