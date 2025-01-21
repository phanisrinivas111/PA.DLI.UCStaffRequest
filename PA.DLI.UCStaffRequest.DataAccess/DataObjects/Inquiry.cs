using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PA.DLI.UCStaffRequest.DataAccess.DataObjects
{
    public class Inquiry
    {
        public int TicketId {  get; set; }
        public int Category { get; set; }
        public string FromEmail { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime LastChangeDate { get; set; }
        public string LastChangeUser { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<HttpPostedFileBase> files { get; set; }
    }
}
