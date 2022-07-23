using SuitAlterationManager.Domain.Base.Models;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;

namespace SuitAlterationManager.Domain.SystemManagement
{
	public class Context : AggregateRoot<ContextID>
	{
		public string Name { get; set; }
		public DateTimeOffset UpdateDate { get; protected set; }
	}
}
