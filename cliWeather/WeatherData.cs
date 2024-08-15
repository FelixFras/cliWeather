using System.Text.Json.Nodes;

namespace cliWeather
{
internal class WeatherData
{
    public static async Task<string> getTemperatureNowAsync(double[] choords)
    {
        // string unit = "&temperature_unit=fahrenheit";
        string unit = "&temperature_unit=celsius";
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={choords[0]}&longitude={choords[1]}&current=temperature_2m&timeformat=unixtime{unit}&timezone=Europe%2FLondon";

        try
        {
            string json = await apiRequestAsync(url);
            var data = JsonObject.Parse(json);

            var current = data?["current"] as JsonObject;
            var current_units = data?["current_units"] as JsonObject;

            if (current != null && current_units != null)
            {
                var temperature = current["temperature_2m"]?.GetValue<double>();
                var symbol = current_units["temperature_2m"]?.GetValue<string>();

                return $"{temperature}{symbol}";
            }

            return "Temperature data not found.";
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            return "Error fetching temperature.";
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
            return "Unexpected error occurred.";
        }
    }

    public static async Task<string> getTemperatureAsync(double[] choords)
    {
        string unit = "&temperature_unit=celsius";
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={choords[0]}&longitude={choords[1]}&daily=temperature_2m_max,temperature_2m_min&timezone=Europe/Berlin{unit}";

        try
        {
            string json = await apiRequestAsync(url);
            var data = JsonObject.Parse(json);

            var daily = data?["daily"] as JsonObject;

            if (daily != null)
            {
                var timeArray = daily["time"]?.AsArray();
                var tempMaxArray = daily["temperature_2m_max"]?.AsArray();
                var tempMinArray = daily["temperature_2m_min"]?.AsArray();

                if (timeArray != null && tempMaxArray != null && tempMinArray != null && timeArray.Count > 0)
                {
                    int index = 0;
                    double maxTemp = tempMaxArray[index]?.GetValue<double>() ?? double.NaN;
                    double minTemp = tempMinArray[index]?.GetValue<double>() ?? double.NaN;

                    return $"{minTemp}°C and {maxTemp}°C";
                }
            }

            return "Temperature data not found.";
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            return "Error fetching temperature.";
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
            return "Unexpected error occurred.";
        }
    }

    public static async Task<string> getHourlyTemperatureAsync(double[] choords)
    {
        string unit = "&temperature_unit=celsius";
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={choords[0]}&longitude={choords[1]}&hourly=temperature_2m&timeformat=unixtime{unit}";

        try
        {
            string json = await apiRequestAsync(url);
            var data = JsonObject.Parse(json);

            var hourly = data?["hourly"] as JsonObject;

            if (hourly != null)
            {
                var timeArray = hourly["time"]?.AsArray();
                var tempArray = hourly["temperature_2m"]?.AsArray();

                if (timeArray != null && tempArray != null && timeArray.Count > 0)
                {
                    long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    // get index of first hour
                    int index = 0;
                    for (int i = 0; i < timeArray.Count; i++)
                    {
                        long time = timeArray[i]?.GetValue<long>() ?? 0;
                        if (time > currentUnixTime)
                        {
                            index = i;
                            break;
                        }
                    }

                    string hourlyTemps = "";
                    for (int i = index; i < timeArray.Count && i < index + 25; i++) // show next 24 hours
                    {
                        long time = timeArray[i]?.GetValue<long>() ?? 0;
                        double temp = tempArray[i]?.GetValue<double>() ?? double.NaN;
                        DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(time);
                        hourlyTemps += $"{dateTime:HH:mm}: {temp}°C\n";
                    }

                    return hourlyTemps.TrimEnd();
                }
            }

            return "Hourly temperature data not found.";
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            return "Error fetching hourly temperature.";
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
            return "Unexpected error occurred.";
        }
    }

    private static async Task<string> apiRequestAsync(string url)
    {
        using (var client = new HttpClient())
        {
            var endpoint = new Uri(url);
            var response = await client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode(); // Throw if not a success code.

            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}
}