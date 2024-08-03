using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swad_assg2
{
    class RenterService
    {
        private List<Renter> renters = new List<Renter>();

        public bool RegisterRenter(Renter renter)
        {
            if (AuthenticateRenter(renter))
            {
                renter.InsuranceDetails = ProvideInsurance(renter);
                renters.Add(renter);
                Console.WriteLine("Renter successfully registered.");
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AuthenticateRenter(Renter renter)
        {
            var authService = new AuthenticationService();
            return authService.PerformAuthenticityCheck(renter);
        }

        private string ProvideInsurance(Renter renter)
        {
            var insuranceService = new InsuranceService();
            return insuranceService.GiveInsurance(renter);
        }
    }
}
