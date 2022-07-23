using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.SystemManagement.Models
{
    public class UpdateUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public string BirthDate { get; set; }
        public List<Guid> IdGroupList { get; set; }
    }
}
