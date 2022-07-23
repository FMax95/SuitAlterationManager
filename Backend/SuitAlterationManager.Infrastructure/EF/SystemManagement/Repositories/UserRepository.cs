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

	}
}
