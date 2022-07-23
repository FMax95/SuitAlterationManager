using SuitAlterationManager.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuitAlterationManager.Domain.SystemManagement.ValueObjects
{
    public class UserID : ID<Guid>
    {
        public UserID() : base() { }

        public UserID(Guid value) : base(value) { }
    }
}
