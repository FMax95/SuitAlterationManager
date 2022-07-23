using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlKata.Execution;
using SuitAlterationManager.Api.CMS.SystemManagement.Responses;
using SuitAlterationManager.Infrastructure.ReadCycle;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Queries
{
    public interface IUserQueries : IQueryService
    {
        Task<List<UserResponse>> GetUsersAsync();
        Task<UserResponse> GetUserAsync(Guid idUser);
    }

    public class UserQueries : IUserQueries
    {
        private readonly QueryFactory db;

        public UserQueries(QueryFactory db)
        {
            this.db = db;
        }
        public async Task<List<UserResponse>> GetUsersAsync()
        {
            var query = db.Query("System.User")
              .Join("System.UserInformation", "User.Id", "UserInformation.IdUser")
              .Where("System.User.IsDeleted", false)
              .Select(
                "User.*", "UserInformation.*"
              );

            var result = await query.GetAsync<UserResponse>();

            return result.ToList();
        }

        public async Task<UserResponse> GetUserAsync(Guid idUser)
        {
            var groupQuery = db.Query("System.Group")
                               .Join("System.UserGroup", "Group.Id", "UserGroup.IdGroup")
                               .Where("UserGroup.IdUser", idUser)
                               .Select("Group.{Id, Name}");

            var query = db.Query("System.User")
              .Join("System.UserInformation", "User.Id", "UserInformation.IdUser")
              .Where("System.User.Id", idUser)
              .Select(
                "User.*", "UserInformation.*"
              );

            var result = (await db.GetMultipleAsync<UserResponse>(new[] { query, groupQuery }));
            var user = await result.ReadFirstAsync<UserResponse>();
            user.Groups = (await result.ReadAsync<GroupResponse>()).ToList();

            return user;
        }
    }
}