using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Repositories
{
	public class UserRepository : BaseRepository<User, UserID>, IUserRepository
	{
		private readonly DbSet<User> users;
		public UserRepository(DbContext context) : base(context)
		{
			users = context.Set<User>();
		}

		public override User Get(UserID id)
		{
			var user = users.Find(id);

			if (user is null)
				return null;
			return user;
		}

		public override async ValueTask<User> GetAsync(UserID id)
		{
			var user = await users.FindAsync(id);

			if (user is null)
				return null;
			return user;
		}

		public async Task<User> GetByEmailAsync(string email)
		{
			var user = await users.Where(x=>x.IsDeleted == false).SingleOrDefaultAsync(u => u.Email == email);

			if (user is null)
				return null;

			return user;
		}

		public User GetByEmail(string email) {
			return users.Where(x => x.IsDeleted == false).SingleOrDefault(u => u.Email == email);
		}
        
		public bool ExistsWithEmail(string email, UserID differentThan = null)
        {
			return users.Where(x => x.IsDeleted == false).Any(u => u.Email == email && u.Id != differentThan);
		}
		
		public Task<bool> ExistsWithEmailAsync(string email, UserID differentThan = null) =>
			 users.Where(x => x.IsDeleted == false).AnyAsync(u => u.Email == email && u.Id != differentThan);

		public async Task<User> GetByRefreshTokenAsync(string refreshToken)
		{
			var user = await users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

			if (user is null)
				return null;

			return user;
		}

		public async Task<User> GetByResetTokenAsync(string resetToken)
		{
			var user = await users.SingleOrDefaultAsync(u => u.ResetToken == resetToken);

			if (user is null)
				return null;

			return user;
		}
	}
}
