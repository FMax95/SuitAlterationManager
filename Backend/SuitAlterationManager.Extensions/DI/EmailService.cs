using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuitAlterationManager.Infrastructure.Email;

namespace SuitAlterationManager.Extensions.DI
{
	public static class EmailService
	{
		public static void AddEmailService(this IServiceCollection services, IConfigurationSection section)
		{
			services.Configure<EmailServiceOptions>(section);
			services.AddScoped<IEmailService, Infrastructure.Email.EmailService>();
		}
	}
}
