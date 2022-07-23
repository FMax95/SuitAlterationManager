using System;

namespace SuitAlterationManager.Domain.Base.Validation
{
	public class ApplicationServiceException : Exception
	{
		public string Code { get; }
		public object[] Parameters { get; }

		public ApplicationServiceException() { }

		public ApplicationServiceException(string code) : this(code, code, default) { }

		public ApplicationServiceException(string code, params object[] args) : this(code, code, args) { }

		public ApplicationServiceException(string code, string message, params object[] args) : base(message)
		{
			Code = code;
			Parameters = args;
		}
	}
}
