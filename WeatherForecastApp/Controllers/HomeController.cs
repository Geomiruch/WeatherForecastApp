using Microsoft.AspNetCore.Mvc;
using WeatherForecastApp.Services;

namespace WeatherForecastApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherService weatherService = new WeatherService();

        public async Task<IActionResult> Index(string city = null)
        {
            if (string.IsNullOrEmpty(city))
            {
                Request.Cookies.TryGetValue("LastCity", out city);
                if (string.IsNullOrEmpty(city))
                {
                    city = "Kyiv"; 
                }
            }

            var forecast = await weatherService.GetWeatherAsync(city);

            Response.Cookies.Append("LastCity", city, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30)
            });

            return View(forecast);
        }
    }
}
