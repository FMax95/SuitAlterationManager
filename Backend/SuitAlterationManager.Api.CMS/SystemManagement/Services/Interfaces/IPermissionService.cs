using SuitAlterationManager.Api.CMS.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Service.Interfaces
{
    public interface IPermissionService : IBaseApplicationService
    {
        /// <summary>
        /// Check the user permissions based on his groups, a context and an action.
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="contextName"></param>
        /// <param name="actionName"></param>
        /// <returns>true if valid</returns>
        Task<bool> ValidatePermission(UserID idUser, string contextName, string actionName);
    }
}
