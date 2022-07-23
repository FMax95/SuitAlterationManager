using System;

namespace SuitAlterationManager.Domain.Base.Validation
{
	public class NotFoundException : Exception
	{
		public string Code { get; }
		public object[] Parameters { get; }

		public NotFoundException() { }

		//public NotFoundException(string message) : base(message) { }

		public NotFoundException(params object[] args)
			: base("Requested element not found.")
		{
			Code = ErrorCodes.ElementNotFound;
			Parameters = args;
		}


		public NotFoundException(string code, params object[] args)
			: base("Requested element not found.")
		{
			Code = code;
			Parameters = args;
		}
	}
}
