using PA.DLI.UCStaffRequest.DataAccess.DataAccess;
using PA.DLI.UCStaffRequest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace PA.DLI.UCStaffRequest.Common.Util
{
    public class ErrorLogging
    {
        private static PA.DLI.UCStaffRequest.DataAccess.DataObjects.ErrorLogBO MapToDataObject(PA.DLI.UCStaffRequest.Models.ErrorLogBO modelErrorLog)
        {
            return new PA.DLI.UCStaffRequest.DataAccess.DataObjects.ErrorLogBO
            {
                ControllerName = modelErrorLog.ControllerName,
                ActionName = modelErrorLog.ActionName,
                Message = modelErrorLog.Message,
                Additional_Info = modelErrorLog.Additional_Info,
                DateCreated = modelErrorLog.DateCreated
            };
        }
        public static void LogWritter(string controllerName, string actionName, string message, string additional)
        {
            try
            {
                var  errorLog = new PA.DLI.UCStaffRequest.Models.ErrorLogBO
                {
                    ControllerName = controllerName,
                    ActionName = actionName,
                    Message = message,
                    Additional_Info = additional,
                    DateCreated = DateTime.Now

                };

                var dataErrorLog = MapToDataObject(errorLog);
  
                var result = new ErrorLogDataAccess().ErrorInfo(dataErrorLog);
            }
            catch (Exception ex)
            {
                var folderPath = (ConfigurationManager.AppSettings["LogFolder"]).ToString();
                string fileName = ConfigurationManager.AppSettings["LogFile"].ToString();
                Directory.CreateDirectory(folderPath);
                string filePath = folderPath + fileName + "_" + DateTime.Now.ToString("MMddyyyy") + ".txt";
                if (!System.IO.File.Exists(filePath))
                {
                    var myFile = System.IO.File.Create(filePath);
                    myFile.Close();
                }
                FileStream fs = new FileStream(string.Format("{0}", filePath), FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter((Stream)fs);
                sw.WriteLine("Date: " + DateTime.Now);
                sw.WriteLine(ex.InnerException);
                sw.WriteLine("Error Message: " + ex.Message);
                sw.WriteLine("ControllerName: " + controllerName);
                sw.WriteLine("Message: " + message);
                sw.WriteLine("ActionName: " + actionName);
                sw.WriteLine("");
                sw.Close();
                fs.Close();
            }
        

        }
    }
}