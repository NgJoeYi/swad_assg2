using System;
using System.Collections.Generic;

namespace swad_assg2
{
    public class CarOwner
    {
        public int OwnerId { get; set; }
        public List<Car> Cars { get; set; }
        public decimal Earnings { get; set; }

        public CarOwner(int ownerId, List<Car> cars, decimal earnings)
        {
            OwnerId = ownerId;
            Cars = cars;
            Earnings = earnings;
        }
    }
}
