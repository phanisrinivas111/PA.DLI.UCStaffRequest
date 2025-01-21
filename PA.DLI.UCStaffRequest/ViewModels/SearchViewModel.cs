using PA.DLI.UCStaffRequest.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PA.DLI.UCStaffRequest.ViewModels
{
    public class SearchViewModel
    {
        public SearchRequest searchRequest {  get; set; }
        public IEnumerable<PA.DLI.UCStaffRequest.Models.SearchResult> Results { get; set; }
        public int TotalResults { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
    public class SearchRequest
    {
        public int? TicketNumber { get; set; }
        public int? Category { get; set; }
        public string CategoryName { get; set; }
        public string SubmittedDate { get; set; }
        public string CwopaId { get; set; }
    }
}