using swad_assg2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static List<Car> cars = new List<Car>();
    static List<ListVehicle> listedVehicles = new List<ListVehicle>();
    static List<ListRental> rentalDetailsList = new List<ListRental>();
    static List<Renter> renters = new List<Renter>();

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
                    Photo = items[7]
                };
                cars.Add(car);
            }
        }

        // Read existing renters details
        using (StreamReader sr = new StreamReader("Renters.csv"))
        {
            // read header first
            string s = sr.ReadLine();
            string[] header = s.Split(',');
            // read the rest 
            while ((s = sr.ReadLine()) != null)
            {
                string[] items = s.Split(',');
                if (items.Length >= 5)
                {
                    Renter renter = new Renter(
                        items[0],
                        items[1],
                        DateTime.Parse(items[2]),
                        items[3],
                        items[4]);
                    renters.Add(renter);
                }
                else
                {
                    Console.WriteLine("Invalid data format in Renters.csv.");
                }
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
                    RegisterCar(); // sequence 1 in the sequence diagram for registering a car
                    break;
                case 4:
                    registerAccount();
                    break;
                case 5:
                    Console.WriteLine("Reserve Car selected.");
                    break;
                case 6:
                    viewListedVehicles();
                    break;
                case 7:
                    viewRenters();
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
              "[7] View Renters\n" +
              "[0] Exit\n" +
              "---------------------------");
        Console.Write("Select an option: ");
        int option = Convert.ToInt32(Console.ReadLine());
        return option;
    }
    // --------------------- MAIN MENU ---------------------

    // ------------------------ Register Account Flow ------------------------
    static void registerAccount()
    {
        string fullName, contactDetails, driversLicense;
        DateTime dateOfBirth;
        inputRenterDetails(out fullName, out contactDetails, out dateOfBirth, out driversLicense);

        Renter renter = new Renter(fullName, contactDetails, dateOfBirth, driversLicense, null);
        RenterService renterService = new RenterService();
        if (renterService.RegisterRenter(renter))
        {
            // Update insurance details after registration
            renter.InsuranceDetails = "Full Coverage";
            // Add to the list and save to file
            renters.Add(renter);
            writeRenterDetailsToFile(renter);
        }
        else
        {
            Console.WriteLine("Authenticity Check Failed.");
            Console.WriteLine("Registration Process is Terminated");
        }
    }

    // --------------------- Input Renter Details ---------------------
    static void inputRenterDetails(out string fullName, out string contactDetails, out DateTime dateOfBirth, out string driversLicense)
    {
        Console.WriteLine("\nEnter Renter Details\n-----------------");


        while (true)
        {
            Console.Write("Full Name: ");
            fullName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fullName) || !Regex.IsMatch(fullName, @"^[a-zA-Z\s]+$"))
            {
                Console.WriteLine("Required Information Missing or Invalid. Please try again.");
                continue;
            }
            break;
        }

        while (true)
        {
            Console.Write("Contact Details: ");
            contactDetails = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(contactDetails))
            {
                Console.WriteLine("Required Information Missing or Invalid. Please try again.");
                continue;
            }
            break;
        }

        while (true)
        {
            Console.Write("Date of Birth (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
            {
                if (CalculateAge(dateOfBirth) <= 17)
                {
                    Console.WriteLine("Registration failed: You must be at least 18 years old to register.");
                    driversLicense = string.Empty; // Assign a default value before returning
                    return;
                }
                break;
            }
            else
            {
                Console.WriteLine("Required Information Missing or Invalid. Please try again.");
            }
        }

        while (true)
        {
            Console.Write("Driver's License: ");
            driversLicense = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(driversLicense))
            {
                Console.WriteLine("Required Information Missing or Invalid. Please try again.");
                continue;
            }
            break;
        }
    }

    // Calculate age from DateTime
    static int CalculateAge(DateTime birthDate)
    {
        int age = DateTime.Now.Year - birthDate.Year;
        if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            age--;
        return age;
    }

    // --------------------- Write Renter Details To File ---------------------
    static void writeRenterDetailsToFile(Renter renter)
    {
        using (StreamWriter sw = new StreamWriter("Renters.csv", true))
        {
            sw.WriteLine($"{renter.FullName},{renter.ContactDetails},{renter.DateOfBirth:yyyy-MM-dd},{renter.DriversLicense},{renter.InsuranceDetails}");
        }
    }

    // --------------------- View Renters ---------------------
    static void viewRenters()
    {
        Console.WriteLine("\nRegistered Renters:\n-----------------");

        foreach (var renter in renters)
        {
            Console.WriteLine($"Full Name: {renter.FullName}");
            Console.WriteLine($"Contact Details: {renter.ContactDetails}");
            Console.WriteLine($"Date of Birth: {renter.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"Driver's License: {renter.DriversLicense}");
            Console.WriteLine($"Insurance Details: {renter.InsuranceDetails}");
            Console.WriteLine("-----------------");
        }

        if (renters.Count == 0)
        {
            Console.WriteLine("No renters registered.");
        }
    }

    // ------------------------ List Vehicle Flow ------------------------
    static void listVehicle() // Sequence 1
    {
        // Sequence 2: Car owner navigates to the “List Vehicle” page.
        Console.WriteLine("Navigate to List Vehicle page");

        // Sequence 3: Car owner enters vehicle details.
        string make, model, color, licensePlate, vin, photo, insuranceDetails;
        int year, mileage;
        inputVehicleDetails(out make, out model, out year, out mileage, out color, out licensePlate, out vin, out photo);

        ListVehicle listedCar = new ListVehicle
        {
            Make = make,
            Model = model,
            Year = year,
            Mileage = mileage,
            Color = color,
            LicensePlate = licensePlate,
            VIN = vin,
            Photo = photo
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
    static void inputVehicleDetails(out string make, out string model, out int year, out int mileage, out string color, out string licensePlate, out string vin, out string photo) // Sequence 3
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












    //joeyi

    // ------------------------ Register Car Flow ------------------------
    static void RegisterCar() // sequence 1
    {
        bool success = false;
        while (!success)
        {
            success = ShowCarDetailsPrompt();
        }
        Console.WriteLine("Car registered successfully.");
    }

    // ------------------------ Show Car Details Prompt ------------------------
    static bool ShowCarDetailsPrompt() // sequence 1.1
    {
        string make, model, color, licensePlate, vin, photo;
        int year, mileage;
        InputCarDetails(out make, out model, out year, out mileage, out color, out licensePlate, out vin, out photo);
        Car newCar = new Car
        {
            Make = make,
            Model = model,
            Year = year,
            Mileage = mileage,
            Color = color,
            LicensePlate = licensePlate,
            VIN = vin,
            Photo = photo
        };
        return SubmitCarDetails(newCar);
    }

    // --------------------- Input Car Details ---------------------
    static void InputCarDetails(out string make, out string model, out int year, out int mileage, out string color, out string licensePlate, out string vin, out string photo) // sequence 2
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
    }

    // --------------------- Submit Car Details ---------------------
    static bool SubmitCarDetails(Car car) // sequence 2.1
    {
        bool validateResult = ValidateDetails(car.LicensePlate, car.VIN); // sequence 2.1.1

        if (validateResult)
        {
            AddNewVehicle(car); // sequence 2.2
            ShowValidateResults("success", car.LicensePlate, car.VIN); // sequence 2.3
            return true;
        }
        else
        {
            ShowErrorMessage("\n***Error: Duplicate car license plate or VIN found.***"); // sequence 2.4
            return false;
        }
    }
    // --------------------- Validate Details ---------------------
    static bool ValidateDetails(string licensePlate, string vin) // sequence 2.1.1 and 2.1.2
    {
        // Check for duplicate license plates or VINs
        foreach (var existingCar in cars)
        {
            if (existingCar.LicensePlate == licensePlate || existingCar.VIN == vin)
            {
                return false;
            }
        }

        return true;
    }

    // --------------------- Add New Vehicle ---------------------
    static void AddNewVehicle(Car car) // sequence 2.2
    {
        cars.Add(car);
        WriteCarDetailsToFile(car);
    }
    // --------------------- Show Validate Results ---------------------
    static void ShowValidateResults(string result, string licensePlate, string vin) // sequence 2.3
    {
        Console.WriteLine($"Validation result: {result} for License Plate: {licensePlate} and VIN: {vin}");
    }
    // --------------------- Show Error Message ---------------------
    static void ShowErrorMessage(string message) // sequence 2.4
    {
        Console.WriteLine(message);
    }
    // --------------------- Write Car Details To File ---------------------
    static void WriteCarDetailsToFile(Car car)
    {
        using (StreamWriter sw = new StreamWriter("Car_Details.csv", true))
        {
            sw.WriteLine($"{car.Make},{car.Model},{car.Year},{car.Mileage},{car.Color},{car.LicensePlate},{car.VIN},{car.Photo}");
        }
    }
}
