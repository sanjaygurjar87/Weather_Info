using System.Threading.Tasks;

using WeatherInfoData.Models;

namespace WeatherInfoData.Interfaces
{
    public interface IWeatherApiCallerService
    {
        Task<JsonWeatherResult> GetWeatherInfo(string cityIds);
    }
}
