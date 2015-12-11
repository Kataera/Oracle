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

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;

using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Helpers;
using Tarot.Managers;

using TreeSharp;

namespace Tarot.Behaviour
{
    internal static class TarotBehaviour
    {
        public static Composite Behaviour
        {
            get { return CreateBehaviour(); }
        }

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        private static async Task<bool> HandleCombat()
        {
            return true;
        }

        private static async Task<bool> HandleFate()
        {
            if (GameObjectManager.Attackers.Any() && !Core.Player.IsMounted)
            {
                Logger.SendDebugLog("Clearing FATE point of interest while we're in combat.");
                Poi.Clear("We're being attacked.");

                return true;
            }

            return true;
        }

        private static async Task<bool> HandleWait()
        {
            if (GameObjectManager.Attackers.Any() && !Core.Player.IsMounted)
            {
                Logger.SendDebugLog("Clearing wait point of interest while we're in combat.");
                Poi.Clear("We're being attacked.");

                return true;
            }

            if (await TarotFateManager.AnyViableFates())
            {
                Logger.SendLog("Detected a viable FATE, exiting wait behaviour.");
                Poi.Clear("Viable FATE detected.");

                return true;
            }

            return true;
        }

        private static async Task<bool> Main()
        {
            if (Tarot.FateDatabase == null)
            {
                await BuildFateDatabase.Main();
            }

            // Safety check.
            if (Poi.Current == null)
            {
                return false;
            }

            switch (Poi.Current.Type)
            {
                case PoiType.Kill:
                    await HandleCombat();
                    break;

                case PoiType.Fate:
                    await HandleFate();
                    break;

                case PoiType.Wait:
                    await HandleWait();
                    break;
            }

            // Always return false to not block the tree.
            return false;
        }
    }
}