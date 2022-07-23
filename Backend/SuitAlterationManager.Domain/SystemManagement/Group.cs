using SuitAlterationManager.Domain.Base.Models;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Collections.Generic;

namespace SuitAlterationManager.Domain.SystemManagement
{
	public class Group : AggregateRoot<GroupID>
	{
		public const string VISITOR_ID = "593EFDF6-1EE7-4756-96B0-0F983B1B8D8A";
		public string Name { get; protected set; }
		public string Description { get; protected set; }
		public bool IsEnabled { get; protected set; }
		public DateTimeOffset UpdateDate { get; protected set; }
		
		private readonly List<GroupPermission> permissions = new List<GroupPermission>();
		public virtual IReadOnlyList<GroupPermission> Permissions => permissions.AsReadOnly();

		public static Group Create(string name, string description, DateTimeOffset updateDate)
		{
			var group = new Group
			{
				Id = new GroupID(Guid.NewGuid()),
				Name = name,
				Description = description,
				UpdateDate = updateDate
			};

			group.Enable(updateDate);

			return group;
		}

		public void Update(string description, DateTimeOffset updateDate)
		{
			Description = description;
			UpdateDate = updateDate;
		}

		public void Enable(DateTimeOffset updateDate)
		{
			IsEnabled = true;
			UpdateDate = updateDate;
		}

		public void Disable(DateTimeOffset updateDate)
		{
			IsEnabled = false;
			UpdateDate = updateDate;
		}
	}
}
