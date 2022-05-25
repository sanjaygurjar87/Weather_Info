
namespace WeatherInfoData.Models
{
    public class JsonWeatherResult
    {
        public int cnt { get; set; }

        public CityWeatherData[] list { get; set; }
    }

    public class CityWeatherData
    {
        public int id { get; set; }

        public Coordinates coord { get; set; }

        public  MainTempDetails main { get; set; }
    }

    public class Coordinates
    {
        public double lon { get; set; }

        public double lat { get; set; }
    }

    public class MainTempDetails
    {
        public double temp { get; set; }

        public double feels_like { get; set; }

        public double temp_min { get; set; }

        public double temp_max { get; set; }

        public double pressure { get; set; }

        public double humidity { get; set; }
    }
}
