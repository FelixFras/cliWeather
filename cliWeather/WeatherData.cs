using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace cliWeather
{
    internal class WeatherData
    {
        // TODO: Get CURRENT temperatures
        public static string getTemperature(double[] choords)
        {

            using (var client = new HttpClient())
            {
                var endpoint = new Uri($"https://api.open-meteo.com/v1/forecast?latitude={choords[0]}&longitude={choords[1]}&hourly=temperature_2m&timeformat=unixtime");
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;

                var data = JsonObject.Parse(json);

                var hourly = data?["hourly"];

                var temperatureArray = hourly?["temperature_2m"]?.AsArray();

                var timeArray = hourly?["time"]?.AsArray();

                if (temperatureArray != null && temperatureArray.Count > 0 && timeArray != null && timeArray.Count > 0)
                {
                    var temperature = temperatureArray[0]?.GetValue<double>() ?? 0;
                    var time = timeArray[0]?.GetValue<int>() ?? 0;

                    Console.WriteLine($"Time (Unix): {time}, Temperature: {temperature}°C");

                    return temperature.ToString() + "°C";
                }
                else
                {
                    Console.WriteLine("Temperature data not found.");
                    return "Temperature data not found.";
                }
            }

            
        }
    }
}
