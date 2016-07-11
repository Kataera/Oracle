using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Managers;
using ff14bot.Navigation;

using Oracle.Helpers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class Mount
    {
        public static async Task<bool> MountUp()
        {
            if (!Actionmanager.AvailableMounts.Any())
            {
                Logger.SendDebugLog("Character does not have any mount available, skipping mount task.");
                return true;
            }

            if (MovementManager.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }

            while (!Core.Player.IsMounted)
            {
                if (GameObjectManager.Attackers.Any())
                {
                    return false;
                }

                if (Actionmanager.CanMount == 0)
                {
                    Actionmanager.Mount();
                }

                await Coroutine.Yield();
            }

            return true;
        }
    }
}