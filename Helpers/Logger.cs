/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

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

using System.Windows.Media;

using ff14bot.Helpers;

using Oracle.Settings;

namespace Oracle.Helpers
{
    internal static class Logger
    {
        private static readonly Color LoggerDebugColour = Color.FromRgb(238, 223, 88);

        private static readonly Color LoggerErrorColour = Color.FromRgb(255, 25, 117);

        private static readonly Color LoggerRegularColour = Color.FromRgb(179, 179, 255);

        internal static void SendDebugLog(string log)
        {
            var prefix = "[Oracle v" + Oracle.Version + "] [DEBUG]: ";

            if (!OracleSettings.Instance.DebugEnabled)
            {
                Logging.WriteQuiet(LoggerDebugColour, prefix + log);
            }
            else
            {
                Logging.Write(LoggerDebugColour, prefix + log);
            }
        }

        internal static void SendErrorLog(string log)
        {
            var prefix = "[Oracle v" + Oracle.Version + "] [ERROR]: ";
            Logging.Write(LoggerErrorColour, prefix + log);
        }

        internal static void SendLog(string log)
        {
            var prefix = "[Oracle v" + Oracle.Version + "]: ";
            Logging.Write(LoggerRegularColour, prefix + log);
        }
    }
}