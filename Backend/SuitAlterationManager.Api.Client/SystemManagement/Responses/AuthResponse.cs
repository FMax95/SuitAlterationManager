using System;

namespace SuitAlterationManager.Api.Client.SystemManagement.Responses
{
    public class AuthResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email{ get; set; }
        public string JwtToken { get; set; }
    }
}