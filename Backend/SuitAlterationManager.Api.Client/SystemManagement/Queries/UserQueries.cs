using SqlKata.Execution;
using SuitAlterationManager.Api.Client.SystemManagement.Responses;
using SuitAlterationManager.Infrastructure.ReadCycle;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.SystemManagement.Queries
{
    public interface IUserQueries : IQueryService
    {
        Task<UserResponse> FindUserByEmailAsync(string email);
    }

    public class UserQueries : IUserQueries
    {
        private readonly QueryFactory db;

        public UserQueries(QueryFactory db)
        {
            this.db = db;
        }
        /// <summary>
        /// Finds the user with the email specified if exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UserResponse> FindUserByEmailAsync(string email)
        {
            var query = db.Query("System.User")
              .Where("Email", email)
              .Select(
                "User.Id",
                "User.Email",
                "User.Password"
              );

            var result = await query.FirstOrDefaultAsync<UserResponse>();
            return result;
        }
    }
}
