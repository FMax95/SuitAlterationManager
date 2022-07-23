using SuitAlterationManager.Domain.SystemManagement.ValueObjects;

namespace SuitAlterationManager.Domain.SystemManagement
{
	public class UserGroup
	{
		public UserID IdUser { get; set; }
		public GroupID IdGroup { get; set; }

		public UserGroup() { }

		internal static UserGroup Create(UserID idUser, GroupID idGroup) =>
			new UserGroup
			{
				IdUser = idUser,
				IdGroup = idGroup
			};
	}
}
