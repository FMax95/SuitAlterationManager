using System;
using Serilog;
using Serilog.Events;

namespace SuitAlterationManager.Infrastructure.Logger.Managers
{
    public class SerilogManager : ILoggerManager
    {
        public ILogger Logger;

        public SerilogManager(LoggerServiceOptions loggerOptions)
        {
            LoggerConfiguration loggerConfiguration;
            switch (loggerOptions.LogManager.ToLower())
            {
                case "sentry":
                    loggerConfiguration = new LoggerConfiguration().WriteTo.Sentry();
                    break;
                case "file":
                    loggerConfiguration = new LoggerConfiguration().WriteTo
                                                                   .File(loggerOptions.Path + loggerOptions.Filename
                                                                       , outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}{NewLine}"
                                                                       , rollingInterval: RollingInterval.Day)
                                                                   .MinimumLevel.Is(GetLoggingLevel(loggerOptions.MinimumLevel))
                                                                   .Enrich.WithProperty("Application", loggerOptions.ApiName);
                    break;
                case "all":
                    loggerConfiguration = new LoggerConfiguration().WriteTo
                                                                   .Sentry()
                                                                   .WriteTo
                                                                   .File(loggerOptions.Path + loggerOptions.Filename
                                                                       , outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}{NewLine}"
                                                                       , rollingInterval: RollingInterval.Day)
                                                                   .MinimumLevel.Is(GetLoggingLevel(loggerOptions.MinimumLevel))
                                                                   .Enrich.WithProperty("Application", loggerOptions.ApiName);
                    break;
                default:
                    loggerConfiguration = new LoggerConfiguration();
                    break;
            }
            Logger = loggerConfiguration.CreateLogger();
        }

        private LogEventLevel GetLoggingLevel(string logLevel)
        {
            switch (logLevel)
            {
                case "Debug":
                    return LogEventLevel.Debug;
                case "Information":
                    return LogEventLevel.Information;
                case "Warning":
                    return LogEventLevel.Warning;
                case "Error":
                    return LogEventLevel.Error;
                default:
                    return LogEventLevel.Information;
            }
        }

        public void Info<T>(string message) => Logger.ForContext<T>().Information(message);
        public void Warn<T>(string message) => Logger.ForContext<T>().Warning(message);
        public void Warn<T>(string message, Exception ex) => Logger.ForContext<T>().Warning(ex, message);
        public void Error<T>(string message, Exception ex) => Logger.ForContext<T>().Error(ex, message);
    }
}
