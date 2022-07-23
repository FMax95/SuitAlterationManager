using System;
using System.Collections.Generic;

namespace SuitAlterationManager.Api.Client.SystemManagement.Models
{
    public class CreateUser
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<Guid> IdGroupList { get; set; }
    }
}
