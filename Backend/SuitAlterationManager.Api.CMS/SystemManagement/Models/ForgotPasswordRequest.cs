using System.ComponentModel.DataAnnotations;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Models
{
	public class ForgotPasswordRequest
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
