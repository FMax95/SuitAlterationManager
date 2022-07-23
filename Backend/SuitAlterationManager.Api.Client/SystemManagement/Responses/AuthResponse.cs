using System;

namespace SuitAlterationManager.Api.Client.SystemManagement.Models
{
    public class AuthResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
