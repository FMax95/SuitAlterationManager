using System;
using System.Collections.Generic;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Responses
{
    public class AuthResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email{ get; set; }
        public string Token { get; set; }
        public Dictionary<string, List<string>> Permissions { get; set; }
    }
}