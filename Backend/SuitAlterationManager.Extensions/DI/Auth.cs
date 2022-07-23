using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using SuitAlterationManager.Domain.SystemManagement.Services.Auth;
using SuitAlterationManager.Infrastructure.Auth;

namespace SuitAlterationManager.Extensions.DI
{
    public static class Auth
	{
		public static void AddAuth(this IServiceCollection services, IConfigurationSection authSection)
		{
			services.Configure<AuthOptions>(authSection);
			var authOptions = authSection.Get<AuthOptions>();

			switch (authOptions.Type)
			{
				case "JWT":
					services.AddJWTAuth(authOptions);
					break;
				default:
					break;
			}

            services.AddScoped<IAuthService, AuthService>();
        }

		private static void AddJWTAuth(this IServiceCollection service, AuthOptions authOptions)
		{
			service
				.AddAuthentication(auth =>
				{
					auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(x =>
				{
					x.RequireHttpsMetadata = false;
					x.SaveToken = true;
					x.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(authOptions.Secret)),
						ValidateIssuer = false,
						ValidateAudience = false,
						ClockSkew = TimeSpan.Zero
					};
					x.Events = new JwtBearerEvents
					{
						OnAuthenticationFailed = context =>
						{
							if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
								context.Response.Headers.Add("Token-Expired", "true");

							return Task.CompletedTask;
						}
					};
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
	}
}
