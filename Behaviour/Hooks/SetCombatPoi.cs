using System.Linq;
using System.Threading.Tasks;

using ff14bot.Helpers;
using ff14bot.Managers;

using Oracle.Helpers;

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
                Logger.SendDebugLog("We're being attacked, picking a target to set as kill Poi.");
                var target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault(attacker => GameObjectManager.Attackers.Contains(attacker));

                if (target == null)
                {
                    Logger.SendErrorLog("Couldn't find a target to set as kill Poi.");
                    return false;
                }

                Logger.SendDebugLog("Setting " + target.Name + " (" + target.ObjectId + ") as kill Poi.");
                Poi.Current = new Poi(target, PoiType.Kill);
                return true;
            }

            return false;
        }
    }
}