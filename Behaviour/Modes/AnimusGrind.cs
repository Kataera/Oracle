using System.Threading.Tasks;

using Oracle.Helpers;

namespace Oracle.Behaviour.Modes
{
    internal static class AnimusGrind
    {
        internal static async Task<bool> HandleAnimusGrind()
        {
            Logger.SendErrorLog("Animus grind mode is not yet implemented.");
            OracleBot.StopOracle("Animus grind mode is not yet implemented.");

            return true;
        }
    }
}