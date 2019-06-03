using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBookingSystem
{
    public class AdminDetail
    {
        private string adminId;
        public string AdminId
        {
            get { return adminId; }
            set { adminId = value; }
        }

        private string adminPass;
        public string AdminPass
        {
            get { return adminPass; }
            set { adminPass = value; }
        }

        public AdminDetail(string theAdminId, string theAdminPass)
        {
            AdminId = theAdminId;
            AdminPass = theAdminPass;
        }
        public AdminDetail() : this(" ", " ")
        {

        }
    }
}
