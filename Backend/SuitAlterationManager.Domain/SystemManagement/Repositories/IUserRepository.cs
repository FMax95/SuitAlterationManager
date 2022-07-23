using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Repositories
{
	public interface IUserRepository : IRepository<User, UserID>
	{
		Task<bool> ExistsWithEmailAsync(string email, UserID differentThan = null);
		Task<User> GetByEmailAsync(string email);
        Task<User> GetByRefreshTokenAsync(string refreshToken);
        Task<User> GetByResetTokenAsync(string resetToken);
    }
}
