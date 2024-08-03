using swad_assg2;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static List<Car> cars = new List<Car>();
    static List<ListVehicle> listedVehicles = new List<ListVehicle>();
    static List<ListRental> rentalDetailsList = new List<ListRental>();
    static List<Renter> renters = new List<Renter>();
    static List<InsuranceCoverage> insurances = new List<InsuranceCoverage>();

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
                Car car = new Car(
                    items[0], // make
                    items[1], // model
                    Convert.ToInt32(items[2]), // year
                    Convert.ToInt32(items[3]), // mileage
                    items[4], // color
                    items[5], // license plate
                    items[6], // VIN
                    items[7]  // photo
                );
                cars.Add(car);
            }
        }

        // Load existing renter details 
        LoadRenterDetails();

        // Load existing insurance details 
        LoadInsuranceDetails();

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

    // ----------------------- Load Renter and Insurance ------------------------
    static void LoadRenterDetails() // I CHANGED THIS OMG
    {
        using (StreamReader sr = new StreamReader("Renter_Details.csv"))
        {
            // read header first
            string s = sr.ReadLine();
            string[] header = s.Split(',');
            // read the rest 
            while ((s = sr.ReadLine()) != null)
            {
                string[] items = s.Split(',');
                Renter renter = new Renter
                {
                    RenterId = items[0],
                    FullName = items[1],
                    ContactDetails = items[2],
                    DateOfBirth = DateTime.Parse(items[3]),
                    DriversLicence = new DriversLicence
                    {
                        LicenceNumber = items[4],
                        ExpiryDate = DateTime.Parse(items[5]),
                        IssuingCountry = items[6]
                    },
                    BookingHistory = items[7],
                    PaymentDetails = items[8],
                    VerificationStatus = items[9],
                    IsPrime = Convert.ToBoolean(items[10]),
                    UpcomingRentals = items[11],
                    IsPenalised = Convert.ToBoolean(items[12])
                };
                renters.Add(renter);
            }
        }
    }

    static void LoadInsuranceDetails() // I CHANGED THIS OMG
    {
        using (StreamReader sr = new StreamReader("Insurance_Details.csv"))
        {
            // read header first
            string s = sr.ReadLine();
            string[] header = s.Split(',');
            // read the rest 
            while ((s = sr.ReadLine()) != null)
            {
                string[] items = s.Split(',');
                InsuranceCoverage insurance = new InsuranceCoverage
                {
                    InsuranceId = items[0],
                    ProviderName = items[1],
                    PolicyNumber = items[2],
                    CoverageDetails = items[3],
                    ValidityPeriod = DateTime.Parse(items[4])
                };
                insurances.Add(insurance);
            }
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

    // ------------------------ Register Renter Account Flow ------------------------


    static void registerAccount() // HELLO I ADDED THIS
    {
        string fullName, contactDetails, licenceNumber, issuingCountry;
        DateTime dateOfBirth, expiryDate;
        inputRenterDetails(out fullName, out contactDetails, out dateOfBirth, out licenceNumber, out expiryDate, out issuingCountry);

        // Calculate the next renter ID
        int nextRenterId = renters
            .Select(r => {
                int id;
                return int.TryParse(r.RenterId, out id) ? (int?)id : null;
            })
            .Where(id => id.HasValue)
            .Max() ?? 0;
        nextRenterId += 1;

        Renter newRenter = new Renter
        {
            RenterId = nextRenterId.ToString(),
            FullName = fullName,
            ContactDetails = contactDetails,
            DateOfBirth = dateOfBirth,
            UserType = "Renter",
            DriversLicence = new DriversLicence // HELLO I ADDED THIS
            {
                LicenceNumber = licenceNumber,
                ExpiryDate = expiryDate,
                IssuingCountry = issuingCountry
            },
            BookingHistory = "NULL",
            PaymentDetails = "NULL",
            VerificationStatus = "Pending",
            IsPrime = false,
            UpcomingRentals = "NULL",
            IsPenalised = false
        };

        bool authenticityCheck = performAuthenticityCheck(newRenter);

        if (authenticityCheck)
        {
            assignInsurance(newRenter);
            registerRenter(newRenter);
            Console.WriteLine("Account registered successfully.");
        }
        else
        {
            Console.WriteLine("Account registration failed due to failed authenticity check.");
        }
    }

    static void inputRenterDetails(out string fullName, out string contactDetails, out DateTime dateOfBirth, out string licenceNumber, out DateTime expiryDate, out string issuingCountry) // HELLO I ADDED THIS
    {
        Console.WriteLine("\nEnter Renter Details\n-----------------");

        Console.Write("Full Name: ");
        fullName = Console.ReadLine();

        Console.Write("Contact Details: ");
        contactDetails = Console.ReadLine();

        Console.Write("Date of Birth (yyyy-mm-dd): ");
        dateOfBirth = DateTime.Parse(Console.ReadLine());

        Console.Write("Drivers Licence Number: ");
        licenceNumber = Console.ReadLine();

        Console.Write("Drivers Licence Expiry Date (yyyy-mm-dd): ");
        expiryDate = DateTime.Parse(Console.ReadLine());

        Console.Write("Drivers Licence Issuing Country: ");
        issuingCountry = Console.ReadLine();
    }

    static bool performAuthenticityCheck(Renter renter) // HELLO I ADDED THIS
    {
        // Implement the authenticity check logic here
        // For now, we'll assume the check passes
        return true;
    }

    static void assignInsurance(Renter renter)
    {
        InsuranceCoverage insurance = new InsuranceCoverage
        {
            InsuranceId = renter.RenterId, // Link insurance to renter ID
            ProviderName = "Default Insurance Provider",
            PolicyNumber = "ICAR" + new Random().Next(1000, 9999).ToString(),
            CoverageDetails = "Basic Coverage",
            ValidityPeriod = DateTime.Now.AddYears(1)
        };

        insurances.Add(insurance);
        Console.WriteLine("Insurance assigned: " + insurance.PolicyNumber);
    }


    static void registerRenter(Renter renter) // HELLO I ADDED THIS
    {
        renters.Add(renter);
        writeRenterDetailsToFile(renter);
    }

    static void writeRenterDetailsToFile(Renter renter) // HELLO I ADDED THIS
    {
        using (StreamWriter sw = new StreamWriter("Renter_Details.csv", true))
        {
            sw.WriteLine($"{renter.RenterId},{renter.FullName},{renter.ContactDetails},{renter.DateOfBirth},{renter.DriversLicence.LicenceNumber},{renter.DriversLicence.ExpiryDate},{renter.DriversLicence.IssuingCountry},{renter.BookingHistory},{renter.PaymentDetails},{renter.VerificationStatus},{renter.IsPrime},{renter.UpcomingRentals},{renter.IsPenalised}");
        }
    }

    // --------------------- View Renters ---------------------
    static void viewRenters()
    {
        Console.WriteLine("\nRenters:\n-----------------");

        foreach (var renter in renters)
        {
            Console.WriteLine($"Renter ID: {renter.RenterId}");
            Console.WriteLine($"Full Name: {renter.FullName}");
            Console.WriteLine($"Contact Details: {renter.ContactDetails}");
            Console.WriteLine($"Date of Birth: {renter.DateOfBirth.ToShortDateString()}");
            Console.WriteLine($"Drivers Licence Number: {renter.DriversLicence.LicenceNumber}");
            Console.WriteLine($"Drivers Licence Expiry Date: {renter.DriversLicence.ExpiryDate.ToShortDateString()}");
            Console.WriteLine($"Drivers Licence Issuing Country: {renter.DriversLicence.IssuingCountry}");
            Console.WriteLine($"Booking History: {renter.BookingHistory}");
            Console.WriteLine($"Payment Details: {renter.PaymentDetails}");
            Console.WriteLine($"Verification Status: {renter.VerificationStatus}");
            Console.WriteLine($"Is Prime: {renter.IsPrime}");
            Console.WriteLine($"Upcoming Rentals: {renter.UpcomingRentals}");
            Console.WriteLine($"Is Penalised: {renter.IsPenalised}");
            Console.WriteLine("-----------------");

            // Display insurance details
            var insurance = insurances.Find(i => i.InsuranceId == renter.RenterId);
            if (insurance != null)
            {
                Console.WriteLine($"Insurance Provider: {insurance.ProviderName}");
                Console.WriteLine($"Policy Number: {insurance.PolicyNumber}");
                Console.WriteLine($"Coverage Details: {insurance.CoverageDetails}");
                Console.WriteLine($"Validity Period: {insurance.ValidityPeriod.ToShortDateString()}");
            }
            else
            {
                Console.WriteLine("No insurance details found.");
            }

            Console.WriteLine("-----------------");
        }

        if (renters.Count == 0)
        {
            Console.WriteLine("No renters found.");
        }
    }
}
    //joeyi's

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
        Car newCar = new Car(make, model, year, mileage, color, licensePlate, vin, photo);

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
            AddNewVehicle(car); // sequence 2.1.3
            ShowValidateResults("success", car.LicensePlate, car.VIN); // sequence 2.1.4
            return true;
        }
        else
        {
            ShowErrorMessage("\n***Error: Duplicate car license plate or VIN found.***"); // sequence 2.1.5
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
    static void AddNewVehicle(Car car) // sequence 2.1.3
    {
        cars.Add(car);
        WriteCarDetailsToFile(car);
    }
    // --------------------- Show Validate Results ---------------------
    static void ShowValidateResults(string result, string licensePlate, string vin) // sequence 2.1.4
    {
        Console.WriteLine($"Validation result: {result} for License Plate: {licensePlate} and VIN: {vin}");
    }
    // --------------------- Show Error Message ---------------------
    static void ShowErrorMessage(string message) // sequence 2.1.5
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

    // end of joeyi's
}
