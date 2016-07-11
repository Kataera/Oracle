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
        private static string LastLog;

        internal static void SendDebugLog(string log)
        {
            log = "[Oracle v" + OracleBot.Version + "] [DEBUG]: " + log;

            if (log.Equals(LastLog))
            {
                return;
            }

            if (!OracleSettings.Instance.DebugEnabled)
            {
                Logging.WriteQuiet(LoggerDebugColour, log);
            }
            else
            {
                Logging.Write(LoggerDebugColour, log);
            }

            LastLog = log;
        }

        internal static void SendErrorLog(string log)
        {
            log = "[Oracle v" + OracleBot.Version + "] [ERROR]: " + log;

            if (log.Equals(LastLog))
            {
                return;
            }

            Logging.Write(LoggerErrorColour, log);
            LastLog = log;
        }

        internal static void SendLog(string log)
        {
            log = "[Oracle v" + OracleBot.Version + "]: " + log;

            if (log.Equals(LastLog))
            {
                return;
            }

            Logging.Write(LoggerRegularColour, log);
            LastLog = log;
        }
    }
}