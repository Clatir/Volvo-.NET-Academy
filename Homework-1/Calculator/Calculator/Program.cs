internal class Program
{

    public class Calculator
    {
        public double Add(double a, double b) => a + b;
        public double Substract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;        
        public double Divide(double a, double b) => a / b;        

        public double Exponent(double a, double b) => (double)Math.Pow(a, b);

        public long Factorial(long a) => a == 0 ? 1 : a * Factorial(a - 1);


    

        public void PrintResult(double result)
        {
            Console.WriteLine(result);
        }


        
    }
    private static void Main(string[] args)
    {
        Calculator myCalc = new Calculator();
        Console.WriteLine(myCalc.Add(4, 4));
    }
}