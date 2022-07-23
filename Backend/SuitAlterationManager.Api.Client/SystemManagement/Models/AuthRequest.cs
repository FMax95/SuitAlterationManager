using System.ComponentModel.DataAnnotations;

namespace SuitAlterationManager.Api.Client.SystemManagement.Models
{
    public class AuthRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}