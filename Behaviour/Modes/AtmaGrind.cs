using System.Threading.Tasks;

using Oracle.Helpers;

namespace Oracle.Behaviour.Modes
{
    internal static class AtmaGrind
    {
        public static async Task<bool> Main()
        {
            Logger.SendErrorLog("Atma grind mode is not yet implemented.");
            OracleBot.StopOracle("Atma grind mode is not yet implemented.");

            return true;
        }
    }
}