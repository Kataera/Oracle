using System.Threading.Tasks;

using Oracle.Helpers;

namespace Oracle.Behaviour.Modes
{
    internal static class AnimusGrind
    {
        public static async Task<bool> Main()
        {
            Logger.SendErrorLog("Animus grind mode is not yet implemented.");
            OracleBot.StopOracle("Animus grind mode is not yet implemented.");

            return true;
        }
    }
}