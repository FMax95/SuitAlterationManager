using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuitAlterationManager.Domain.Base.Interfaces;
using System.Linq;
using System.Reflection;

namespace SuitAlterationManager.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllTypes(this IServiceCollection services)
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
        }
    }
}