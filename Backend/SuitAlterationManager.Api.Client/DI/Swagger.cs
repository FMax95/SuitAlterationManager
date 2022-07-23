using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SuitAlterationManager.Extensions.DI
{
    public static class Swagger
    {
        public static void AddSwagger(this IServiceCollection services, string documentVersion, string documentTitle, Func<SwaggerGenOptions, SwaggerGenOptions>? lambda = null)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(documentVersion, new OpenApiInfo
                {
                    Title = documentTitle,
                    Version = documentVersion
                });
                c.CustomSchemaIds(s => s.FullName);
                c.AddSwaggerJwtAuthentication();

                if (lambda != null)
                {
                    c = lambda(c);
                }
            });
        }

        public static void AddSwaggerJwtAuthentication(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }

        public static void UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = configuration["ApiName"];
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("v1/swagger.json", "Specification 1");
            });
        }
    }
}
