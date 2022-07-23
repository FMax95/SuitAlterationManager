using System;
using SuitAlterationManager.Domain.Base.Models;

namespace SuitAlterationManager.Domain.SystemManagement.ValueObjects
{
	public class RefreshTokenID: ID<Guid>
    {
        public RefreshTokenID() : base() { }

        public RefreshTokenID(Guid value) : base(value) { }
    }
}