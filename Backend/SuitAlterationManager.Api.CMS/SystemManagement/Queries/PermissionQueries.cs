using SqlKata;
using SqlKata.Execution;
using SuitAlterationManager.Api.CMS.SystemManagement.Responses;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Infrastructure.ReadCycle;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Queries
{
    public interface IPermissionQueries : IQueryService
    {
        Task<Dictionary<string, List<string>>> GetUserPermissionsAsync(UserID idUser);
    }

    public class PermissionQueries : IPermissionQueries
    {
        private readonly QueryFactory db;

        public PermissionQueries(QueryFactory db)
        {
            this.db = db;
        }

        public async Task<Dictionary<string, List<string>>> GetUserPermissionsAsync(UserID idUser)
        {
            Query query = db.Query("System.Group")
                .Join("System.UserGroup", "UserGroup.IdGroup", "Group.Id")
                .Join("System.GroupPermission", "GroupPermission.IdGroup", "Group.Id")
                .Join("System.Action", "Action.Id", "GroupPermission.IdAction")
                .Join("System.Context", "Context.Id", "Action.IdContext")
                .Where("UserGroup.IdUser", "=", idUser.Value.ToString())
                .Select(
                    "Context.Name as Context",
                    "Action.Name as Action"
                );

            IEnumerable<ContextActionResponse> userContextActions = await query.GetAsync<ContextActionResponse>();
            IEnumerable<string> contextsDistinct = userContextActions.Select(ca => ca.Context).Distinct();

            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            contextsDistinct.ToList().ForEach(ctx => result.Add(ctx, userContextActions.Where(uca => uca.Context == ctx).Select(uca => uca.Action).ToList()));
            return result;
        }
    }
}