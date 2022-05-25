using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

using WeatherInfoData.Interfaces;
using WeatherInfoData.Models;
using WeatherInfoData.Services;

namespace WeatherInfoDataTests
{
    [TestClass]
    public class WeatherDataServiceTests
    {
        [TestMethod]
        public async Task GetWeatherInfoByIdShouldGetWeatherDataForGivenCityId()
        {
            IWeatherApiCallerService weatherApiCallerService = Substitute.For<IWeatherApiCallerService>();
            WeatherDataService weatherDataService = new WeatherDataService(weatherApiCallerService);
            CityWeatherInfo expectedCityWeatherInfo = new CityWeatherInfo()
            { 
                Id = 2673722,
                Name = "Stockholm",
                CountryCode = "SE",
                Description = $"As Sweden’s capital is dramatically situated and spread over 14 islands," +
                              $" it is often called the “Venice of the North”. Stockholm is not only the seat of the Swedish" +
                              $" parliament and government, but also the country’s financial and economic centre. " +
                              $" It has recently boldly marketed itself as “The Capital of Scandinavia” – " +
                              $"proof, perhaps, that a little metropolitan arrogance is not alien to Stockholmers",
                Latitude = 12.34,
                Longitude = 13.45,
                Temperature = 15
            };

            CityWeatherData[] mockCityWeatherData = new CityWeatherData[] 
            { 
                new CityWeatherData() 
                { 
                    id = 2673722, 
                    coord = new Coordinates() 
                    { 
                        lat = 12.34, 
                        lon = 13.45 
                    }, 
                    main = new MainTempDetails() 
                    { 
                        temp = 15 
                    } 
                } 
            };

            weatherApiCallerService.GetWeatherInfo(Arg.Any<string>()).Returns(new JsonWeatherResult() {
                cnt = 1,
                list = mockCityWeatherData
            });

            var result = await weatherDataService.GetWeatherInfoById(2673722);

            Assert.AreEqual(expectedCityWeatherInfo.Id, result.Id);
            Assert.AreEqual(expectedCityWeatherInfo.Name, result.Name);
            Assert.AreEqual(expectedCityWeatherInfo.CountryCode, result.CountryCode);
            Assert.AreEqual(expectedCityWeatherInfo.Description, result.Description);
            Assert.AreEqual(expectedCityWeatherInfo.Latitude, result.Latitude);
            Assert.AreEqual(expectedCityWeatherInfo.Longitude, result.Longitude);
            Assert.AreEqual(expectedCityWeatherInfo.Temperature, result.Temperature);
        }

        [TestMethod]
        public async Task GetWeatherInfoByIdShouldNotReturnDataWhenCityIdIsMissingInLiteDB()
        {
            IWeatherApiCallerService weatherApiCallerService = Substitute.For<IWeatherApiCallerService>();
            WeatherDataService weatherDataService = new WeatherDataService(weatherApiCallerService);
            const string detailsNotFound = "Details not available";

            CityWeatherInfo expectedCityWeatherInfo = new CityWeatherInfo()
            {
                Id = 1,
                Description = detailsNotFound
            };

            var result = await weatherDataService.GetWeatherInfoById(1);

            Assert.AreEqual(expectedCityWeatherInfo.Id, result.Id);
            Assert.AreEqual(detailsNotFound, result.Description);
        }

        [TestMethod]
        public async Task GetWeatherInfoForAllCitiesShouldReturnWeatherInfoForAllCitiesInLiteDB()
        {
            IWeatherApiCallerService weatherApiCallerService = Substitute.For<IWeatherApiCallerService>();
            WeatherDataService weatherDataService = new WeatherDataService(weatherApiCallerService);
            List<CityWeatherInfo> expectedWeatherInfos = TestWeatherInfos();
            CityWeatherData[] mockCityWeatherData = MockWeatherData();

            weatherApiCallerService.GetWeatherInfo(Arg.Any<string>()).Returns(new JsonWeatherResult()
            {
                cnt = 1,
                list = mockCityWeatherData
            });

            var result = await weatherDataService.GetWeatherInfoForAllCities();

            Assert.AreEqual(5, result.Count);
            for(int i = 0; i < expectedWeatherInfos.Count; i++)
            {
                Assert.AreEqual(expectedWeatherInfos[i].Id, result[i].Id);
                Assert.AreEqual(expectedWeatherInfos[i].Name, result[i].Name);
                Assert.AreEqual(expectedWeatherInfos[i].CountryCode, result[i].CountryCode);
                Assert.AreEqual(expectedWeatherInfos[i].Description, result[i].Description);
                Assert.AreEqual(expectedWeatherInfos[i].Latitude, result[i].Latitude);
                Assert.AreEqual(expectedWeatherInfos[i].Longitude, result[i].Longitude);
                Assert.AreEqual(expectedWeatherInfos[i].Temperature, result[i].Temperature);
            }
        }

        private List<CityWeatherInfo> TestWeatherInfos()
        {
            List<CityWeatherInfo> cityWeatherInfos = new List<CityWeatherInfo>();

            CityWeatherInfo stockholm = new CityWeatherInfo()
            {
                Id = 2673722,
                Name = "Stockholm",
                CountryCode = "SE",
                Description = $"As Sweden’s capital is dramatically situated and spread over 14 islands," +
                              $" it is often called the “Venice of the North”. Stockholm is not only the seat of the Swedish" +
                              $" parliament and government, but also the country’s financial and economic centre. " +
                              $" It has recently boldly marketed itself as “The Capital of Scandinavia” – " +
                              $"proof, perhaps, that a little metropolitan arrogance is not alien to Stockholmers",
                Latitude = 12.34,
                Longitude = 13.45,
                Temperature = 15
            };

            CityWeatherInfo gothenburg = new CityWeatherInfo()
            {
                Id = 5695743,
                Name = "Gothenburg",
                CountryCode = "US",
                Description = $"Gothenburg is always on the go. It’s a working city with a mixture of hard physical" +
                              $" labour and international trade culture (both Volvo and SKF are based here), " +
                              $"as well as a busy port. Gothenburg is also a lively and vibrant city of culture." +
                              $"Major exhibitions, large conventions, large sporting events and concerts are often held here, " +
                              $"rather than in Stockholm",
                Latitude = 34.5,
                Longitude = 54.34,
                Temperature = 18
            };

            CityWeatherInfo newYork = new CityWeatherInfo()
            {
                Id = 5128581,
                Name = "New York",
                CountryCode = "US",
                Description = $"City in US",
                Latitude = 84.5,
                Longitude = 94.34,
                Temperature = 14
            };

            CityWeatherInfo losAngeles = new CityWeatherInfo()
            {
                Id = 5368361,
                Name = "Los Angeles",
                CountryCode = "US",
                Description = $"City in US",
                Latitude = 93.5,
                Longitude = 80.34,
                Temperature = 14
            };

            CityWeatherInfo newDelhi = new CityWeatherInfo()
            {
                Id = 1261481,
                Name = "New Delhi",
                CountryCode = "India",
                Description = $"City in India",
                Latitude = 103.5,
                Longitude = 200.34,
                Temperature = 25
            };

            cityWeatherInfos.Add(newDelhi);
            cityWeatherInfos.Add(stockholm);
            cityWeatherInfos.Add(newYork);
            cityWeatherInfos.Add(losAngeles);
            cityWeatherInfos.Add(gothenburg);

            return cityWeatherInfos;
        }

        private CityWeatherData[] MockWeatherData()
        {
            CityWeatherData[] mockCityWeatherData = new CityWeatherData[]
            {
                new CityWeatherData()
                {
                    id = 2673722,
                    coord = new Coordinates()
                    {
                        lat = 12.34,
                        lon = 13.45
                    },
                    main = new MainTempDetails()
                    {
                        temp = 15
                    }
                },
                new CityWeatherData()
                {
                    id = 5695743,
                    coord = new Coordinates()
                    {
                        lat = 34.5,
                        lon = 54.34
                    },
                    main = new MainTempDetails()
                    {
                        temp = 18
                    }
                },
                new CityWeatherData()
                {
                    id = 5128581,
                    coord = new Coordinates()
                    {
                        lat = 84.5,
                        lon =  94.34
                    },
                    main = new MainTempDetails()
                    {
                        temp = 14
                    }
                },
                new CityWeatherData()
                {
                    id = 5368361,
                    coord = new Coordinates()
                    {
                        lat = 93.5,
                        lon =  80.34
                    },
                    main = new MainTempDetails()
                    {
                        temp = 14
                    }
                },
                new CityWeatherData()
                {
                    id = 1261481,
                    coord = new Coordinates()
                    {
                        lat = 103.5,
                        lon = 200.34
                    },
                    main = new MainTempDetails()
                    {
                        temp = 25
                    }
                }
            };

            return mockCityWeatherData;
        }
    }
}
