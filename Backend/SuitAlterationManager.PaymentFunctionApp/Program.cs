using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using SuitAlterationManager.Extensions;
using SuitAlterationManager.Extensions.DI;
using System;
using System.Reflection;

namespace PaymentFunctionApp
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    services.AddWriteCycle(context.Configuration.GetValue<string>("SqlConnectionString"),
                                   Assembly.Load("SuitAlterationManager.Domain"),
                                   Assembly.Load("SuitAlterationManager.Infrastructure"),
                                   false);
                    services.RegisterAllTypes();
                })
                .Build();

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo
                    .ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = Environment.GetEnvironmentVariable("ApplicationInsightKey") }, TelemetryConverter.Traces)
                    .CreateLogger();

                Log.ForContext<Program>().Fatal(ex, "Host terminated unexpectedly!");
            }
            finally
            {
                Serilog.Log.CloseAndFlush();
            }
        }
    }
}