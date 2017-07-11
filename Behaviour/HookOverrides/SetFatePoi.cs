using System.Threading.Tasks;

using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Managers;

namespace Oracle.Behaviour.HookOverrides
{
    internal static class SetFatePoi
    {
        internal static async Task<bool> Main()
        {
            if (CommonBehaviors.IsLoading)
            {
                return false;
            }

            if (!OracleFateManager.IsFateSet())
            {
                if (!OracleFateManager.IsFateAvailable())
                {
                    return false;
                }

                await OracleFateManager.SelectFate();
            }

            if (Poi.Current.Type == PoiType.None || Poi.Current.Type == PoiType.Wait)
            {
                var fate = FateManager.GetFateById(OracleFateManager.CurrentFateId);
                Poi.Current = new Poi(fate, PoiType.Fate);
            }

            return false;
        }
    }
}