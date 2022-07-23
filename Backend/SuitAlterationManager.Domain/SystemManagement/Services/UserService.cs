using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.Repositories;

namespace SuitAlterationManager.Domain.SystemManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

    }
}
