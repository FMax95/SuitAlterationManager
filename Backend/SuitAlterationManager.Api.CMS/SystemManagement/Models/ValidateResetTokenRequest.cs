using System.ComponentModel.DataAnnotations;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Models
{
	public class ValidateResetTokenRequest
	{
		[Required]
		public string Token { get; set; }
	}
}
