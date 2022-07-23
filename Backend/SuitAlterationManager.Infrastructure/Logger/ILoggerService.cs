using System;

namespace SuitAlterationManager.Infrastructure.Logger
{
	public interface ILoggerService
	{
		void Info<T>(string message);
		void Warn<T>(string message);
		void Warn<T>(string message, Exception ex);
		void Error<T>(string message, Exception ex);
	}
}
