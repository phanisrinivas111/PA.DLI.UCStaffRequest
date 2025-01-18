using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PA.DLI.UCStaffRequest.DataAccess.DataObjects
{
    public class ErrorLogBO
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Message { get; set; }
        public string Additional_Info { get; set; }
        public DateTime DateCreated { get; set; }
    }
}