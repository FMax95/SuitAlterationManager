using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;
using SuitAlterationManager.Domain.Base.Models;

namespace SuitAlterationManager.Infrastructure.EF
{
	public abstract class BaseRepository<T, TId> where T : AggregateRoot<TId> where TId : ValueObject
    {
        protected readonly DbContext context;
        protected BaseRepository(DbContext context) =>
            this.context = context;
        public virtual T Get(TId id) => context.Find<T>(id);
        public virtual ValueTask<T> GetAsync(TId id) => context.FindAsync<T>(id);
        public virtual EntityEntry<T> Add(T entity) => context.Add(entity);
        public virtual ValueTask<EntityEntry<T>> AddAsync(T entity) => context.AddAsync(entity);
        public virtual EntityEntry<T> Attach(T entity) => context.Attach(entity);
        public virtual EntityEntry<T> Update(T entity) => context.Update(entity);
        public virtual EntityEntry<T> Remove(TId id)
        {
            var entity = context.Find<T>(id);
            return entity != null ? context.Remove(entity) : null;
        }
        public virtual EntityEntry<T> Remove(T entity) => context.Remove(entity);

        public virtual int SaveChanges() => context.SaveChanges();
        public virtual Task<int> SaveChangesAsync() => context.SaveChangesAsync();
        public virtual bool Exists(TId id) => context.Set<T>().Count(e => e.Id == id) > 0;
        public virtual async Task<bool> ExistsAsync(TId id) => await context.Set<T>().CountAsync(e => e.Id == id) > 0;
       
    }
}
