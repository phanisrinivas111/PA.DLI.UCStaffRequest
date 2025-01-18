using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.Common.Logging
{
    public class LoggerManager
    {
        protected LogWriter logWriter;
        private static LoggerManager instance = null;

        public LoggerManager()
        {
            InitLogging();
        }

        private void InitLogging()
        {
            try
            {
                logWriter = new LogWriterFactory().Create();  // Creates the log writer instance
                Logger.SetLogWriter(logWriter, false);         // Setting the log writer for logging
            }
            catch (Exception ex)
            {
                // Handle the initialization error
                Console.WriteLine($"Error initializing logger: {ex.Message}");
            }
        }

        public LogWriter LogWriter
        {
            get
            {
                return logWriter;
            }
        }

        public static LoggerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoggerManager();
                }
                return instance;
            }
        }

        public void Log(string message, int priority, TraceEventType eventType)
        {
            try
            {
                if (logWriter != null)
                {
                    logWriter.Write(message, "Messaging", priority, 2000, eventType);
                }
                else
                {
                    Console.WriteLine("LogWriter is not initialized.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing log: {ex.Message}");
            }
        }
        // New LogError method
        public void LogError(string controllerName, string methodName, string message, string stackTrace, string parameters)
        {
            string logMessage = $"Error in {controllerName}Controller. Method: {methodName}, Message: {message}, StackTrace: {stackTrace}, Parameters: {parameters}";
           
        }
    }
}
