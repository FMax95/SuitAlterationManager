using SuitAlterationManager.Infrastructure.Logger.Managers;
using System;

namespace SuitAlterationManager.Infrastructure.Logger
{
    public class LoggerService : ILoggerService
	{
		public ILoggerManager Logger { get; set; }

		public LoggerService()
		{
			Logger = new SerilogManager();
		}

		public void Info<T>(string message) => Logger.Info<T>(message);
		public void Warn<T>(string message) => Logger.Warn<T>(message);
		public void Warn<T>(string message, Exception ex) => Logger.Warn<T>(message, ex);
		public void Error<T>(string message, Exception ex) => Logger.Error<T>(message, ex);
	}
}
