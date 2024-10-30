using WeatherForecastApp.Services;

namespace WeatherForecastApp.Jobs
{
    public class WeatherJob
    {
        private readonly WeatherService weatherService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public WeatherJob(WeatherService service, IHttpContextAccessor contextAccessor)
        {
            weatherService = service;
            httpContextAccessor = contextAccessor;
        }

        public async Task CheckRainNotification()
        {
            var httpContext = httpContextAccessor.HttpContext;

            var city = httpContext?.Request.Cookies["LastCity"] ?? "Kyiv";
            var forecast = await weatherService.GetWeatherAsync(city);

            if (forecast.IsRainExpected)
            {
                Console.WriteLine($"⚠️ Rain is expected today in {city}!");
            }
        }
    }
}
