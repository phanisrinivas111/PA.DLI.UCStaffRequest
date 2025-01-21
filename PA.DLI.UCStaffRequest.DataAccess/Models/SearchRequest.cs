using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PA.DLI.UCStaffRequest.DataAccess.Models
{
    public class SearchRequest
    {
        public int? TicketNumber { get; set; }
        public int? Category { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string CwopaId { get; set; }
    }
}