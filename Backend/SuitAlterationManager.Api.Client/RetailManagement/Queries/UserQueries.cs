using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlKata.Execution;
using SuitAlterationManager.Api.Client.SystemManagement.Responses;
using SuitAlterationManager.Infrastructure.ReadCycle;

namespace SuitAlterationManager.Api.Client.SystemManagement.Queries
{
    public interface IUserQueries : IQueryService
    {
        Task<List<UsersResponse>> GetUsersAsync();
        Task<UsersResponse> GetUserAsync(Guid idUser);
    }

    public class UserQueries : IUserQueries
    {
        private readonly QueryFactory db;

        public UserQueries(QueryFactory db)
        {
            this.db = db;
        }
        public async Task<List<UsersResponse>> GetUsersAsync()
        {
            var query = db.Query("System.User")
              .Join("System.UserInformation", "User.Id", "UserInformation.IdUser")
              .Select(
                "User.*", "UserInformation.*"
              );

            var result = await query.GetAsync<UsersResponse>();

            return result.ToList();
        }

        public async Task<UsersResponse> GetUserAsync(Guid idUser)
        {
            var query = db.Query("System.User")
              .Join("System.UserInformation", "User.Id", "UserInformation.IdUser")
              .Where("System.User.Id", idUser)
              .Select(
                "User.*", "UserInformation.*"
              );

            var result = (await query.GetAsync<UsersResponse>()).ToList().Single();

            return result;
        }
    }
}