using System;
using SuitAlterationManager.Domain.Base.Models;

namespace SuitAlterationManager.Domain.SystemManagement.ValueObjects
{
	public class ContextID : ID<Guid>
    {
        public ContextID() : base() { }

        public ContextID(Guid value) : base(value) { }
    }
}
