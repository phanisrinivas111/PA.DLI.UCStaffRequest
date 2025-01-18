using System;
using System.Collections.Generic;
using System.Diagnostics;
using PA.DLI.UCStaffRequest.DataAccess.Common;
using PA.DLI.UCStaffRequest.DataAccess.Common.Logging;
using PA.DLI.UCStaffRequest.DataAccess.DataAccess;
using PA.DLI.UCStaffRequest.DataAccess.DataObjects;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PA.DLI.UCStaffRequest.ViewModels
{
    public class AdminViewModel
    {
        // Lists to hold user and category data
        public List<User> Users { get; set; }
        public List<Category> Categories { get; set; }
        public List<string> Roles { get; set; }
        public string Result { get; set; }
      

        // Constructor to initialize lists
        public AdminViewModel()
        {
            Users = new List<User>();
            Categories = new List<Category>();
            Roles = new List<string>();
        }


        // User class with CWOPA ID and Role
        public class User
        {
            public string USERId { get; set; } // Represents the user ID
            public string Role { get; set; } // Role is now dynamic, retrieved from the database
        }

        // Category class with CategoryName and Email
        public class Category
        {
            public int CategoryId { get; set; }  // Added CategoryId property
            public string CategoryName { get; set; }
            public string Email { get; set; }
        }

        
    }
}