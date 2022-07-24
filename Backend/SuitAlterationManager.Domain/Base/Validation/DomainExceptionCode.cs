using System;
using System.Collections.Generic;
using System.Text;

namespace SuitAlterationManager.Domain.Base.Validation
{
    public static class DomainExceptionCode
    {
        public const string MeasureNotVaild = "MeasureNotVaild";
        public const string CustomerEmailNotVaild = "CustomerEmailNotVaild";
        public const string CannotStartAlteration_NotPaid = "CannotStartAlteration_NotPaid";
        public const string CannotFinishAlteration_NotStarted = "CannotFinishAlteration_NotStarted";
    }
}
