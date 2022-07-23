using System;
using System.Collections.Generic;
using System.Linq;
using SuitAlterationManager.Domain.Base.Infrastructure;

namespace SuitAlterationManager.Domain.Base.Models
{
    public abstract class Entity<T> where T : ValueObject
    {
        public virtual T Id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity<T> other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return GetRealType() == other.GetRealType() && Id == other.Id;
        }
        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(Entity<T> a, Entity<T> b) => !(a == b);
        public override int GetHashCode() => $"{GetRealType()}{Id}".GetHashCode();
        protected Type GetRealType()
        {
            var type = GetType();
            return type.ToString().Contains("Castle.Proxies.") ? type.BaseType : type;
        }
    }

    public abstract class EntityWithCompositeId<T> : Entity<T> where T : ValueObject
    {
        /// <summary>
        /// When overriden in a derived class, gets all components of the identity of the entity.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetIdentityComponents();
        public override bool Equals(object obj)
        {
            if (!(obj is EntityWithCompositeId<T> other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (GetRealType() != other.GetRealType())
                return false;
            return GetIdentityComponents().SequenceEqual(other.GetIdentityComponents());
        }

        public override int GetHashCode() =>
           GetIdentityComponents().CombineHashCodes();
    }
}
