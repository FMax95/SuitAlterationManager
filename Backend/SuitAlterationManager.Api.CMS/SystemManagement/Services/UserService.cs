using SuitAlterationManager.Api.CMS.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Services
{
    public class UserService : Interfaces.IUserService
    {
        private readonly Domain.SystemManagement.Services.Interfaces.IUserService userService;

        public UserService(Domain.SystemManagement.Services.Interfaces.IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<UserCreatedDTO> CreateUser(string email, string password, IEnumerable<Guid> groups,
                                                     DateTime? birthDate = null, string firstName = null,
                                                     string lastName = null, string image = null)
        {
            IEnumerable<GroupID> groupIDs = groups.Select(g => new GroupID(g));
            return await userService.CreateUser(email, password, groupIDs, birthDate, firstName, lastName, image);
        }

        public async Task UpdateUser(UserID userId, string email, bool isEnabled, IEnumerable<Guid> groups,
                                     DateTime? birthDate = null, string firstName = null, string lastName = null,
                                     string image = null)
        {
            IEnumerable<GroupID> groupIDs = groups.Select(g => new GroupID(g));
            await userService.UpdateUser(userId, email, isEnabled, groupIDs, birthDate, firstName, lastName, image);
        }
    }
}
