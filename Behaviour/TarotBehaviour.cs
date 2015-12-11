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

using Tarot.Behaviour.Tasks.Fates;
using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Enumerations;
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

        private static void ClearPoi(string reason)
        {
            Logger.SendLog(reason);
            Poi.Clear(reason);
        }

        private static void ClearPoi(string reason, bool sendLog)
        {
            if (sendLog)
            {
                Logger.SendLog(reason);
            }

            Poi.Clear(reason);
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
            if (Core.Player.Distance(TarotFateManager.CurrentFate.Location) > TarotFateManager.CurrentFate.Radius)
            {
                await MoveToFate.Main();
            }

            if (TarotFateManager.CurrentFate == null)
            {
                return false;
            }

            if (GameObjectManager.Attackers.Any() && !Core.Player.IsMounted)
            {
                ClearPoi("We're being attacked.", false);
                return true;
            }

            return await FateRunner.Main();
        }

        private static async Task<bool> HandleWait()
        {
            if (GameObjectManager.Attackers.Any() && !Core.Player.IsMounted)
            {
                ClearPoi("We're being attacked.", false);
                return true;
            }

            if (await TarotFateManager.AnyViableFates())
            {
                ClearPoi("Viable FATE detected.");
                return true;
            }

            return true;
        }

        private static async Task<bool> Main()
        {
            if (TarotFateManager.FateDatabase == null)
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