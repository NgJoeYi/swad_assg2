using swad_assg2;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static List<Car> cars = new List<Car>();
    static List<ListVehicle> listedVehicles = new List<ListVehicle>();
    static List<ListRental> rentalDetailsList = new List<ListRental>();

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

        // Main menu loop
        bool running = true;
        while (running)
        {
            int option = mainMenu();
            switch (option)
            {
                case 1:
                    Console.WriteLine("Return Car selected.");
                    break;
                case 2:
                    listVehicle(); // sequence 1 in the sequence diagram for listing a vehicle
                    break;
                case 3:
                    registerCar(); // sequence 1 in the sequence diagram for registering a car
                    break;
                case 4:
                    Console.WriteLine("Register Account selected.");
                    break;
                case 5:
                    Console.WriteLine("Reserve Car selected.");
                    break;
                case 6:
                    viewListedVehicles();
                    break;
                case 0:
                    running = false;
                    Console.WriteLine("Exiting the program.");
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }
        }
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
              "[6] View Listed Vehicles\n" +
              "[0] Exit\n" +
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
        Console.WriteLine("Car registered successfully.");
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

    // ------------------------ List Vehicle Flow ------------------------
    static void listVehicle() // Sequence 1
    {
        // Sequence 2: Car owner navigates to the “List Vehicle” page.
        Console.WriteLine("Navigate to List Vehicle page");

        // Sequence 3: Car owner enters vehicle details.
        string make, model, color, licensePlate, vin, photo, insuranceDetails;
        int year, mileage;
        inputVehicleDetails(out make, out model, out year, out mileage, out color, out licensePlate, out vin, out photo, out insuranceDetails);

        ListVehicle listedCar = new ListVehicle
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

        // Sequence 4: System validates the entered vehicle details.
        if (validateListedVehicle(listedCar))
        {
            Console.WriteLine("Car details match the registered vehicle.");

            // Sequence 5: Car owner specifies rental details.
            ListRental rentalDetails = inputRentalDetails();

            // Sequence 6: Car owner reviews rental details and submits the listing.
            saveRentalDetails(rentalDetails);

            // Add the listed vehicle and rental details to the lists
            listedVehicles.Add(listedCar);
            rentalDetailsList.Add(rentalDetails);

            // Sequence 7: System validates the entered rental details.
            // Sequence 8: System saves the vehicle information and lists it on the platform.
            displayConfirmationMessage("Vehicle listed successfully with rental details."); // Sequence 9

            // Sequence 9: System sends a confirmation message to the car owner.
        }
        else
        {
            Console.WriteLine("Car details do not match any registered vehicle.");
        }
    }

    static bool validateListedVehicle(ListVehicle vehicle) // Sequence 4
    {
        foreach (var existingCar in cars)
        {
            if (existingCar.LicensePlate == vehicle.LicensePlate && existingCar.VIN == vehicle.VIN)
            {
                return true;
            }
        }
        return false;
    }

    static ListRental inputRentalDetails() // Sequence 5
    {
        Console.WriteLine("\nEnter Rental Details\n-----------------");

        Console.Write("Daily Rate: ");
        double dailyRate = Convert.ToDouble(Console.ReadLine());

        Console.Write("Availability Dates: ");
        string availabilityDates = Console.ReadLine();

        Console.Write("Pickup Location: ");
        string pickupLocation = Console.ReadLine();

        Console.Write("Return Location: ");
        string returnLocation = Console.ReadLine();

        Console.Write("Insurance (true/false): ");
        bool insurance = Convert.ToBoolean(Console.ReadLine());

        return new ListRental
        {
            DailyRate = dailyRate,
            AvailabilityDates = availabilityDates,
            PickupLocation = pickupLocation,
            ReturnLocation = returnLocation,
            Insurance = insurance
        };
    }

    static void saveRentalDetails(ListRental rentalDetails) // Sequence 6
    {
        using (StreamWriter sw = new StreamWriter("Rental_Details.csv", true))
        {
            sw.WriteLine($"{rentalDetails.DailyRate},{rentalDetails.AvailabilityDates},{rentalDetails.PickupLocation},{rentalDetails.ReturnLocation},{rentalDetails.Insurance}");
        }
    }

    static void displayConfirmationMessage(string message) // Sequence 9
    {
        Console.WriteLine(message);
    }

    // --------------------- Input Vehicle Details ---------------------
    static void inputVehicleDetails(out string make, out string model, out int year, out int mileage, out string color, out string licensePlate, out string vin, out string photo, out string insuranceDetails) // Sequence 3
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
    // --------------------- Input Vehicle Details ---------------------

    // --------------------- View Listed Vehicles ---------------------
    static void viewListedVehicles()
    {
        Console.WriteLine("\nListed Vehicles:\n-----------------");

        for (int i = 0; i < listedVehicles.Count; i++)
        {
            var vehicle = listedVehicles[i];
            var rental = rentalDetailsList[i];

            Console.WriteLine($"Vehicle {i + 1} Details:");
            Console.WriteLine($"Make: {vehicle.Make}");
            Console.WriteLine($"Model: {vehicle.Model}");
            Console.WriteLine($"Year: {vehicle.Year}");
            Console.WriteLine($"Mileage: {vehicle.Mileage}");
            Console.WriteLine($"Color: {vehicle.Color}");
            Console.WriteLine($"License Plate: {vehicle.LicensePlate}");
            Console.WriteLine($"VIN: {vehicle.VIN}");
            Console.WriteLine($"Photo: {vehicle.Photo}");
            Console.WriteLine($"Insurance Details: {vehicle.InsuranceDetails}");

            Console.WriteLine("\nRental Details:");
            Console.WriteLine($"Daily Rate: {rental.DailyRate}");
            Console.WriteLine($"Availability Dates: {rental.AvailabilityDates}");
            Console.WriteLine($"Pickup Location: {rental.PickupLocation}");
            Console.WriteLine($"Return Location: {rental.ReturnLocation}");
            Console.WriteLine($"Insurance: {rental.Insurance}");
            Console.WriteLine("-----------------");
        }

        if (listedVehicles.Count == 0)
        {
            Console.WriteLine("No vehicles listed.");
        }
    }
    // --------------------- View Listed Vehicles ---------------------
}
