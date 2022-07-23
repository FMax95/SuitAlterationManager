using System;
using System.Collections.Generic;
using System.Text;

namespace SuitAlterationManager.Domain.SystemManagement.DTO
{
    public class UserCreatedDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public List<Guid> Groups { get; set; }
    }
}
