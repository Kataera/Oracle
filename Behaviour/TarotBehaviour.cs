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

using System.Threading.Tasks;

using Tarot.Behaviour.Tasks;
using Tarot.Behaviour.Tasks.Utilities;

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

        private static async Task<bool> Main()
        {
            // Check that the FATE database has been populated.
            if (Tarot.FateDatabase == null)
            {
                await BuildFateDatabase.Main();
            }

            await CombatHandler.Main();
            await FateHandler.Main();
            await IdleHandler.Main();

            // Always return false to not block the tree.
            return false;
        }
    }
}