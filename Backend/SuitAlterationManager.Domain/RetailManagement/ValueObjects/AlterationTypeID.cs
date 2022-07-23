using SuitAlterationManager.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuitAlterationManager.Domain.AlterationManagement.ValueObjects
{
    public class AlterationTypeID : ID<Guid>
    {
        public AlterationTypeID() : base() { }

        public AlterationTypeID(Guid value) : base(value) { }
    }
}
