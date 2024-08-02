using swad_assg2;

class Program
{
    static void Main(string[] args)
    {
        List<Car> cars = new List<Car>();

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
            Console.WriteLine("Register Car selected.");
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
        static int mainMenu() {
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


        // --------------------- Read Car Details ---------------------
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
        // --------------------- Read Car Details ---------------------


    }
}