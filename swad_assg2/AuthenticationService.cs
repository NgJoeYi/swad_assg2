using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swad_assg2
{
    class AuthenticationService
    {
        public bool PerformAuthenticityCheck(Renter renter)
        {
            if (CalculateAge(renter.DateOfBirth) <= 17)
            {
                Console.WriteLine("Authentication failed: Renter must be at least 18 years old.");
                return false;
            }

            // Additional authentication checks can be added here
            return true;
        }

        private int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                age--;
            return age;
        }
    }
}
