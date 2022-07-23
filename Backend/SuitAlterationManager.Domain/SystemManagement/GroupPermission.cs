using SuitAlterationManager.Domain.SystemManagement.ValueObjects;

namespace SuitAlterationManager.Domain.SystemManagement
{
	public class GroupPermission
	{
		public GroupID IdGroup { get; set; }
		public ActionID IdAction { get; set; }
	}
}
