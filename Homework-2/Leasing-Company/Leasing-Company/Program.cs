using static Program;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata.Ecma335;

internal class Program

{

    public abstract class Vehicle
    {

        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string yearOfProduction { get; set; }
        public string color { get; set; }
        public double price { get; set; }
        public string registrationNumber { get; set; }
        public int mileage { get; set; }
        public int serviceInterval { get; set; }
        public string comfortClass { get; set; }
        public double modelCoefficient { get; set; }


        public abstract double CalculateRentalCost();

        public static List<Vehicle> GetVehicles()
        {
            List<Vehicle> vehicleList = new List<Vehicle>();


            string filePath = "Cars.json";

            string JSONString = File.ReadAllText(filePath);

            JArray jsonArray = JArray.Parse(JSONString);
            foreach (JObject item in jsonArray)
            {


                if (item.ContainsKey("leeseesRating"))
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

       

    }





       public class PassengerVehicle : Vehicle
        {

            public double lesseesRating { get; set; }

            public override double CalculateRentalCost()
            {
                return 0;
            }



        }

       public class CargoVehicle : Vehicle
        {
            public double cargoWeight { get; set; }

            public override double CalculateRentalCost()
            {
                return 0;
            }
        }











        static void Main()
        {


        List <Vehicle> myVehicleList = Vehicle.GetVehicles();

  



        }
    

}