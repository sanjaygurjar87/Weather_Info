using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherInfoData.Models
{
    public class CityWeatherInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public string Description { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double Temperature { get; set; }
    }
}
