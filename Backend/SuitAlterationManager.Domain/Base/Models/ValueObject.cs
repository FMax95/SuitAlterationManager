using System.Collections.Generic;
using System.Linq;
using SuitAlterationManager.Domain.Base.Infrastructure;

namespace SuitAlterationManager.Domain.Base.Models
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;
            var vo = obj as ValueObject;
            return vo != null && GetEqualityComponents().SequenceEqual(vo.GetEqualityComponents());
        }
        public override int GetHashCode() =>
            GetEqualityComponents().CombineHashCodes();
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(ValueObject a, ValueObject b) =>
            !(a == b);
    }
}
