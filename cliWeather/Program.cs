namespace cliWeather
{
internal class Program
{
    static async Task Main(string[] args)
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
                case "now": // TODO: recognize only first word
                    await weatherCommand(input);
                    break;
                default:
                    Console.WriteLine($"The command '{input[0]}' could not be found!");
                    break;
            }

        } while (input[0] != "exit");
    }

    private static async Task weatherCommand(string[] input)
    {
        string city;
        try
        {
            city = input[1];
        } catch (Exception) 
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("now [city]");
            return;
        }
            
        city = city.ToLower();
        city = char.ToString(city[0]).ToUpper() + city.Substring(1);

        double[] choords = CityData.getCoordinates(city);
        string temperature;

        if (choords[0] != 0 && choords[1] != 0)
        {
            temperature = await WeatherData.getTemperatureAsync(choords);
            Console.WriteLine($"The temperature in {city} is: {temperature} ");
            return;
        } else
        {
            Console.WriteLine("City not found.");
            return;
        }
            
    }


    private static void helpCommand()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("exit\t\t\tExit the programm");
        Console.WriteLine("now [city]\t\tShow the current temperature");
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
}
}