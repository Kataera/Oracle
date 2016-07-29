using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class BindHomePoint
    {
        private static GameObject GetAetheryteObject(uint aetheryteId)
        {
            return GameObjectManager.GameObjects.FirstOrDefault(obj => obj.Type == GameObjectType.AetheryteObject && obj.NpcId == aetheryteId);
        }

        public static async Task<bool> Main(uint aetheryteId)
        {
            Logger.SendLog("Binding to the aetheryte crystal.");
            var aetheryteObject = GetAetheryteObject(aetheryteId);

            if (aetheryteObject == null)
            {
                Logger.SendErrorLog("Could not find an aetheryte crystal.");
                return false;
            }

            if (!WorldManager.CanFly)
            {
                while (aetheryteObject.Distance(Core.Player) > 8f)
                {
                    Navigator.MoveTo(aetheryteObject.Location, "Aetheryte crystal.");
                    await Coroutine.Yield();
                }

                Navigator.Stop();
            }
            else
            {
                while (aetheryteObject.Distance(Core.Player) > 8f)
                {
                    Core.Player.Face(aetheryteObject);
                    Navigator.PlayerMover.MoveTowards(aetheryteObject.Location);
                    await Coroutine.Yield();
                }

                Navigator.PlayerMover.MoveStop();
            }

            aetheryteObject.Interact();
            await Coroutine.Sleep(MainSettings.Instance.ActionDelay);
            SelectString.ClickLineContains("Set Home Point");
            await Coroutine.Sleep(MainSettings.Instance.ActionDelay);
            SelectYesno.ClickYes();
            await Coroutine.Sleep(MainSettings.Instance.ActionDelay);

            Logger.SendLog("Home point bound successfully.");

            return true;
        }
    }
}