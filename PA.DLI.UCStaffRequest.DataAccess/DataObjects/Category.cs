using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.DataObjects
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
