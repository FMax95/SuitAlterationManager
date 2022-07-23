using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuitAlterationManager.Infrastructure.Logger;

namespace SuitAlterationManager.Extensions.DI
{
    public static class Log
    {
        public static void AddLoggerService(this IServiceCollection services, IConfiguration configuration)
        {
            LoggerServiceOptions loggerOptions = GetLoggerOptions(configuration); Directory.CreateDirectory(System.IO.Path.GetFullPath(loggerOptions.Path));

            services.AddSingleton<ILoggerService, LoggerService>(provider => new LoggerService(
                loggerOptions: loggerOptions
            ));
        }

        public static LoggerServiceOptions GetLoggerOptions(IConfiguration configuration)
        {
            LoggerServiceOptions loggerOptions = new LoggerServiceOptions()
            {
                ApiName = configuration.GetValue<string>("ApiName"),
                MinimumLevel = configuration.GetSection("FileLog").GetValue<string>("MinimumLevel"),
                LogManager = configuration.GetSection("Logger").GetValue<string>("LogManager"),
                Path = configuration.GetSection("FileLog").GetValue<string>("Path"),
                Filename = configuration.GetSection("FileLog").GetValue<string>("Filename")
            };
#if DEBUG
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                loggerOptions.Path = "C:" + loggerOptions.Path;
            else
            {
                var tempPath = Environment.CurrentDirectory.Split("/").Take(3).ToList();
                tempPath.Add(loggerOptions.Path);
                loggerOptions.Path = String.Join("/", tempPath);
            }
#endif
            return loggerOptions;
        }
    }
}