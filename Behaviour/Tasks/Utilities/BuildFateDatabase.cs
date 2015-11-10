﻿/*
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

namespace Tarot.Behaviour.Tasks.Utilities
{
    using System.Threading.Tasks;

    using global::Tarot.Helpers;

    internal static class BuildFateDatabase
    {
        public static async Task<bool> Task()
        {
            // Make sure we actually need to populate the data, since XML parsing is very expensive.
            if (Tarot.FateDatabase != null)
            {
                return true;
            }

            Logger.SendLog("Building " + Tarot.Instance.Name + "'s FATE database, this may take a few seconds.");
            Tarot.FateDatabase = XmlParser.GetFateDatabase();
            Logger.SendLog(Tarot.Instance.Name + "'s FATE database has been built successfully.");

            return true;
        }
    }
}