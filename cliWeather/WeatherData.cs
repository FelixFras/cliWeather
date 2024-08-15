using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace cliWeather
{
    internal class WeatherData
    {
        public static async Task<string> getTemperatureAsync(double[] choords)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var endpoint = new Uri($"https://api.open-meteo.com/v1/forecast?latitude={choords[0]}&longitude={choords[1]}&hourly=temperature_2m&timeformat=unixtime");
                    var response = await client.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.

                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonObject.Parse(json);

                    var hourly = data?["hourly"];
                    var temperatureArray = hourly?["temperature_2m"]?.AsArray();
                    var timeArray = hourly?["time"]?.AsArray();

                    if (timeArray != null && temperatureArray != null && timeArray.Count > 0 && temperatureArray.Count > 0)
                    {
                        long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        int closestIndex = -1;
                        long closestTimeDifference = long.MaxValue;

                        for (int i = 0; i < timeArray.Count; i++)
                        {
                            long time = timeArray[i]?.GetValue<long>() ?? 0;
                            long timeDifference = Math.Abs(time - currentUnixTime);

                            if (timeDifference < closestTimeDifference)
                            {
                                closestTimeDifference = timeDifference;
                                closestIndex = i;
                            }
                        }

                        if (closestIndex != -1)
                        {
                            double temperature = temperatureArray[closestIndex]?.GetValue<double>() ?? 0;
                            return $"{temperature}°C";
                        }
                    }

                    return "Temperature data not found.";
                }
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
    }
}