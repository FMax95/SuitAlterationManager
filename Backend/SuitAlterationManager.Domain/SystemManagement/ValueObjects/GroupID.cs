using System;
using SuitAlterationManager.Domain.Base.Models;

namespace SuitAlterationManager.Domain.SystemManagement.ValueObjects
{
	public class GroupID : ID<Guid>
    {
        public GroupID() : base() { }

        public GroupID(Guid value) : base(value) { }
    }
}
