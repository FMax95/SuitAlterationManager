using SuitAlterationManager.Domain.Base.Attributes;
using System;

namespace SuitAlterationManager.Domain.Base.Validation
{
	[ExcludeFromLogging]
	public class DomainException : Exception
	{
		public string Code { get; }
		public object[] Parameters { get; }

		public DomainException() { }

		public DomainException(string code) : this(code, code, default) { }

		public DomainException(string code, params object[] args) : this(code, code, args) { }

		public DomainException(string code, string message, params object[] args) : base(message)
		{
			Code = code;
			Parameters = args;
		}
	}
}
