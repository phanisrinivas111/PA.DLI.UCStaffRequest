using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PA.DLI.UCStaffRequest.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Email { get; set; }
        public DateTime LastChangedDate { get; set; }

        public string LastChangedUser { get; set; }
        public string IsDeleted { get; set; }
    }
}