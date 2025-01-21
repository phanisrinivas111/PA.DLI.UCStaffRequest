using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.Models
{
    public class SearchResult
    {
        public int TicketId { get; set; }
        public string Category { get; set; }
        public string FromEmail { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
