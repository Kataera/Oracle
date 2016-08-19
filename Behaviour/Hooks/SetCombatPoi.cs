using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;
using ff14bot.Managers;

namespace Oracle.Behaviour.Hooks
{
    internal static class SetCombatPoi
    {
        internal static async Task<bool> Main()
        {
            if (Poi.Current.Type != PoiType.None)
            {
                return false;
            }

            if (GameObjectManager.Attackers.Any())
            {
                var target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
                if (target == null)
                {
                    return false;
                }

                Poi.Current = new Poi(target, PoiType.Kill);
                return true;
            }

            return false;
        }
    }
}