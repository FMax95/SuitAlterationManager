using Microsoft.AspNetCore.Mvc;
using SuitAlterationManager.Api.CMS.Filters;
using SuitAlterationManager.Api.CMS.SystemManagement.Queries;
using SuitAlterationManager.Domain.Constants;
using SuitAlterationManager.Extensions;
using SuitAlterationManager.Extensions.Attributes;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.SystemManagement
{
    [Route("api/groups")]
    [TypeFilter(typeof(AllowExceptionAttribute))]
    public class GroupController : BaseController
    {
        private readonly IGroupQueries queries;

        public GroupController(IGroupQueries queries)
        {
            this.queries = queries;
        }

        [HttpGet]
        [TypeFilter(typeof(ValidatePermissionsFilter), Arguments = new object[] { ActionValue.Read, ContextValue.User  })]
        public async Task<IActionResult> GetAll()
        {
            var result = await queries.GetGroupsAsync();

            return Ok(result);
        }

    }
}