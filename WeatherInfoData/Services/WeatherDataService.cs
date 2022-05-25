using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using WeatherInfoData.Interfaces;
using WeatherInfoData.Models;

namespace WeatherInfoData.Services
{
    public class WeatherDataService : IWeatherDataService
    {
        const string DETAILS_NOT_FOUND = "Details not available";
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Nordea.db");

        private readonly IWeatherApiCallerService _weatherApiCallerService;

        public WeatherDataService(IWeatherApiCallerService weatherApiCallerService)
        {
            _weatherApiCallerService = weatherApiCallerService;

            // In realtime scenario this will not be required as we would have
            // a stable db that already contains city information
            using (var db = new LiteDatabase(dbPath))
            {
                var cities = db.GetCollection<City>("Cities");
                //cities.DeleteAll();
                var list = cities.Query().ToList();

                if (list.Count == 0)
                    SeedData(db, dbPath);
            }
        }

        public async Task<CityWeatherInfo> GetWeatherInfoById(int cityId)
        {
            var cityWeatherInfo = new CityWeatherInfo();
            var city = GetRequestedCityFromLiteDb(cityId);

            if (!city.Description.Equals(DETAILS_NOT_FOUND))
            {
                JsonWeatherResult result = await GetWeatherInfo(Convert.ToString(cityId));

                cityWeatherInfo.Id = cityId;
                cityWeatherInfo.Name = city.Name;
                cityWeatherInfo.CountryCode = city.CountryCode;
                cityWeatherInfo.Description = city.Description;
                cityWeatherInfo.Longitude = result.list[0].coord.lon;
                cityWeatherInfo.Latitude = result.list[0].coord.lat;
                cityWeatherInfo.Temperature = result.list[0].main.temp;
            }
            else
            {
                cityWeatherInfo = new CityWeatherInfo()
                {
                    Id = cityId,
                    Description = city.Description
                };
            }

            return cityWeatherInfo;
        }

        public async Task<List<CityWeatherInfo>> GetWeatherInfoForAllCities()
        {
            var cityWeatherInfos = new List<CityWeatherInfo>();
            var cities = GetAllCitiesFromLiteDB();
            JsonWeatherResult result = null;
            var requestIds = string.Empty;

            for(int i = 0; i < cities.Count; i++)
            {
                if (i == 0)
                    requestIds = Convert.ToString(cities[i].Id) + ",";
                else if (i == cities.Count - 1)
                    requestIds += Convert.ToString(cities[i].Id);
                else
                    requestIds += Convert.ToString(cities[i].Id) + ",";
            }

            if (!String.IsNullOrEmpty(requestIds))
                result = await GetWeatherInfo(requestIds);

            if (result != null)
            {
                foreach (var city in cities)
                { 
                    var currentCityWeatherData = result.list.Where(n => n.id == city.Id).FirstOrDefault();

                    if (currentCityWeatherData != null)
                    { 
                        var cityWeatherInfo = new CityWeatherInfo();

                        cityWeatherInfo.Id = city.Id;
                        cityWeatherInfo.Name = city.Name;
                        cityWeatherInfo.CountryCode = city.CountryCode;
                        cityWeatherInfo.Description = city.Description;
                        cityWeatherInfo.Longitude = currentCityWeatherData.coord.lon;
                        cityWeatherInfo.Latitude = currentCityWeatherData.coord.lat;
                        cityWeatherInfo.Temperature = currentCityWeatherData.main.temp;

                        cityWeatherInfos.Add(cityWeatherInfo);
                    }
                }
            }

            return cityWeatherInfos;
        }

        private City GetRequestedCityFromLiteDb(int cityId)
        { 
            var requestedCity = new City();

            using (var db = new LiteDatabase(dbPath))
            {
                var cities = db.GetCollection<City>("Cities");
                var list = cities.Query().ToList();

                requestedCity = list.Where(n => n.Id == cityId).FirstOrDefault();
                if (requestedCity != null)
                    return requestedCity;
                else
                    return new City() { Id = cityId, Description = DETAILS_NOT_FOUND };
            }
        }

        private List<City> GetAllCitiesFromLiteDB()
        {
            using (var db = new LiteDatabase(dbPath))
            {
                var cities = db.GetCollection<City>("Cities");
                return cities.Query().ToList();
            }
        }

        private async Task<JsonWeatherResult> GetWeatherInfo(string cityIds)
        {
            var weatherData = await _weatherApiCallerService.GetWeatherInfo(cityIds);
            return weatherData;
        }

        private void SeedData(LiteDatabase db, string dbPath)
        {
            var dbCities = db.GetCollection<City>("Cities");
            var cities = new List<City>();

            //City 1
            var city1 = new City()
            {
                Id = 2673722,
                Name = "Stockholm",
                CountryCode = "SE",
                Description = $"As Sweden’s capital is dramatically situated and spread over 14 islands," +
                              $" it is often called the “Venice of the North”. Stockholm is not only the seat of the Swedish" +
                              $" parliament and government, but also the country’s financial and economic centre. " +
                              $" It has recently boldly marketed itself as “The Capital of Scandinavia” – " +
                              $"proof, perhaps, that a little metropolitan arrogance is not alien to Stockholmers"
            };
            cities.Add(city1);

            //City 2
            var city2 = new City()
            {
                Id = 5695743,
                Name = "Gothenburg",
                CountryCode = "US",
                Description = $"Gothenburg is always on the go. It’s a working city with a mixture of hard physical" +
                              $" labour and international trade culture (both Volvo and SKF are based here), " +
                              $"as well as a busy port. Gothenburg is also a lively and vibrant city of culture." +
                              $"Major exhibitions, large conventions, large sporting events and concerts are often held here, " +
                              $"rather than in Stockholm"
            };

            cities.Add(city2);

            //City 3
            var city3 = new City()
            {
                Id = 5128581,
                Name = "New York",
                CountryCode = "US",
                Description = $"City in US"
            };
            cities.Add(city3);

            //City 4
            var city4 = new City()
            {
                Id = 5368361,
                Name = "Los Angeles",
                CountryCode = "US",
                Description = $"City in US"
            };
            cities.Add(city4);

            //City 5
            var city5 = new City()
            {
                Id = 1261481,
                Name = "New Delhi",
                CountryCode = "India",
                Description = $"City in India"
            };
            cities.Add(city5);

            dbCities.Insert(cities);
        }
    }
}
