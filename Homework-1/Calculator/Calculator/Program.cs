internal class Program
{

    public class Calculator
    {
        public  double Add(double a, double b) => a + b;
        public  double Substract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b)
        {
            if (b != 0) return a / b;
            return 0;
        }

        public double Exponent(double a, double b) => (double)Math.Pow(a, b);

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
                
                else if (operation == "!")
                {
                    Console.WriteLine("Enter the number:");
                    string number = Console.ReadLine();
                    if (int.TryParse(number, out int numberParsed) & numberParsed >=0)
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

               else if(operation =="^")
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