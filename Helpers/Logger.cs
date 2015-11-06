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

namespace Tarot.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Media;

    using ff14bot.Helpers;

    using global::Tarot.Settings;

    internal static class Logger
    {
        private static readonly Color LoggerRegularColour = Color.FromRgb(179, 179, 255);

        private static readonly Color LoggerErrorColour = Color.FromRgb(255, 25, 117);

        private static readonly Color LoggerDebugColour = Color.FromRgb(255, 153, 0);

        private static readonly Assembly Assembly = System.Reflection.Assembly.GetExecutingAssembly();

        private static readonly Version Version = Assembly.GetName().Version;

        internal static void SendLog(string log)
        {
            var prefix = "[" + Tarot.BotName + " v" + Version.ToString() + "]: ";
            Logging.Write(LoggerRegularColour, prefix + log);
        }

        internal static void SendErrorLog(string log)
        {
            var prefix = "[" + Tarot.BotName + " v" + Version.ToString() + "] [ERROR]: ";
            Logging.Write(LoggerErrorColour, prefix + log);
        }

        internal static void SendDebugLog(string log)
        {
            if (TarotSettings.Instance.DebugEnabled)
            {
                var prefix = "[" + Tarot.BotName + " v" + Version.ToString() + "] [DEBUG]: ";
                Logging.Write(LoggerDebugColour, prefix + log);
            }
        }
    }
}