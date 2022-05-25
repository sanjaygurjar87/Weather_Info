using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherInfoData.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public string Description { get; set; }
    }
}
