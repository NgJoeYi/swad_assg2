using swad_assg2;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        List<Car> cars = new List<Car>();

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
            registerCar(cars); // sequence 1 in the sequence diagram
            /*
            1.                          registerCar()
            1.1                         showCarDetailsPrompt()
            2.                          inputCarDetails(...)
            2.1                         submitCarDetails(...)
            2.1.1                       validateResult = validateDetails()
            2.1.2                       validateResult
            2.2/2.2.1/2.2.1.1           addNewVehicle(...)
            2.3                         showValidateResult(...)
            2.4                         showErrorMessage(...)
            */
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
        static void registerCar(List<Car> cars) // sequence 1
        {
            showCarDetailsPrompt(cars);
        }

        // ------------------------ Show Car Details Prompt ------------------------
        static void showCarDetailsPrompt(List<Car> cars) // sequence 1.1
        {
            Car newCar = InputCarDetails();
            submitCarDetails(cars, newCar);
        }

        // --------------------- Input Car Details ---------------------
        static Car InputCarDetails() // sequence 2
        {
            Car car = new Car();
            Console.WriteLine("Enter Car Details:");

            Console.Write("Make: ");
            car.Make = Console.ReadLine();

            Console.Write("Model: ");
            car.Model = Console.ReadLine();

            Console.Write("Year: ");
            car.Year = Convert.ToInt32(Console.ReadLine());

            Console.Write("Mileage: ");
            car.Mileage = Convert.ToInt32(Console.ReadLine());

            Console.Write("Color: ");
            car.Color = Console.ReadLine();

            Console.Write("License Plate: ");
            car.LicensePlate = Console.ReadLine();

            Console.Write("VIN: ");
            car.VIN = Console.ReadLine();

            Console.Write("Photo: ");
            car.Photo = Console.ReadLine();

            Console.Write("Insurance Details: ");
            car.InsuranceDetails = Console.ReadLine();

            return car;
        }
        // --------------------- Input Car Details ---------------------

        // --------------------- Submit Car Details ---------------------
        static void submitCarDetails(List<Car> cars, Car car) // sequence 2.1
        {
            bool validateResult = validateResults(cars, car);

            if (validateResult)
            {
                addNewVehicle(cars, car); // sequence 2.2
                showValidateResults("success"); // sequence 2.3
            }
            else
            {
                showErrorMessage("Error: Duplicate car license plate found."); // sequence 2.4
            }
        }
        // --------------------- Submit Car Details ---------------------

        // --------------------- Add New Vehicle ---------------------
        static void addNewVehicle(List<Car> cars, Car car) // sequence 2.2
        {
            cars.Add(car);
            writeCarDetailsToFile(car);
            Console.WriteLine("Car registered successfully.");
        }
        // --------------------- Add New Vehicle ---------------------

        // --------------------- Validate Results ---------------------
        static bool validateResults(List<Car> cars, Car car) // sequence 2.1.1 and 2.1.2
        {
            // Check for duplicate license plates
            foreach (var existingCar in cars)
            {
                if (existingCar.LicensePlate == car.LicensePlate)
                {
                    return false;
                }
            }

            return true;
        }
        // --------------------- Validate Results ---------------------

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
