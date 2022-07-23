using System;
using SuitAlterationManager.Domain.Base.Models;

namespace SuitAlterationManager.Domain.SystemManagement.ValueObjects
{
	public class ActionID : ID<Guid>
    {
        public ActionID() : base() { }

        public ActionID(Guid value) : base(value) { }
    }
}
