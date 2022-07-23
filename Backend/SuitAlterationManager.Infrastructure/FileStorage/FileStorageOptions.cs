using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.FileStorage
{
	public class FileStorageOptions
	{
		public string[] PermittedExtensions { get; set; }
		public string PublicUri { get; set; }
		public string StorageRootPath { get; set; }
		public string Driver { get; set; }
		public FileStorageDriverConfigurationsOptions DriverConfigurations { get; set; }
		public string GetStorageRootPath()
		{
#if DEBUG
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				var tempPath = Environment.CurrentDirectory.Split("/").Take(3).ToList();
				tempPath.Add(StorageRootPath);
				return String.Join("/", tempPath);
			}
#endif
			return StorageRootPath;
		}
	}

	public class FileStorageDriverConfigurationsOptions
	{
		public string AccountName { get; set; }
		public string ContainerName { get; set; }
		public string ConnectionString { get; set; }
		public string RootFolder { get; set; }
	}
}