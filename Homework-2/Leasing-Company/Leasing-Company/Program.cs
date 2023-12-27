internal class Program

{

    public abstract class vehicle
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
        public long durationOfTrip { get; set; }
        public long travelDistance { get; set; }
        public double modelCoefficient { get; set; }

    }

    class passengerVehicle : vehicle
    {

        public double lesseesRating { get; set; }
    
    }

    class cargoVehicle : vehicle
    {
        public double cargoWeight { get; set; }
    }

    private static void Main(string[] args)
    {

        passengerVehicle myPassangerVehicle = new passengerVehicle();


        Console.WriteLine("Hello, World!");
    }
}