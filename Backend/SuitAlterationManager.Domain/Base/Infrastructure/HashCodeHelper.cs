using System.Collections.Generic;
using System.Linq;

namespace SuitAlterationManager.Domain.Base.Infrastructure
{
    internal static class HashCodeHelper
    {
        public static int CombineHashCodes(this IEnumerable<object> objects)
        {
            return objects.Aggregate(17, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
        }
    }
}
