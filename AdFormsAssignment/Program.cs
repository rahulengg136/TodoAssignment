using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

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
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception exp)
            {
                Log.Error($"Something went wrong in starting the app . Message {exp.Message}. Stacktrace : {exp.StackTrace}");
            }
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
