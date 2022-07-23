using System.Collections.Generic;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Responses
{
    public class UserPermissionResponse
    {
        public string Context { get; set; }
        public List<string> Actions { get; set; }
    }
}