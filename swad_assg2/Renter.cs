using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swad_assg2
{
    class Renter
    {
        public string FullName { get; set; }
        public string ContactDetails { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DriversLicense { get; set; }
        public string InsuranceDetails { get; set; }

        public Renter(string fullName, string contactDetails, DateTime dateOfBirth, string driversLicense, string insuranceDetails)
        {
            FullName = fullName;
            ContactDetails = contactDetails;
            DateOfBirth = dateOfBirth;
            DriversLicense = driversLicense;
            InsuranceDetails = insuranceDetails;
        }
    }
}
