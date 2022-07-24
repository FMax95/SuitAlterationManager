using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace SuitAlterationManager.Infrastructure.Logger.Managers
{
	public class SerilogManager : ILoggerManager
	{
		protected readonly ILogger _logger;

		public SerilogManager()
		{
			LoggerConfiguration loggerConfiguration = new LoggerConfiguration().WriteTo.
				ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = "33b99516-996a-4a14-aa8d-53013a40bf3e" }, TelemetryConverter.Traces);
			
			_logger = loggerConfiguration.CreateLogger();
		}

		public void Info<T>(string message) => _logger.ForContext<T>().Information(message);
		public void Warn<T>(string message) => _logger.ForContext<T>().Warning(message);
		public void Warn<T>(string message, Exception ex) => _logger.ForContext<T>().Warning(ex, message);
		public void Error<T>(string message, Exception ex) => _logger.ForContext<T>().Error(ex, message);
	}
}
