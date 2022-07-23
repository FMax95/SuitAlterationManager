using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Repositories
{
	public interface IUserRepository : IRepository<User, UserID>
	{
    }
}
