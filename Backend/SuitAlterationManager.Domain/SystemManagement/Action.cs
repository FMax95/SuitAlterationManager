using SuitAlterationManager.Domain.Base.Models;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;

namespace SuitAlterationManager.Domain.SystemManagement
{
	public class Action : AggregateRoot<ActionID>
	{
		public ContextID IdContext { get; set; }
		public string Name { get; set; }
		public DateTimeOffset UpdateDate { get; protected set; }
	}
}
