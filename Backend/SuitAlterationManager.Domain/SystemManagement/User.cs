using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Domain.Base.Models;
using System;

namespace SuitAlterationManager.Domain.SystemManagement
{
    public class User : AggregateRoot<UserID>
    {
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        
        public User()
        {
            this.IsEnabled = true;
            this.UpdateDate = DateTime.Now;
        }
    }
}
