using System;

namespace SuitAlterationManager.Api.Client.SystemManagement.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
