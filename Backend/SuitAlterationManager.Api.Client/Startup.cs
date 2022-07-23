using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuitAlterationManager.Extensions.Attributes;
using SuitAlterationManager.Extensions.DI;
using SuitAlterationManager.Extensions.Middlewares;

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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigins", builder =>
                {
                    builder
                        .WithOrigins(
                            Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new string[] { })
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.AddEmailService(Configuration.GetSection("Email"));

            services.AddAuth(Configuration.GetSection("Auth"));

            services.AddWriteCycle(Configuration.GetConnectionString("Db"),
                Assembly.Load("SuitAlterationManager.Domain"),
                Assembly.Load("SuitAlterationManager.Infrastructure"),
                Environment.IsDevelopment());

            services.AddReadCycle(Configuration.GetConnectionString("Db"),
                Assembly.Load("SuitAlterationManager.Api.Client"));

            services.AddFileStorage(Configuration.GetSection("FileStorage"));

            services.AddLoggerService(Configuration);

            services.AddSwagger("v1", Configuration["ApiName"]);

            services.AddHttpContextAccessor();
            services.AddControllers(options =>
            {
                // The order parameter rappresents the attribute execution order.
                // If another attribute is executed at the same time (AllowExceptionAttribute), it'll be applied first.
                options.Filters.Add<DisallowExceptionAttribute>(order: 2);
            });

            // Base services and base repositories
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

            app.UseCors("AllowOrigins");

            // File Storage Service	
            app.UseFileStorage(Configuration["FileStorage:StorageRootPath"], "/contents",
                Configuration.GetSection("FileStorage"));

            app.UseRouting();

            app.UseMiddleware<AuthMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger(Configuration);
        }
    }
}