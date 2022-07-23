using System;
using SuitAlterationManager.Infrastructure.Logger.Managers;

namespace SuitAlterationManager.Infrastructure.Logger
{
    public class LoggerService : ILoggerService
    {
        public readonly LoggerServiceOptions loggerOptions;
        public ILoggerManager Logger { get; set; }

        public LoggerService(LoggerServiceOptions loggerOptions)
        {
            this.loggerOptions = loggerOptions;
            InitLogger();
        }

        private void InitLogger()
        {
            Logger = new SerilogManager(loggerOptions);
        }

        public void Info<T>(string message) => Logger.Info<T>(message);
        public void Warn<T>(string message) => Logger.Warn<T>(message);
        public void Warn<T>(string message, Exception ex) => Logger.Warn<T>(message, ex);
        public void Error<T>(string message, Exception ex) => Logger.Error<T>(message, ex);
    }
}
