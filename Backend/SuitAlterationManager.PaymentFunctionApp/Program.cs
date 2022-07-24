using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SuitAlterationManager.Extensions;
using SuitAlterationManager.Extensions.DI;
using System.Reflection;

namespace PaymentFunctionApp
{
    public class Program
    {
        public static void Main()
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
    }
}