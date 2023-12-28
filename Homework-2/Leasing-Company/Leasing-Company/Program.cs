using static Program;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

internal class Program

{

    public abstract class Vehicle
    {

        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public int yearOfProduction { get; set; }
        public string color { get; set; }
        public double price { get; set; }
        public string registrationNumber { get; set; }
        public int mileage { get; set; }
        public int inService { get; set; }
        public int comfortClass { get; set; }
        public double modelCoefficient { get; set; }


        public abstract double CalculateRentalCost();
        public abstract double CalculateValueOverTime();
        public abstract double CalculateMilageToMaintenance();
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
            return $"ID: {id}, Brand: {brand}, Model: {model}, Year: {yearOfProduction}, " +
                   $"Color: {color}, Price: {price}, Registration Number: {registrationNumber}, " +
                   $"Mileage: {mileage}, In service: {inService}, " +
                   $"Comfort Class: {comfortClass}, Model Coefficient: {modelCoefficient}";
        }




    }

    public class ContainerClass

    {
       static  List<Vehicle> vehicleList = Vehicle.GetVehicles();
       static StringBuilder vehicleSB = new StringBuilder();



        public static String GetVehiclesByBrand(string brand)
        {
            vehicleSB.Clear();

            var vehiclesByBrand = from vehicle in vehicleList
                                 where vehicle.brand == brand
                                 select vehicle;

            foreach (var vehicle in vehiclesByBrand)
            {
                vehicleSB.Append(vehicle.ToString());
            }
            return vehicleSB.ToString();
            
        }

        public static String GetVehiclesExceededTenure()
        {
            vehicleSB.Clear();
            var vehiclesExceededTenure = from vehicle in vehicleList
                                         where (vehicle is PassengerVehicle && vehicle.inService>5 & vehicle.mileage>100000) || (vehicle is CargoVehicle && vehicle.inService>7&vehicle.mileage>1000000)
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


            return Math.Round(vehicleSum.Sum(vehicle => vehicle.CalculateValueOverTime()),2);
        }

        public static string GetVehiclesComfortClassByBrandAndColor(string brand, string color)
        {
            vehicleSB.Clear();

            var vehiclesByBrandAndColor = from vehicle in vehicleList
                                          where vehicle.brand == brand && vehicle.color == color
                                          orderby vehicle.comfortClass
                                          select vehicle;

            foreach (var vehicle in vehiclesByBrandAndColor)
            {
                vehicleSB.Append(vehicle.ToString());
            }
            return vehicleSB.ToString();
        }

        public static string GetVehiclesForMaintenance()
        {   vehicleSB.Clear();

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

            public double lesseesRating { get; set; }

            public override double CalculateRentalCost()
            {
                return 0;
            }

        public override double CalculateValueOverTime()
        {
            return price * Math.Pow(0.9, DateTime.Now.Year-yearOfProduction);
        }

        public override string ToString()
        {
            return base.ToString() + $", Lessees Rating: {lesseesRating}" + "\n";
        }

        public override double CalculateMilageToMaintenance() => 5000- (mileage % 5000);

    }

    public class CargoVehicle : Vehicle
        {
            public double cargoWeight { get; set; }

            public override double CalculateRentalCost()
            {
                return 0;
            }

        public override double CalculateValueOverTime()
        {
            return price * Math.Pow(0.93,DateTime.Now.Year - yearOfProduction);
        }

        public override double CalculateMilageToMaintenance() => 15000 - (mileage % 15000);

        public override string ToString()
        {
            return base.ToString() + $", Cargo Weight: {cargoWeight}" + "\n";
        }

    }

    static void Main()
        {


        List <Vehicle> myVehicleList = Vehicle.GetVehicles();


        /**
        foreach (var vehicle in myVehicleList)
        {
            Console.WriteLine(vehicle);
        }
        **/

        //Console.WriteLine(containerClass.GetVehiclesByBrand("BMW"));
        //Console.WriteLine(containerClass.GetVehiclesExceededTenure());
        //Console.WriteLine(containerClass.CalculateTotalValue());
        //Console.WriteLine(containerClass.GetVehiclesComfortClassByBrandAndColor("Audi", "Black"));
//        Console.WriteLine(ContainerClass.GetVehiclesForMaintenance());



        int myIntTime = DateTime.Now.Year;






    }


}