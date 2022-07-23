using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuitAlterationManager.Api.Client.Base.Interfaces;
using SuitAlterationManager.Domain.Base.Interfaces;
using System.Linq;
using System.Reflection;

namespace SuitAlterationManager.Api.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllTypes(this IServiceCollection services, IConfiguration Configuration)
        {
            var serviceInterfaces = Assembly.Load("SuitAlterationManager.Domain").DefinedTypes
                .Where(x => x.IsInterface && x.GetInterfaces()
                    .Any(i => i == typeof(IBaseService))).ToList();
            var serviceImplementation = Assembly.Load("SuitAlterationManager.Domain")
                .DefinedTypes
                .Where(x => x.GetInterfaces()
                    .Any(i => i == typeof(IBaseService)))
                .Where(x => x.IsClass).ToList();
            foreach (var intefaceType in serviceInterfaces)
                services.Add(new ServiceDescriptor(intefaceType,
                    serviceImplementation.First(x => x.GetInterfaces().Contains(intefaceType)),
                    ServiceLifetime.Scoped));


            var applicationServiceInterfaces = Assembly.Load("SuitAlterationManager.Api.Client").DefinedTypes
                .Where(x => x.IsInterface && x.GetInterfaces()
                    .Any(i => i == typeof(IBaseApplicationService))).ToList();
            var applicationServiceImplementation = Assembly.Load("SuitAlterationManager.Api.Client")
                .DefinedTypes
                .Where(x => x.GetInterfaces()
                    .Any(i => i == typeof(IBaseApplicationService)))
                .Where(x => x.IsClass).ToList();
            foreach (var intefaceType in applicationServiceInterfaces)
                services.Add(new ServiceDescriptor(intefaceType,
                    applicationServiceImplementation.First(x => x.GetInterfaces().Contains(intefaceType)),
                    ServiceLifetime.Scoped));
        }
    }
}