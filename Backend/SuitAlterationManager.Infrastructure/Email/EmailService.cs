using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.Email
{
	public class EmailTemplates
	{
		public const string PasswordRecovery = "recovery_password.html";
	}
	public interface IEmailService
	{
		Task SendAsync(EmailMessage message);
		Task<string> GetTemplateAsync(string templateName);
	}
	public class EmailService : IEmailService
	{
		private readonly EmailServiceOptions smtpOptions;
		public EmailService(IOptions<EmailServiceOptions> smtpOptions)
		{
			this.smtpOptions = smtpOptions.Value;
		}
		
		public async Task<string> GetTemplateAsync(string templateName)
		{
			var templatePath = Path.Combine(smtpOptions.CustomTemplatesFolder, templateName);

			// if not exist need to fallback into standard folder
			if (!File.Exists(templatePath))
				templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Email/Templates", templateName);

			return await File.ReadAllTextAsync(templatePath);
		}

		public async Task SendAsync(EmailMessage message)
		{
			// create message
			var email = new MimeMessage();

			var address = MailboxAddress.Parse(message.From ?? smtpOptions.From);
			address.Name = message.FromDisplayName ?? smtpOptions.FromDisplayName;

			email.From.Add(address);
			email.To.Add(MailboxAddress.Parse(message.To));
			email.Subject = message.Subject;

			message.Body = message.interpolate(message.Body, new Dictionary<string, string>() { 
				{ "VirtualPlaceUri", smtpOptions.VirtualPlaceUrl } 
			});
			email.Body = new TextPart(TextFormat.Html) { Text = message.Body };

			// send email
			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(smtpOptions.Host, smtpOptions.Port, SecureSocketOptions.StartTlsWhenAvailable);
			await smtp.AuthenticateAsync(smtpOptions.Username, smtpOptions.Password);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
	}
}
