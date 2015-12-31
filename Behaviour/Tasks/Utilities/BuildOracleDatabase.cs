/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

using System.Threading.Tasks;

using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class BuildOracleDatabase
    {
        public static async Task<bool> Main()
        {
            // Make sure we actually need to populate the data, since XML parsing is very expensive.
            if (OracleManager.OracleDatabase != null)
            {
                return true;
            }

            Logger.SendLog("Building Oracle's FATE database, this may take a few seconds.");
            OracleManager.OracleDatabase = XmlParser.GetFateDatabase(true);
            Logger.SendLog("Oracle's FATE database has been built successfully.");

            return true;
        }
    }
}