using System.Threading.Tasks;

using Oracle.Behaviour.BotBase.Handlers;
using Oracle.Managers;

using TreeSharp;

namespace Oracle.Behaviour
{
    internal static class OracleBehaviour
    {
        internal static Composite Behaviour => CreateBehaviour();

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        private static async Task<bool> Main()
        {
            if (OracleFateManager.OracleFateData == null)
            {
                await OracleFateManager.LoadFateData();
            }

            await ChocoboHandler.Main();
            await FateHandler.Main();
            await WaitHandler.Main();

            // Always return false to not block the tree.
            return false;
        }
    }
}