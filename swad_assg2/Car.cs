using System;
using System.Collections.Generic;

namespace swad_assg2
{
    public class Car
    {
        public int CarId { get; set; }
        public bool Availability { get; set; }
        public decimal RentalRate { get; set; }
        public string InsuranceCoverage { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string VIN { get; set; }
        public string Photos { get; set; }
        public string Description { get; set; }
        public List<string> Reviews { get; set; }

        public Car(int carId, bool availability, decimal rentalRate, string insuranceCoverage, string make, string model, int year, int mileage, string color, string licensePlate, string vin, string photos, string description, List<string> reviews)
        {
            CarId = carId;
            Availability = availability;
            RentalRate = rentalRate;
            InsuranceCoverage = insuranceCoverage;
            Make = make;
            Model = model;
            Year = year;
            Mileage = mileage;
            Color = color;
            LicensePlate = licensePlate;
            VIN = vin;
            Photos = photos;
            Description = description;
            Reviews = reviews;
        }
    }
}

