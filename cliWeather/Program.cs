using System.ComponentModel.Design;
using System.Security.Cryptography;

namespace cliWeather
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Weather Programm: \ntype 'help' to see available commands. ");

            string[] input;

            do 
            {
                input = getInput(">");

                switch (input[0])
                {
                    case "exit":
                        return;
                    case "help":
                        helpCommand();
                        break;
                    case "weather": // TODO: recognize only first word
                        weatherCommand(input);
                        break;
                    default:
                        Console.WriteLine($"The command '{input[0]}' could not be found!");
                        break;
                }

            } while (input[0] != "exit");
        }

        static void weatherCommand(string[] input)
        {
            string city;
            try
            {
                city = input[1];
            } catch (Exception e) 
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("weather [city]");
                return;
            }
            
            city = city.ToLower();
            string temperature = "Temperature not found";


            Console.WriteLine($"The temperature in {city} is: {temperature} ");
        }

        static void helpCommand()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("exit\t\t\tExit the programm");
            Console.WriteLine("weather [city]\t\tShow the current weather");
        }

        static string[] getInput(string quote)
        {
            Console.Write(quote + " ");
            string? input;
            do
            {
                input = Console.ReadLine();
            } while (input == null);

            input = input.ToLower();

            string[] inputArray = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return inputArray;
        }
    }
}