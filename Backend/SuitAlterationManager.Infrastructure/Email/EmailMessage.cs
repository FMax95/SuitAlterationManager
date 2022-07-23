using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.Email
{
	public class EmailMessage
	{
		private readonly IEmailService emailService;
		public string From { get; set; }
		public string FromDisplayName { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public List<IFormFile> Attachments { get; set; }
		public string Layout { get; protected set; } = "base.html";
		public string Template { get; protected set; } = "";
		public string interpolate(string text, Dictionary<string, string> data)
		{
			foreach (var entry in data)
			{
				text = text.Replace("{{" + entry.Key + "}}", entry.Value);
			}
			return text;
		}
		public async Task buildBody(Dictionary<string, string> data)
		{
			var templateBody = "";
			if (string.IsNullOrEmpty(Template))
			{
				// default template print all data without formatting
				foreach (var entry in data)
				{
					templateBody += "{{" + entry.Key + "}}\n";
				}
			}
			else
			{
				templateBody = await emailService.GetTemplateAsync(Template);
			}

			templateBody = interpolate(templateBody, data);

			var layoutBody = await emailService.GetTemplateAsync("Layouts/" + Layout);
			Body = layoutBody.Replace("{{body}}", templateBody);
		}
		public EmailMessage(IEmailService emailService)
		{
			this.emailService = emailService;
		}
	}
}
