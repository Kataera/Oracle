/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.RemoteWindows;

using Tarot.Helpers;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.Utilities
{
    internal static class BindHomePoint
    {
        public static async Task<bool> Main()
        {
            Logger.SendLog("Binding to the aetheryte crystal.");
            var aetheryteObject = GameObjectManager.GameObjects.FirstOrDefault(obj => obj.Type == GameObjectType.AetheryteObject);

            if (aetheryteObject == null)
            {
                Logger.SendErrorLog("Could not find an aetheryte crystal.");
                return false;
            }

            while (aetheryteObject.Distance(Core.Player) > 10f)
            {
                Navigator.MoveTo(aetheryteObject.Location, "Aetheryte crystal.");
                await Coroutine.Yield();
            }

            Navigator.Stop();

            aetheryteObject.Interact();
            await Coroutine.Sleep(TarotSettings.Instance.ActionDelay);
            SelectString.ClickLineContains("Set Home Point");
            await Coroutine.Sleep(TarotSettings.Instance.ActionDelay);
            SelectYesno.ClickYes();
            await Coroutine.Sleep(TarotSettings.Instance.ActionDelay);

            Logger.SendLog("Home point bound successfully.");

            return true;
        }
    }
}