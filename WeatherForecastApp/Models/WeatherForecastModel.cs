namespace WeatherForecastApp.Models
{
    public class WeatherForecastModel
    {
        public string CityName { get; set; }
        public string WeatherText { get; set; }
        public double Temperature { get; set; }
        public bool IsRainExpected { get; set; }
    }
}
