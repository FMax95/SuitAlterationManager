using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Compilers;
using SqlKata.Execution;
using SuitAlterationManager.Infrastructure.ReadCycle;

namespace SuitAlterationManager.Api.Client.DI
{
    public static class ReadCycle
    {
        public static void AddReadCycle(this IServiceCollection services, string dbConnectionString, Assembly queriesAssembly)
        {
            services.AddScoped(_ => (IDbConnection)new SqlConnection(dbConnectionString));

            services.AddScoped(_ =>
            {
                var compiler = new SqlServerCompiler();
                IDbConnection connection = new SqlConnection(dbConnectionString);
                return new QueryFactory(connection, compiler);
            });

            services.AddQueries(queriesAssembly);
        }

        private static void AddQueries(this IServiceCollection services, Assembly assembly)
        {
            var serviceInterfaces = assembly.DefinedTypes.Where(x => x.IsInterface && x.GetInterfaces()
                   .Any(i => i == typeof(IQueryService))).ToList();

            var serviceImplementation = assembly.DefinedTypes.Where(x => x.GetInterfaces()
                    .Any(i => i == typeof(IQueryService))).Where(x => x.IsClass).ToList();

            foreach (var intefaceType in serviceInterfaces)
                services.Add(new ServiceDescriptor(intefaceType, serviceImplementation.First(x => x.GetInterfaces().Contains(intefaceType)), ServiceLifetime.Scoped));
        }
    }
}
