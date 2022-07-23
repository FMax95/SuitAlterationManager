using SuitAlterationManager.Api.Client.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.SystemManagement.Services.Interfaces
{
    public interface IUserService : IBaseApplicationService
    {
        Task<UserCreatedDTO> CreateUser(string email, string password, DateTime? birthDate = null, string firstName = null, string lastName = null);
        Task UpdateUser(UserID userId, string email, bool isEnabled, DateTime? birthDate = null, string firstName = null, string lastName = null);
    }
}
