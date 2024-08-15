using System.Text.Json.Nodes;

namespace cliWeather
{
internal class WeatherData
{
    public static async Task<string> getTemperatureAsync(double[] choords)
    {
        // string unit = "&temperature_unit=fahrenheit";
        // string symbol = "°F";
        string unit = "&temperature_unit=celsius";
        string symbol = "°C";
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={choords[0]}&longitude={choords[1]}&hourly=temperature_2m&timeformat=unixtime{unit}";

        try
        {
            string json = await apiRequestAsync(url);

            var data = JsonObject.Parse(json);

            var hourly = data?["hourly"];
            var temperatureArray = hourly?["temperature_2m"]?.AsArray();
            var timeArray = hourly?["time"]?.AsArray();

            if (timeArray != null && temperatureArray != null && timeArray.Count > 0 && temperatureArray.Count > 0)
            {
                var timeArrayLong = timeArray.Select(t => t.GetValue<long>()).ToArray();
                int closestIndex = getClosestTimeIndex(timeArrayLong);

                if (closestIndex != -1)
                {
                    double temperature = temperatureArray[closestIndex]?.GetValue<double>() ?? 0;
                    return $"{temperature}{symbol}";
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

    private static int getClosestTimeIndex(long[] timeArray)
    {
        long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int closestIndex = -1;
        long closestTimeDifference = long.MaxValue;

        for (int i = 0; i < timeArray.Length; i++)
        {
            long time = timeArray[i];
            long timeDifference = Math.Abs(time - currentUnixTime);

            if (timeDifference < closestTimeDifference)
            {
                closestTimeDifference = timeDifference;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
}