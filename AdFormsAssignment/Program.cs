using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AdFormsAssignment
{
    /// <summary>
    /// Program class of the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            ConfigureLogger();
            Serilog.Log.Information("Application started");
            CreateHostBuilder(args).Build().Run();
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                             .Enrich.FromLogContext()
                             //.WriteTo.Console(new RenderedCompactJsonFormatter())
                             //.WriteTo.Debug(outputTemplate: DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"))
                             .WriteTo.File(path: @"Logs/Applog.txt", rollingInterval: RollingInterval.Day, outputTemplate:
        "{NewLine}{Timestamp:dd MMM yyyy hh:mm:ss tt} [{Level:u3}] Scope: {Properties:j} {Message:lj}{NewLine}{Exception}")
                             .CreateLogger();
        }

        /// <summary>
        /// CreateHostBuilder method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
