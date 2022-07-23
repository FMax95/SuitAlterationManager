using System.ComponentModel.DataAnnotations;

namespace SuitAlterationManager.Api.Client.SystemManagement.Models
{
	public class ValidateResetTokenRequest
	{
		[Required]
		public string Token { get; set; }
	}
}
