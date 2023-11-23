internal class Program
{

    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Substract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int Divide(int a, int b)
        {
            return a / b;

        }

        public int Exponent(int a, int b)
        {
            return a ^ b;
        }

        public long Factorial(int a)
        {
            long result = 1;
            for (int i = 1; i <= a; i++)
            {
                result *= i;
            }
            return result;
        }
    }
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}