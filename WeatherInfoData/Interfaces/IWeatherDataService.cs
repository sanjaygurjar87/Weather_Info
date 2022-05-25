using System.Collections.Generic;
using System.Threading.Tasks;

using WeatherInfoData.Models;

namespace WeatherInfoData.Interfaces
{
    public interface IWeatherDataService
    {
        Task<CityWeatherInfo> GetWeatherInfoById(int cityId);

        Task<List<CityWeatherInfo>> GetWeatherInfoForAllCities();
    }
}
