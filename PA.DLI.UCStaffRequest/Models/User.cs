using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PA.DLI.UCStaffRequest.Models
{
    public class User
    {
        //public string CWOPAID { get; set; } // Represents the user ID
        //public string Role { get; set; } // Role is now dynamic, retrieved from the database

        public string UserId { get; set; }
        public string RoleCode { get; set; }
        public DateTime LastChangeDate { get; set; }
        public string LastChangeUser { get; set; }
        public string IsDeleted { get; set; }
    }
}