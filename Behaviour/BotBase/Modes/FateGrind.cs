using System;
using System.Threading.Tasks;

using Oracle.Behaviour.BotBase.Fates;
using Oracle.Enumerations;
using Oracle.Managers;

namespace Oracle.Behaviour.BotBase.Modes
{
    internal class FateGrind
    {
        internal static async Task<bool> Main()
        {
            await RunFateType();
            return true;
        }

        private static async Task RunFateType()
        {
            switch (OracleFateManager.CurrentFate.Type)
            {
                case FateType.Kill:
                    await KillFate.Main();
                    break;
                case FateType.Boss:
                    await BossFate.Main();
                    break;
                case FateType.MegaBoss:
                    await MegaBossFate.Main();
                    break;
                case FateType.Escort:
                    await EscortFate.Main();
                    break;
                case FateType.Defence:
                    await DefenceFate.Main();
                    break;
                case FateType.Collect:
                    await CollectFate.Main();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}