using SuitAlterationManager.Api.CMS.SystemManagement.Queries;
using SuitAlterationManager.Api.CMS.SystemManagement.Service.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionQueries permissionQueries;

        public PermissionService(IPermissionQueries permissionQueries)
        {
            this.permissionQueries = permissionQueries;
        }

        public async Task<bool> ValidatePermission(UserID idUser, string actionName, string contextName)
        {
            if (idUser == null)
                return false;

            Dictionary<string, List<string>> userPermissions = await permissionQueries.GetUserPermissionsAsync(idUser);
            return userPermissions.ContainsKey(contextName) && userPermissions[contextName].Contains(actionName);
        }
    }
}
