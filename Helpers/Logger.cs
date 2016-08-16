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
        private static readonly Color LoggerWarningColour = Color.FromRgb(246, 136, 32);
        private static string lastLog;

        internal static void SendDebugLog(string log)
        {
            log = "[Oracle v" + OracleBot.Version + "] [DEBUG]: " + log;

            if (log.Equals(lastLog))
            {
                return;
            }

            if (!MainSettings.Instance.ShowDebugInConsole)
            {
                Logging.WriteQuiet(LoggerDebugColour, log);
            }
            else
            {
                Logging.WriteVerbose(LoggerDebugColour, log);
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

        internal static void SendWarningLog(string log)
        {
            log = "[Oracle v" + OracleBot.Version + "] [WARNING]: " + log;

            if (log.Equals(lastLog))
            {
                return;
            }

            Logging.Write(LoggerWarningColour, log);
            lastLog = log;
        }
    }
}