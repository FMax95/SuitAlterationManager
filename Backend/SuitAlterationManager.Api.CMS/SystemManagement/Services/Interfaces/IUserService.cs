using SuitAlterationManager.Api.CMS.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Services.Interfaces
{
    public interface IUserService : IBaseApplicationService
    {
        Task<UserCreatedDTO> CreateUser(string email, string password, IEnumerable<Guid> groups, DateTime? birthDate = null, string firstName = null, string lastName = null, string image = null);
        Task UpdateUser(UserID userId, string email, bool isEnabled, IEnumerable<Guid> groups, DateTime? birthDate = null, string firstName = null, string lastName = null, string image = null);
    }
}
