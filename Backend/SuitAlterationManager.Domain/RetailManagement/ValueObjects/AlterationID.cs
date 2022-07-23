using SuitAlterationManager.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuitAlterationManager.Domain.AlterationManagement.ValueObjects
{
    public class AlterationID : ID<Guid>
    {
        public AlterationID() : base() { }

        public AlterationID(Guid value) : base(value) { }
    }
}
