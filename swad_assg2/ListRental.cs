using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swad_assg2
{
    public class ListRental
    {
        public double DailyRate { get; set; }
        public DateTime AvailabilityStartDateTime { get; set; }
        public DateTime AvailabilityEndDateTime { get; set; }
        public string PickupLocation { get; set; }
        public string ReturnLocation { get; set; }
        public bool Insurance { get; set; }
    }
}

