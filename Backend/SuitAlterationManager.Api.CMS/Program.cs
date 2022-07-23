using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using SuitAlterationManager.Infrastructure.Logger;
using SuitAlterationManager.Infrastructure.Logger.Managers;
using Log = SuitAlterationManager.Extensions.DI.Log;

namespace SuitAlterationManager.Api.CMS
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
                LoggerServiceOptions loggerServiceOptions =
                    Log.GetLoggerOptions(configuration);
                Serilog.Log.Logger = new SerilogManager(loggerServiceOptions).Logger;
                Serilog.Log.Logger.Write(LogEventLevel.Fatal, "Error during Startup " + ex);
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
                    webBuilder.UseSentry();
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