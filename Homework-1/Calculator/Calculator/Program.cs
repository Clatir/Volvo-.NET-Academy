internal class Program
{

    public class Calculator
    {
        public  double Add(double a, double b) => a + b;
        public  double Substract(double a, double b) => a - b;
        public  double Multiply(double a, double b) => a * b;        
        public  double Divide(double a, double b) => a / b;        

        public  double Exponent(double a, double b) => (double)Math.Pow(a, b);

        public long Factorial (int n) => n == 0 ? 1 : n * Factorial(n - 1);

        public void PrintResult(double result) => Console.WriteLine(result);

        public void PrintResult(long result) => Console.WriteLine(result);

        public bool ValidateNumbers(params string[] numbers)
        {
            foreach (var number in numbers)
            {
                if (!double.TryParse(number, out _))
                {
                    Console.WriteLine("Invalid number");
                    return false;
                }
            }
            return true;
        }

        
        

        public void Start()
        {   


            while (true)
            {
                Console.WriteLine("Select the operation (+, -, *, /, !,^, Exit):");
                string operation = Console.ReadLine();
                if (operation == "Exit")
                {
                    break;
                }        

               


            }
         


      
        }   


    

        
    }
    private static void Main(string[] args)
    {
       Calculator myCalc = new Calculator();
        myCalc.Start();
        
        
    }
}