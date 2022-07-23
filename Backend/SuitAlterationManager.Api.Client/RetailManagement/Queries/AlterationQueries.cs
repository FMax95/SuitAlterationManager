using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlKata.Execution;
using SuitAlterationManager.Api.Client.SystemManagement.Responses;
using SuitAlterationManager.Infrastructure.ReadCycle;

namespace SuitAlterationManager.Api.Client.SystemManagement.Queries
{
    public interface IAlterationQueries : IQueryService
    {
        Task<List<AlterationResponse>> GetAlterationsAsync();
    }

    public class AlterationQueries : IAlterationQueries
    {
        private readonly QueryFactory db;

        public AlterationQueries(QueryFactory db)
        {
            this.db = db;
        }
        
        public async Task<List<AlterationResponse>> GetAlterationsAsync()
        {
            var query = db.Query("Retail.Alteration")
              .Select(
                "Alteration.Id" ,
                "Alteration.CustomerEmail" ,
                "Alteration.Type" ,
                "Alteration.Direction" ,
                "Alteration.Status" ,
                "Alteration.MeasureCM" ,
                "Alteration.CreateDate" ,
                "Alteration.UpdateDate"
              );

            var result = await query.GetAsync<AlterationResponse>();

            return result.ToList();
        }
    }
}