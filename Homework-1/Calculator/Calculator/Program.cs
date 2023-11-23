using System.Reflection.Metadata.Ecma335;

internal class Program
{

    public class Calculator
    {
        public  static double Add(double a, double b) => Math.Round(a + b, 3);
        public  static double Substract(double a, double b) => Math.Round(a - b,3);
        public static double Multiply(double a, double b) => Math.Round(a * b, 3);
        public static double Divide(double a, double b)
        { 
            if(b== 0)
            {
                return 0;
            }
            
            return Math.Round( a / b,3);
        }

        
        public static double Exponent(double a, double b) => Math.Round((double)Math.Pow(a, b),3);

        public static long Factorial (int n) => n == 0 ? 1 : n * Factorial(n - 1);

        public static void PrintResult(double result) => Console.WriteLine(result);

        public static void PrintResult(long result) => Console.WriteLine(result);

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

        public void SampleResults()
        {
            Random rnd = new Random();
            var rndDouble = new Random();

            var rDouble = rndDouble.NextDouble();


            int rndNum = rnd.Next(1, 10);
            int rndNum2 = rnd.Next(1, 10);
            var rRangeDouble = Math.Round(rDouble * (10 - 1) + 1, 2);
            var rRangeDouble2 = Math.Round(rDouble * (10 - 2) + 2, 2);

            //var rRangeDouble = rDouble * (upperBound - lowerBound) + lowerBound;

            Console.WriteLine("\nRandom double number between 1 and 10: " + rRangeDouble);

            Console.WriteLine("Random double number2 between 2 and 10: " + rRangeDouble2);

            Console.WriteLine("Random number between 1 and 10: " + rndNum);

            Console.WriteLine("Sample addition: " + rRangeDouble + " + " + rRangeDouble2 + " = " + (Add(rRangeDouble, rRangeDouble2)));
            Console.WriteLine("Sample substraction: " + rRangeDouble + " - " + rRangeDouble2 + " = " + (Substract(rRangeDouble, rRangeDouble2)));
            Console.WriteLine("Sample multiplication: " + rRangeDouble + " * " + rRangeDouble2 + " = " + (Multiply(rRangeDouble, rRangeDouble2)));
            Console.WriteLine("Sample division: " + rRangeDouble + " / " + rRangeDouble2 + " = " + (Divide(rRangeDouble, rRangeDouble2)));
            Console.WriteLine("Sample exponent: " + rndNum + " ^ " + rndNum2 + " = " + (Exponent(rndNum, rndNum2)));
            Console.WriteLine("Sample factorial: " + rndNum + " ! " + " = " + (Factorial(rndNum)));
            

        }

        
        
        

        public void Start()
        {   


            while (true)
            {
                Console.WriteLine("\nSelect the option (+, -, *, /, !,^, Exit):\nTo show sample results type 'sample'\n");
                string operation = Console.ReadLine();
                if (operation == "Exit")
                {
                    break;
                }

                else if (operation == "!")
                {
                    Console.WriteLine("Enter the number:");
                    string number = Console.ReadLine();
                    if (int.TryParse(number, out int numberParsed) & numberParsed >= 0)
                    {


                        PrintResult(Factorial(numberParsed));
                    }
                    else
                    { Console.WriteLine("No factorial for floats"); }

                }

                else if (operation == "+")
                {
                    Console.WriteLine("Enter the first number:");
                    string firstNumber = Console.ReadLine();
                    Console.WriteLine("Enter the second number:");
                    string secondNumber = Console.ReadLine();
                    if (ValidateNumbers(firstNumber, secondNumber))
                    {
                        PrintResult(Add(double.Parse(firstNumber), double.Parse(secondNumber)));
                    }
                }

                else if (operation == "-")
                {
                    Console.WriteLine("Enter the first number:");
                    string firstNumber = Console.ReadLine();
                    Console.WriteLine("Enter the second number:");
                    string secondNumber = Console.ReadLine();
                    if (ValidateNumbers(firstNumber, secondNumber))
                    {
                        PrintResult(Substract(double.Parse(firstNumber), double.Parse(secondNumber)));
                    }
                }

                else if (operation == "*")
                {
                    Console.WriteLine("Enter the first number:");
                    string firstNumber = Console.ReadLine();
                    Console.WriteLine("Enter the second number:");
                    string secondNumber = Console.ReadLine();
                    if (ValidateNumbers(firstNumber, secondNumber))
                    {
                        PrintResult(Multiply(double.Parse(firstNumber), double.Parse(secondNumber)));
                    }
                }
                else if (operation == "/")
                {
                    Console.WriteLine("Enter the first number:");
                    string firstNumber = Console.ReadLine();
                    Console.WriteLine("Enter the second number:");
                    string secondNumber = Console.ReadLine();
                    if (ValidateNumbers(firstNumber, secondNumber))
                    {
                        if (double.Parse(secondNumber) == 0)
                        {
                            Console.WriteLine("Cannot divide by zero");

                        }

                        PrintResult(Divide(double.Parse(firstNumber), double.Parse(secondNumber)));
                    }


                }

                else if (operation == "^")
                {
                    Console.WriteLine("Enter the first number:");
                    string firstNumber = Console.ReadLine();
                    Console.WriteLine("Enter the second number:");
                    string secondNumber = Console.ReadLine();
                    if (ValidateNumbers(firstNumber, secondNumber))
                    {
                        PrintResult(Exponent(double.Parse(firstNumber), double.Parse(secondNumber)));
                    }
                }

                else if (operation == "sample")
                    SampleResults();

                else
                {
                    Console.WriteLine("Invalid operation");
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