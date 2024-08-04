using System;
namespace swad_assg2
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public float TotalCost { get; set; }
        public string PickUpLocation { get; set; }
        public string ReturnLocation { get; set; }
        public bool BookingStatus { get; set; }
        public Car Car { get; set; }
        public Renter Renter { get; set; }
        public Booking(int bookingId, DateTime startDateTime, DateTime endDateTime, float totalCost, string pickUpLocation, string returnLocation, bool bookingStatus)
        {
            BookingId = bookingId;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            TotalCost = totalCost;
            PickUpLocation = pickUpLocation;
            ReturnLocation = returnLocation;
            BookingStatus = bookingStatus;
        }
    }
}

