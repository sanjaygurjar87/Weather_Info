using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using WeatherInfoData.Interfaces;
using WeatherInfoData.Models;
using WeatherInfoData.Services;

namespace WeatherInfoAPI.Controllers
{
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherDataService _weatherDataService;

        public WeatherController(IWeatherDataService weatherDataService)
        {
            _weatherDataService = weatherDataService;
        }

        [HttpGet("api/weather/{cityId}")]
        public async Task<IActionResult> GetWeatherInfoById(string cityId)
        {
            if (string.IsNullOrEmpty(cityId))
                return BadRequest();

            int id;
            CityWeatherInfo cityWeatherInfo = new CityWeatherInfo();

            if (Int32.TryParse(cityId, out id))
                cityWeatherInfo = await _weatherDataService.GetWeatherInfoById(id);
            else
                return BadRequest($"Invalid cityId {cityId} found in request");

            return new JsonResult(cityWeatherInfo);
        }

        [HttpGet("api/weather")]
        public async Task<IActionResult> GetWeatherInfoForAllCities()
        {
            List<CityWeatherInfo> citiesWeatherInfo = await _weatherDataService.GetWeatherInfoForAllCities();

            return new JsonResult(citiesWeatherInfo);
        }
    }
}
