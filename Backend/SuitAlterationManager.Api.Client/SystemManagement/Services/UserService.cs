using SuitAlterationManager.Api.Client.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.SystemManagement.Services
{
    public class UserService : Interfaces.IUserService
    {
        private readonly Domain.SystemManagement.Services.Interfaces.IUserService userService;

        public UserService(Domain.SystemManagement.Services.Interfaces.IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<UserCreatedDTO> CreateUser(string email, string password, DateTime? birthDate = null,
                                                           string firstName = null, string lastName = null)
        {
            try
            {
                IEnumerable<GroupID> groupIDs = new List<GroupID>() { new GroupID(Guid.Parse(Group.VISITOR_ID)) };
                return await userService.CreateUser(email, password, groupIDs, birthDate, firstName, lastName);
            }
            catch (DomainException)
            {
                throw new DomainException(ErrorCodes.InvalidUser);
            }
        }

        public async Task UpdateUser(UserID userId, string email, bool isEnabled, DateTime? birthDate = null,
                                     string firstName = null, string lastName = null)
        {
            try
            {
                IEnumerable<GroupID> groupIDs = new List<GroupID>() { new GroupID(Guid.Parse(Group.VISITOR_ID)) };
                await userService.UpdateUser(userId, email, isEnabled, groupIDs, birthDate, firstName, lastName);
            }
            catch (DomainException)
            {
                throw new DomainException(ErrorCodes.InvalidUser);
            }
        }
    }
}
