using System.Threading.Tasks;
using SuitAlterationManager.Domain.Base.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SuitAlterationManager.Domain.Base.Interfaces
{
    public interface IRepository<T, in TId> where T : class, IAggregateRoot where TId : ValueObject
    {
        T Get(TId id);
        ValueTask<T> GetAsync(TId id);
        EntityEntry<T> Add(T entity);
        ValueTask<EntityEntry<T>> AddAsync(T entity);
        EntityEntry<T> Attach(T entity);
        EntityEntry<T> Update(T entity);
        EntityEntry<T> Remove(TId id);
        EntityEntry<T> Remove(T entity);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        bool Exists(TId id);
        Task<bool> ExistsAsync(TId id);
    }
}
