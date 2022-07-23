using System;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Responses
{
    public class ContextActionResponse
    {
        public Guid? IdUser { get; set; }
        public string Context { get; set; }
        public string Action { get; set; }
    }
}
