using swad_assg2;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static List<Car> cars = new List<Car>();

    static void Main(string[] args)
    {
        // Read existing car details
        using (StreamReader sr = new StreamReader("Car_Details.csv"))
        {
            // read header first
            string s = sr.ReadLine();
            string[] header = s.Split(',');
            // read the rest 
            while ((s = sr.ReadLine()) != null)
            {
                string[] items = s.Split(',');
                Car car = new Car
                {
                    Make = items[0],
                    Model = items[1],
                    Year = Convert.ToInt32(items[2]),
                    Mileage = Convert.ToInt32(items[3]),
                    Color = items[4],
                    LicensePlate = items[5],
                    VIN = items[6],
                    Photo = items[7],
                    InsuranceDetails = items[8]
                };
                cars.Add(car);
            }
        }

        int option = mainMenu();
        if (option == 1)
        {
            Console.WriteLine("Return Car selected.");
        }
        else if (option == 2)
        {
            Console.WriteLine("List Vehicle selected.");
        }
        else if (option == 3)
        {
            registerCar(); // sequence 1 in the sequence diagram
        }
        else if (option == 4)
        {
            Console.WriteLine("Register Account selected.");
        }
        else if (option == 5)
        {
            Console.WriteLine("Reserve Car selected.");
        }
        else
        {
            Console.WriteLine("Invalid option selected.");
        }

        // --------------------- MAIN MENU ---------------------
        static int mainMenu()
        {
            Console.WriteLine("-------- iCar Menu --------\n" +
                  "[1] Return Car\n" +
                  "[2] List Vehicle\n" +
                  "[3] Register Car\n" +
                  "[4] Register Account\n" +
                  "[5] Reserve Car\n" +
                  "---------------------------");
            Console.Write("Select an option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            return option;
        }
        // --------------------- MAIN MENU ---------------------

        // ------------------------ Register Car Flow ------------------------
        static void registerCar() // sequence 1
        {
            bool success = false;
            while (!success)
            {
                success = showCarDetailsPrompt();
            }
        }

        // ------------------------ Show Car Details Prompt ------------------------
        static bool showCarDetailsPrompt() // sequence 1.1
        {
            string make, model, color, licensePlate, vin, photo, insuranceDetails;
            int year, mileage;
            inputCarDetails(out make, out model, out year, out mileage, out color, out licensePlate, out vin, out photo, out insuranceDetails);
            Car newCar = new Car
            {
                Make = make,
                Model = model,
                Year = year,
                Mileage = mileage,
                Color = color,
                LicensePlate = licensePlate,
                VIN = vin,
                Photo = photo,
                InsuranceDetails = insuranceDetails
            };
            return submitCarDetails(newCar);
        }

        // --------------------- Input Car Details ---------------------
        static void inputCarDetails(out string make, out string model, out int year, out int mileage, out string color, out string licensePlate, out string vin, out string photo, out string insuranceDetails) // sequence 2
        {
            Console.WriteLine("\nEnter Car Details\n-----------------");

            Console.Write("Make: ");
            make = Console.ReadLine();

            Console.Write("Model: ");
            model = Console.ReadLine();

            Console.Write("Year: ");
            year = Convert.ToInt32(Console.ReadLine());

            Console.Write("Mileage: ");
            mileage = Convert.ToInt32(Console.ReadLine());

            Console.Write("Color: ");
            color = Console.ReadLine();

            Console.Write("License Plate: ");
            licensePlate = Console.ReadLine();

            Console.Write("VIN: ");
            vin = Console.ReadLine();

            Console.Write("Photo: ");
            photo = Console.ReadLine();

            Console.Write("Insurance Details: ");
            insuranceDetails = Console.ReadLine();
        }
        // --------------------- Input Car Details ---------------------

        // --------------------- Submit Car Details ---------------------
        static bool submitCarDetails(Car car) // sequence 2.1
        {
            bool validateResult = validateDetails(car.LicensePlate); // sequence 2.1.1

            if (validateResult)
            {
                addNewVehicle(car); // sequence 2.2
                showValidateResults("success"); // sequence 2.3
                return true;
            }
            else
            {
                showErrorMessage("\n***Error: Duplicate car license plate found.***\n"); // sequence 2.4
                return false;
            }
        }
        // --------------------- Submit Car Details ---------------------

        // --------------------- Add New Vehicle ---------------------
        static void addNewVehicle(Car car) // sequence 2.2
        {
            cars.Add(car);
            writeCarDetailsToFile(car);
            Console.WriteLine("Car registered successfully.");
        }
        // --------------------- Add New Vehicle ---------------------

        // --------------------- Validate Details ---------------------
        static bool validateDetails(string licensePlate) // sequence 2.1.1 and 2.1.2
        {
            // Check for duplicate license plates
            foreach (var existingCar in cars)
            {
                if (existingCar.LicensePlate == licensePlate)
                {
                    return false;
                }
            }

            return true;
        }
        // --------------------- Validate Details ---------------------

        // --------------------- Show Validate Results ---------------------
        static void showValidateResults(string result) // sequence 2.3
        {
            Console.WriteLine($"Validation result: {result}");
        }
        // --------------------- Show Validate Results ---------------------

        // --------------------- Show Error Message ---------------------
        static void showErrorMessage(string message) // sequence 2.4
        {
            Console.WriteLine(message);
        }
        // --------------------- Show Error Message ---------------------

        // --------------------- Write Car Details To File ---------------------
        static void writeCarDetailsToFile(Car car)
        {
            using (StreamWriter sw = new StreamWriter("Car_Details.csv", true))
            {
                sw.WriteLine($"{car.Make},{car.Model},{car.Year},{car.Mileage},{car.Color},{car.LicensePlate},{car.VIN},{car.Photo},{car.InsuranceDetails}");
            }
        }
        // --------------------- Write Car Details To File ---------------------
    }
}
