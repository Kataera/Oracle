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

using Tarot.Helpers;
using Tarot.Managers;

namespace Tarot.Behaviour.Tasks.Utilities
{
    internal static class BuildTarotDatabase
    {
        public static async Task<bool> Main()
        {
            // Make sure we actually need to populate the data, since XML parsing is very expensive.
            if (TarotFateManager.TarotDatabase != null)
            {
                return true;
            }

            Logger.SendLog("Building Tarot's FATE database, this may take a few seconds.");
            TarotFateManager.TarotDatabase = XmlParser.GetFateDatabase(true);
            Logger.SendLog("Tarot's FATE database has been built successfully.");

            return true;
        }
    }
}