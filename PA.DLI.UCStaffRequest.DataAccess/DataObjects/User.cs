using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.DataObjects
{
    public class User
    {
        public string UserId { get; set; }
        public string RoleCode { get; set; }
        public DateTime LastChangeDate { get; set; }
        public string LastChangeUser { get; set; }
        public string IsDeleted { get; set; }
    }
}
