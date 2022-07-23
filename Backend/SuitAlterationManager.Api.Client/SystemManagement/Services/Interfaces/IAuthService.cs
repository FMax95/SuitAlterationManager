using SuitAlterationManager.Api.Client.Base.Interfaces;
using SuitAlterationManager.Api.Client.SystemManagement.Models;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.SystemManagement.Services.Interfaces
{
    public interface IAuthService : IBaseApplicationService
    {
        public Task<AuthResponse> Authenticate(string email, string password);
        bool VerifyPassword(string insertedPassword, string userPassword);
    }
}
