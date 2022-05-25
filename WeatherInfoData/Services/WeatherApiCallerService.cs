using System;
using System.Net;
using System.Threading.Tasks;

using Newtonsoft.Json;

using WeatherInfoData.Interfaces;
using WeatherInfoData.Models;

namespace WeatherInfoData.Services
{
    public class WeatherApiCallerService : IWeatherApiCallerService
    {
        private static readonly WebClient client = new WebClient();

        public async Task<JsonWeatherResult> GetWeatherInfo(string cityIds)
        {
            string apiUrl = "https://api.openweathermap.org/data/2.5/group?appid=62945e2cfa4a17e0c665925f56dd3d2c&units=metric&id=" + cityIds;
            client.Headers.Add("Content-Type:application/json");
            client.Headers.Add("Accept:application/json");

            var result = await client.DownloadStringTaskAsync(new Uri(apiUrl));

            var weatherData = JsonConvert.DeserializeObject<JsonWeatherResult>(result);

            return weatherData;
        }
    }
}
