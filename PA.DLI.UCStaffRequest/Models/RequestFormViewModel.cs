using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PA.DLI.UCStaffRequest.Models
{
    public class RequestFormViewModel
    {
        public int Category { get; set; }
        public string FromEmail { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public List<HttpPostedFileBase> files { get; set; }
    }
}