using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlKata.Execution;
using SuitAlterationManager.Api.CMS.SystemManagement.Responses;
using SuitAlterationManager.Infrastructure.ReadCycle;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Queries
{
    public interface IGroupQueries : IQueryService
    {
        Task<List<GroupResponse>> GetGroupsAsync();
    }

    public class GroupQueries : IGroupQueries
    {
        private readonly QueryFactory db;

        public GroupQueries(QueryFactory db)
        {
            this.db = db;
        }
        public async Task<List<GroupResponse>> GetGroupsAsync()
        {
            var query = db.Query("System.Group")
              .Select(
                "Group.Id", "Group.Name"
              );

            var result = await query.GetAsync<GroupResponse>();

            return result.ToList();
        }
    }
}