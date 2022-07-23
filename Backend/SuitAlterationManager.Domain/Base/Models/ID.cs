using System.Collections.Generic;

namespace SuitAlterationManager.Domain.Base.Models
{
    public abstract class ID<T> : ValueObject
    {
        public virtual T Value { get; }

        protected ID()
        { }

        protected ID(T id) => Value = id;
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
