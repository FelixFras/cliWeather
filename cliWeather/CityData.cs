using System.Text.Json.Nodes;

namespace cliWeather
{
internal class CityData
{
    public static double[] getCoordinates(string city)
    {
        using (var client = new HttpClient())
        {
            var endpoint = new Uri($"https://geocoding-api.open-meteo.com/v1/search?name={city}&count=1&language=en&format=json");
            var result = client.GetAsync(endpoint).Result;
            var json = result.Content.ReadAsStringAsync().Result;

            var data = JsonObject.Parse(json);
            var results = data?["results"]?.AsArray();

            if (results != null && results.Count > 0)
            {
                var latitude = results[0]?["latitude"]?.GetValue<double>() ?? 0;
                var longitude = results[0]?["longitude"]?.GetValue<double>() ?? 0;

                return new double[] { latitude, longitude };
            }
            else
            {
                return new double[] { 0, 0 };
            }
        }
    }
}
}
