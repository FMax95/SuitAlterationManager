using System;

namespace SuitAlterationManager.Api.Client.SystemManagement.Responses
{
    public class UsersResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
    }
}