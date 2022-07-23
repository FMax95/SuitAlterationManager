using System;

namespace SuitAlterationManager.Domain.Base.Validation
{
	public class ForbiddenException : Exception
	{
		public string Code { get; }
		public object[] Parameters { get; }

		public ForbiddenException() { }

		public ForbiddenException(string code) : this(code, code, default) { }

		public ForbiddenException(string code, params object[] args) : this(code, code, args) { }

		public ForbiddenException(string code, string message, params object[] args) : base(message)
		{
			Code = code;
			Parameters = args;
		}
	}
}
