using Newtonsoft.Json.Linq;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Services
{
    public class WeatherService
    {
        private readonly string apiKey = "oLZoVZfeNTxpnB7NpselKrN7FTvrS3Ym";
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<WeatherForecastModel> GetWeatherAsync(string city)
        {
            var locationKey = await GetLocationKeyAsync(city);

            var forecastUrl = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{locationKey}?apikey={apiKey}&metric=true";
            var response = await httpClient.GetStringAsync(forecastUrl);

            var data = JObject.Parse(response);
            var forecast = data["DailyForecasts"][0];
            var weatherText = forecast["Day"]["IconPhrase"].ToString();
            var temperature = (double)forecast["Temperature"]["Maximum"]["Value"];
            var isRainExpected = weatherText.Contains("Rain");

            return new WeatherForecastModel
            {
                CityName = city,
                WeatherText = weatherText,
                Temperature = temperature,
                IsRainExpected = isRainExpected
            };
        }

        private async Task<string> GetLocationKeyAsync(string city)
        {
            var locationUrl = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={apiKey}&q={city}";
            var response = await httpClient.GetStringAsync(locationUrl);
            var data = JArray.Parse(response);
            return data[0]["Key"].ToString();
        }
    }
}
