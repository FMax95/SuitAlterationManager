using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SuitAlterationManager.Infrastructure.FileStorage;

namespace SuitAlterationManager.Extensions.DI
{
    public static class FileStorage
    {
        static readonly List<string> ValidDrivers = new List<string>
        {
            "Local",
            "AzureBlobStorage"
        };

        public static void AddFileStorage(this IServiceCollection services, IConfigurationSection section)
        {
            string driver = GetDriver(section);

            services.Configure<FileStorageOptions>(section);
            switch (driver)
            {
                case "Local":
                    services.AddScoped<IFileStorageService, LocalFileStorageService>();
                    break;
                case "AzureBlobStorage":
                    services.AddScoped<IFileStorageService, AzureBlobStorageService>();
                    break;
            }
        }

        private static string GetDriver(IConfigurationSection section)
        {
            var driver = section.GetSection("Driver").Value;
            if (!ValidDrivers.Contains(driver))
            {
                throw new NotImplementedException();
            }

            return driver;
        }

        /// <summary>
        /// It provides an endpoint from where the files can be taken from a physical folder unless AzureBlobStorage is set
        /// </summary>
        /// <param name="app"></param>
        /// <param name="fileProviderRootPath"></param>
        /// <param name="requestPath"></param>
        /// <param name="section"></param>
        public static void UseFileStorage(this IApplicationBuilder app, string fileProviderRootPath, string requestPath, IConfigurationSection section)
        {
            string driver = GetDriver(section);

            if (driver == "AzureBlobStorage")
                return;
#if DEBUG
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                fileProviderRootPath = "C:" + fileProviderRootPath;
            else
            {
                var tempPath = Environment.CurrentDirectory.Split("/").Take(3).ToList();
                tempPath.Add(fileProviderRootPath);
                fileProviderRootPath = String.Join("/", tempPath);
            }
#endif
            Directory.CreateDirectory(System.IO.Path.GetFullPath(fileProviderRootPath));
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(fileProviderRootPath),
                RequestPath = new PathString(requestPath)
            });
        }
    }
}
