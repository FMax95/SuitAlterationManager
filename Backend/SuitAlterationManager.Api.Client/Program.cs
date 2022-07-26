using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SuitAlterationManager.Api.Client
{
    public class Program
    {
        private static IConfigurationRoot configuration;

        public static async Task<int> Main(string[] args)
        {
            try
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var host = BuildHost(args);

                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo
                    .ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = configuration.GetValue<string>("ApplicationInsightKey") }, TelemetryConverter.Traces)
                    .CreateLogger();

                Log.ForContext<Program>().Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Serilog.Log.CloseAndFlush();
            }
        }

        private static IHost BuildHost(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseConfiguration(configuration);
                })
                .UseSerilog();
            return host.Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}