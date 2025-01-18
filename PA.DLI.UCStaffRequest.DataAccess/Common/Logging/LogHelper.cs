using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.Common.Logging
{
    public static class LogHelper
    {
        private static readonly string logFilePath = "path_to_log_file/log.txt";

        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public static void LogError(string message, Exception ex)
        {
            Log("ERROR", $"{message}\nException: {ex}");
        }

        public static void LogDebug(string message)
        {
            Log("DEBUG", message);
        }

        private static void Log(string logLevel, string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";
            try
            {
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Handle logging failure, e.g., write to event viewer or system logs
            }
        }
    }
}
