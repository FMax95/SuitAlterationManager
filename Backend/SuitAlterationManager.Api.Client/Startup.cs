using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuitAlterationManager.Api.Client.DI;
using SuitAlterationManager.Api.Middlewares;

namespace SuitAlterationManager.Api.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWriteCycle(Configuration.GetConnectionString("Db"),
                Assembly.Load("SuitAlterationManager.Domain"),
                Assembly.Load("SuitAlterationManager.Infrastructure"),
                Environment.IsDevelopment());

            services.AddReadCycle(Configuration.GetConnectionString("Db"),
                Assembly.Load("SuitAlterationManager.Api.Client"));

            services.AddSwagger("v1", Configuration["ApiName"]);
            services.AddJWTAuth();

            services.AddAutoMapper(typeof(Startup));

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.RegisterAllTypes(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger(Configuration);
        }
    }
}