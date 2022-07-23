using Microsoft.AspNetCore.Mvc.Filters;
using SuitAlterationManager.Api.CMS.SystemManagement.Service.Interfaces;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.Constants;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Extensions;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.Filters
{
    public class ValidatePermissionsFilter : IAsyncActionFilter
    {
        private readonly IPermissionService _permissionService;
        private readonly string _action;
        private readonly string _context;

        public ValidatePermissionsFilter(IPermissionService permissionService, ActionValue? action = null, ContextValue? context = null)
        {
            _permissionService = permissionService;
            _action = action?.ToString();
            _context = context?.ToString();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            var controller = actionContext.Controller;
            UserID loggedUserId = controller is BaseController ? ((BaseController)controller).LoggedUserId : null;

            // If the action has not been passed as parameter, take the method name
            string actionName = _action ?? actionContext.RouteData.Values["action"]?.ToString();
            // If the context has not been passed as parameter, take the controller name
            string contextName = _context ?? actionContext.RouteData.Values["controller"]?.ToString();
            // If the permission validation fails it will throw an error and block execution
            if (!await _permissionService.ValidatePermission(loggedUserId, actionName, contextName))
                throw new ForbiddenException(ErrorCodes.PermissionDenied);

            await next();
        }
    }
}
