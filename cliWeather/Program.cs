using System;
using System.Threading.Tasks;

namespace cliWeather
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Weather Program: \ntype 'help' to see available commands.");

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
                    case "now": // TODO: recognize only the first word
                        await nowCommand(input);
                        break;
                    case "weather":
                        await weatherCommand(input);
                        break;
                    default:
                        Console.WriteLine($"The command '{input[0]}' could not be found!");
                        break;
                }

            } while (input[0] != "exit");
        }

        private static async Task nowCommand(string[] input)
        {
            string city = weatherCheck(input);
            if (string.IsNullOrEmpty(city)) {return;}

            double[] choords = CityData.getCoordinates(city);
            if (choords[0] != 0 && choords[1] != 0)
            {
                string temperature = await WeatherData.getTemperatureNowAsync(choords);
                Console.WriteLine($"The temperature in {city} is: {temperature}");
            }
            else
            {
                Console.WriteLine("City not found.");
            }
        }

        private static async Task weatherCommand(string[] input)
        {
            string city = weatherCheck(input);
            if (string.IsNullOrEmpty(city)) { return; }

            double[] choords = CityData.getCoordinates(city);
            if (choords[0] != 0 && choords[1] != 0)
            {
                string temperatures = await WeatherData.getTemperatureAsync(choords);
                Console.WriteLine($"The temperature in {city} ranges between {temperatures}");
            }
            else
            {
                Console.WriteLine("City not found.");
            }
        }

        private static void helpCommand()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("exit\t\t\tExit the program");
            Console.WriteLine("now [city]\t\tShow the current temperature");
            Console.WriteLine("weather [city]\t\tShow the lowest and highest temperatures for the current day.");
        }

        private static string[] getInput(string quote)
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

        private static string weatherCheck(string[] input)
        {
            if (input.Length > 1)
            {
                string city = input[1];
                city = city.ToLower();
                // Capitalize the first letter
                city = char.ToUpper(city[0]) + city.Substring(1);
                return city;
            }
            else
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("now [city]");
                return string.Empty;
            }
        }
    }
}
