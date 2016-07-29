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
        private static string lastLog;

        internal static void SendDebugLog(string log)
        {
            log = "[Oracle v" + OracleBot.Version + "] [DEBUG]: " + log;

            if (log.Equals(lastLog))
            {
                return;
            }

            if (!MainSettings.Instance.DebugEnabled)
            {
                Logging.WriteQuiet(LoggerDebugColour, log);
            }
            else
            {
                Logging.Write(LoggerDebugColour, log);
            }

            lastLog = log;
        }

        internal static void SendErrorLog(string log)
        {
            log = "[Oracle v" + OracleBot.Version + "] [ERROR]: " + log;

            if (log.Equals(lastLog))
            {
                return;
            }

            Logging.Write(LoggerErrorColour, log);
            lastLog = log;
        }

        internal static void SendLog(string log)
        {
            log = "[Oracle v" + OracleBot.Version + "]: " + log;

            if (log.Equals(lastLog))
            {
                return;
            }

            Logging.Write(LoggerRegularColour, log);
            lastLog = log;
        }
    }
}