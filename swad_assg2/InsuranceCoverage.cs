using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swad_assg2
{
    public class InsuranceCoverage
    {
        public string InsuranceId { get; set; }
        public string ProviderName { get; set; }
        public string PolicyNumber { get; set; }
        public string CoverageDetails { get; set; }
        public DateTime ValidityPeriod { get; set; }
    }
}
