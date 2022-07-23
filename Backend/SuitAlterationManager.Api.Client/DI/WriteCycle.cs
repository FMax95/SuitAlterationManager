using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Infrastructure.EF;

namespace SuitAlterationManager.Extensions.DI
{
    public static class WriteCycle
    {
        public static void AddWriteCycle(this IServiceCollection services, string dbConnectionString,
                                         Assembly repositoryInterfaces, Assembly repositoryImplementations, bool isDevelopment)
        {
            services.AddRepositories(repositoryInterfaces, repositoryImplementations);

            services.AddDbContext<SuitAlterationManagerContext>(builder =>
            {
                builder.UseSqlServer(dbConnectionString);

                builder.UseLazyLoadingProxies();

                if (!isDevelopment)
                    return;

                builder.LogTo(Console.WriteLine);
                builder.EnableDetailedErrors().EnableSensitiveDataLogging();
            });

            services.AddScoped<DbContext>(sp => sp.GetService<SuitAlterationManagerContext>());
        }

        private static void AddRepositories(this IServiceCollection services, Assembly interfaces, Assembly implementations)
        {
            var repositoryInterfaces = interfaces.DefinedTypes.Where(x => x.IsInterface && x.GetInterfaces()
                                                              .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>))).ToList();
            var repositoryImplementations = implementations.DefinedTypes.Where(x => x.GetInterfaces()
                                                           .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>))).ToList();

            foreach (var intefaceType in repositoryInterfaces)
                services.Add(new ServiceDescriptor(intefaceType, repositoryImplementations.First(x => x.GetInterfaces().Contains(intefaceType)), ServiceLifetime.Scoped));
        }
    }
}
