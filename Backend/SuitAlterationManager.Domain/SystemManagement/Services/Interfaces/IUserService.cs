using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Services.Interfaces
{
    public interface IUserService : IBaseService
    {
        Task<UserCreatedDTO> CreateUser(string email, string password, IEnumerable<GroupID> groupIDs, DateTime? birthDate = null, string firstName = null, string lastName = null, string image = null);
        Task DisableUser(UserID userId);
        Task UpdateUser(UserID userId, string email, bool isEnabled, IEnumerable<GroupID> groupIDs, DateTime? birthDate = null, string firstName = null, string lastName = null, string image = null);
    }
}
