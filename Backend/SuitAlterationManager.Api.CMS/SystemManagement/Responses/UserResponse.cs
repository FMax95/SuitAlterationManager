using System;
using System.Collections.Generic;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public List<GroupResponse> Groups { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}