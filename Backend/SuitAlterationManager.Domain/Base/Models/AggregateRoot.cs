using SuitAlterationManager.Domain.Base.Interfaces;

namespace SuitAlterationManager.Domain.Base.Models
{
    public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot where T : ValueObject
    {
        public virtual byte[] DatabaseVersion { get; protected set; }
    }
}
