using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swad_assg2
{
    public class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string VIN { get; set; }
        public string Photo { get; set; }

        public Car(string make, string model, int year, int mileage, string color, string licensePlate, string vin, string photo)
        {
            Make = make;
            Model = model;
            Year = year;
            Mileage = mileage;
            Color = color;
            LicensePlate = licensePlate;
            VIN = vin;
            Photo = photo;
        }
    }

}
