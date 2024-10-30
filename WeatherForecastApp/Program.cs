using Hangfire;
using WeatherForecastApp.Jobs;
using WeatherForecastApp.Services;

namespace WeatherForecastApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHangfire(config =>
                config.UseInMemoryStorage());

            builder.Services.AddSingleton<WeatherService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHangfireServer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            RecurringJob.AddOrUpdate<WeatherJob>(job => job.CheckRainNotification(), Cron.Daily);

            app.Run();
        }
    }
}
