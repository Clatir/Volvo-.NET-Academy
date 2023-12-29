using static Program;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Drawing;

internal class Program

{

    public abstract class Vehicle
    {

        private int _id { get; set; }
        private string _brand { get; set; }

        private string _model { get; set; }
        private int _yearOfProduction { get; set; }
        private string _color { get; set; }
        private double _price { get; set; }
        private string _registrationNumber { get; set; }
        private int _mileage { get; set; }
        private int _inService { get; set; }
        private int _comfortClass { get; set; }
        private double _modelCoefficient { get; set; }


        public int Id { get => _id; set => _id = value; }
        public string Brand { get => _brand; set => _brand = value; }
        public string Model { get => _model; set => _model = value; }
        public int YearOfProduction{ get => _yearOfProduction; set => _yearOfProduction = value;}
        public string Color { get => _color; set => _color = value; }
        public double Price { get => _price; set => _price = value; }
        public string RegistrationNumber { get => _registrationNumber; set => _registrationNumber = value; }
        public int Mileage { get => _mileage; set => _mileage = value; }
        public int InService { get => _inService; set => _inService = value; }
        public int ComfortClass { get => _comfortClass; set => _comfortClass = value; }
        public double ModelCoefficient { get => _modelCoefficient; set => _modelCoefficient = value; }




        public abstract double CalculateRentalCost(double durationOfTheTrip, double travelDistance);
        public abstract double CalculateValueOverTime();
        public abstract double CalculateMilageToMaintenance();

        public double CalculateDurationOfTheTripFactor(double durationOfTheTrip)
        {
            if (durationOfTheTrip > 0 && durationOfTheTrip <= 10) return 1;
            else if (durationOfTheTrip > 10 && durationOfTheTrip <= 20) return 1.05;
            else return 1.07;
        }


        public double CalculateTravelDistanceFactor(double TravelDistanceFactor)
        {
            if (TravelDistanceFactor > 0 && TravelDistanceFactor <= 1000) return 1;
            else if (TravelDistanceFactor > 1000 && TravelDistanceFactor <= 2000) return 1.05;
            else return 1.07;
        }

        public static List<Vehicle> GetVehicles()
        {
            List<Vehicle> vehicleList = new List<Vehicle>();


            string filePath = "Cars.json";

            string JSONString = File.ReadAllText(filePath);

            JArray jsonArray = JArray.Parse(JSONString);
            foreach (JObject item in jsonArray)
            {


                if (item.ContainsKey("lesseesRating"))
                {
                    vehicleList.Add(item.ToObject<PassengerVehicle>());
                }
                else
                {
                    vehicleList.Add(item.ToObject<CargoVehicle>());
                }


            }

            return vehicleList;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Brand: {Brand}, Model: {Model}, Year: {YearOfProduction}, " +
                   $"Color: {Color}, Price: {Price}, Registration Number: {RegistrationNumber}, " +
                   $"Mileage: {Mileage}, In service: {_inService}, " +
                   $"Comfort Class: {ComfortClass}, Model Coefficient: {ModelCoefficient}";
        }




    }

    public class ContainerClass

    {
        static List<Vehicle> vehicleList = Vehicle.GetVehicles();
        static StringBuilder vehicleSB = new StringBuilder();



        public static String GetVehiclesByBrand(string Brand)
        {
            vehicleSB.Clear();

            var vehiclesByBrand = from vehicle in vehicleList
                                  where vehicle.Brand.Equals(Brand)
                                  select vehicle;

            foreach (var vehicle in vehiclesByBrand)
            {
                vehicleSB.Append(vehicle.ToString());
            }
            return vehicleSB.ToString();

        }


        public static String GetAllCars()
        {
            vehicleSB.Clear();

            var allVehicles = from vehicle in vehicleList
                                  
                                  select vehicle;

            foreach (var vehicle in allVehicles)
            {
                vehicleSB.Append(vehicle.ToString());
            }
            return vehicleSB.ToString();

        }

        public static String GetVehiclesExceededTenure(String Model)
        {
            vehicleSB.Clear();
            var vehiclesExceededTenure = from vehicle in vehicleList
                                         where (vehicle.Model.Equals(Model) && vehicle is PassengerVehicle && vehicle.InService > 5 && vehicle.Mileage > 100000) || (vehicle.Model.Equals(Model) && vehicle is CargoVehicle && vehicle.InService > 7 & vehicle.Mileage > 1000000)
                                         select vehicle;

            foreach (var vehicle in vehiclesExceededTenure)
            {
                vehicleSB.Append(vehicle.ToString());
            }
            return vehicleSB.ToString();
        }


        public static double CalculateTotalValue()
        {

            var vehicleSum = from vehicle in vehicleList
                             select vehicle;


            return Math.Round(vehicleSum.Sum(vehicle => vehicle.CalculateValueOverTime()), 2);
        }

        public static double CalculateValueById(int selectedId, double selectedDurationOfTheTrip,double selectedTravelDistance )
        {

            var selctedVehicle = from vehicle in vehicleList
                          where vehicle.Id.Equals(selectedId)
                          select vehicle;
            return Math.Round(selctedVehicle.Sum(vehicle => vehicle.CalculateRentalCost(selectedDurationOfTheTrip,selectedTravelDistance)), 2);
        }

        public static string GetVehiclesComfortClassByBrandAndColor(string Brand, string Color)
        {
            vehicleSB.Clear();

            var vehiclesByBrandAndColor = from vehicle in vehicleList
                                          where vehicle.Brand.Equals(Brand) && vehicle.Color.Equals(Color)
                                          orderby vehicle.ComfortClass
                                          select vehicle;

            foreach (var vehicle in vehiclesByBrandAndColor)
            {
                vehicleSB.Append(vehicle.ToString());
            }
            return vehicleSB.ToString();
        }

        public static string GetVehiclesForMaintenance()
        {
            vehicleSB.Clear();

            var vehiclesForMaintenance = from vehicle in vehicleList
                                         where vehicle.CalculateMilageToMaintenance() <= 1000
                                         select vehicle;

            foreach (var vehicle in vehiclesForMaintenance)
            {
                vehicleSB.Append(vehicle.ToString());
            }
            return vehicleSB.ToString();
        }



    }

    public class PassengerVehicle : Vehicle
    {

        private double _lesseesRating { get; set; }
        public double LesseesRating { get => _lesseesRating; set => _lesseesRating = value; }

        public override double CalculateRentalCost(double durationOfTheTrip, double travelDistance)
        {
            return Price * CalculateDurationOfTheTripFactor(durationOfTheTrip)*CalculateTravelDistanceFactor(travelDistance)*ModelCoefficient*LesseesRating*0.25;
        }

        public override double CalculateValueOverTime()
        {
            return Price * Math.Pow(0.9, DateTime.Now.Year - YearOfProduction);
        }

        public override string ToString()
        {
            return base.ToString() + $", Lessees Rating: {_lesseesRating}" + "\n";
        }

        public override double CalculateMilageToMaintenance() => 5000 - (Mileage % 5000);

    }

    public class CargoVehicle : Vehicle
    {
        private double _cargoWeight { get; set; }

        public double CargoWeight { get => _cargoWeight; set => _cargoWeight = value; }

        public override double CalculateRentalCost(double durationOfTheTrip, double travelDistance)
        {
            
            return Price * CalculateDurationOfTheTripFactor(durationOfTheTrip) * CalculateTravelDistanceFactor(travelDistance) * ModelCoefficient * CargoWeight * 0.0025;
        }

        public override double CalculateValueOverTime()
        {
            return Price * Math.Pow(0.93, DateTime.Now.Year - YearOfProduction);
        }

        public override double CalculateMilageToMaintenance() => 15000 - (Mileage % 15000);

        public override string ToString()
        {
            return base.ToString() + $", Cargo Weight: {_cargoWeight}" + "\n";
        }

    }

    public class leasingSystem
    {
        public static void DisplayInterfaceOptions()
        {
            Console.WriteLine("Select option:\n 1.List inventory of vehicles of specified brand \n" +
                " 2.List of vehicles of a chosen Model that have exceeded a predetermined operational tenure. \n" +
                " 3.Calculate total value of the entire vehicle fleet owned. \n" +
                " 4.Show vehicles with matching Brand and colour sorted by comfort class \n" +
                " 5.Show a list of vehicles that are within 1000 km of requiring maintenance \n"+
                " 6.Calculate rental price");


        }

        public static void ClearInterface()
        {

            Console.WriteLine("Press Enter to continue");
            Console.ReadKey();
            Console.Clear();

        }

        static int GetValidInt()
        {
            int result;
            
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer \n");
                
            }

            return result;
        }

        static double GetValidDouble()
        {
            double result;
            
            while (!double.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid input. Please enter a valid double \n");
                
            }
            return result;
        }

        public static void SelectInterfaceOption()
        {
            while (true)
            {
                DisplayInterfaceOptions();
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Enter brand:");
                        Console.WriteLine(ContainerClass.GetVehiclesByBrand(Console.ReadLine()));
                        ClearInterface();


                        break;

                    case "2":
                        Console.WriteLine("Enter model:");
                        Console.WriteLine(ContainerClass.GetVehiclesExceededTenure(Console.ReadLine()));
                        ClearInterface();
                        break;
                    case "3":
                        Console.WriteLine("Total value of the entire vehicle fleet: " + ContainerClass.CalculateTotalValue());
                        ClearInterface();
                        break;
                    case "4":
                        Console.WriteLine("Enter brand:");
                        string Brand = Console.ReadLine();
                        Console.WriteLine("Enter color:");
                        string Color = Console.ReadLine();
                        Console.WriteLine(ContainerClass.GetVehiclesComfortClassByBrandAndColor(Brand, Color));
                        ClearInterface();
                        break;
                    case "5":
                        Console.WriteLine(ContainerClass.GetVehiclesForMaintenance());
                        ClearInterface();
                        break;
                    case "6":
                        Console.WriteLine(ContainerClass.GetAllCars());
                        Console.WriteLine("Enter Id:");
                        int vehicleId = GetValidInt();

                        Console.WriteLine("Enter duration of the trip:");
                        double durationOfTheTrip = GetValidDouble();

                        Console.WriteLine("Enter travel distance:");
                        double travelDistance = GetValidDouble();
                        Console.WriteLine("Rental price: " + ContainerClass.CalculateValueById(vehicleId, durationOfTheTrip, travelDistance));
                        ClearInterface();
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        ClearInterface();
                        break;
                }

            }

        }


    }

    static void Main()
    {
     
        leasingSystem.SelectInterfaceOption();

    }


}



