using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swad_assg2
{
    public class User
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ContactDetails { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserType { get; set; }
        public DriversLicence DriversLicence { get; set; }
    }

}
