using SuitAlterationManager.Domain.Base.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
	{
		private readonly FileStorageOptions storageOptions;
		public LocalFileStorageService(IOptions<FileStorageOptions> storageOptions)
		{
			this.storageOptions = storageOptions.Value;
		}
		/// <summary>
		/// Like Path.Combine, but adds the StorageRootPath at the beginning.
		/// It also checks that the resulting path is a valid sub-path of StorageRootPath.
		/// </summary>
		/// <param name="pieces">path pieces</param>
		/// <returns>the combined path</returns>
		protected string CombinePath(params string[] pieces)
		{
			var combinedPath = Path.Combine(storageOptions.GetStorageRootPath(), Path.Combine(pieces));
			var fullPath = Path.GetFullPath(combinedPath);
			var fullRootPath = Path.GetFullPath(storageOptions.GetStorageRootPath());
			if (!fullPath.StartsWith(fullRootPath))
			{
				throw new DomainException(ErrorCodes.InvalidFile, $"File {fullPath} not found!");
			}
			return combinedPath;
		}


		/// <summary>
		/// Delete file from stage folder
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="folderName"></param>
		public Task Delete(string fileName, string folderName)
		{
			return Task.Run(() =>
			{
				var targetFile = CombinePath(folderName, fileName);
				File.Delete(targetFile);
			});
		}

		/// <summary>
		/// Save file (move from temp to stage folder)
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="folderName"></param>
		public Task Save(string fileName, string folderName)
		{
			return Task.Run(() =>
			{
				// check file in temp folder
				var tempFile = CombinePath("Temp", fileName);
				var targetPath = CombinePath(folderName);
				var tagetFile = CombinePath(folderName, fileName);

				if (!File.Exists(tempFile))
					throw new DomainException(ErrorCodes.InvalidFile, $"File {fileName} not found!");

				if (!Directory.Exists(targetPath))
					Directory.CreateDirectory(targetPath);

				File.Move(tempFile, tagetFile);
				//delete temp file
				File.Delete(tempFile);
			});
		}

		/// <summary>
		/// Save a list of files (move from temp to stage folder)
		/// </summary>
		/// <param name="fileNames"></param>
		/// <param name="folderName"></param>
		public Task SaveBatch(List<string> fileNames, string folderName)
		{
			return Task.Run(() =>
			{
				foreach (var fileName in fileNames)
				{
					Save(fileName, folderName);
				}
			});
		}

		/// <summary>
		/// Save file (move from temp to stage folder) if fileName is different than oldFileName;
		/// if there is a difference the new file name (set under folderName) is returned;
		/// otherwise the unchanged fileName is returned.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="oldFileName"></param>
		/// <param name="folderName"></param>
		public string SaveIfChanged(string fileName, string oldFileName, string folderName)
		{
			if (!string.IsNullOrWhiteSpace(fileName) && oldFileName != fileName)
			{
				Save(fileName, folderName);
				return $"{folderName}/{fileName}";
			}
			return fileName;
		}

		/// <summary>
		/// Return the new fileName if fileName is different than oldFileName;
		/// if there is a difference the new file name (set under folderName) is returned;
		/// otherwise the unchanged fileName is returned.
		/// This is the same as SaveIfChanged, without the saving/moving of the file.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="oldFileName"></param>
		/// <param name="folderName"></param>
		public string FileNameIfChanged(string fileName, string oldFileName, string folderName)
		{
			if (!string.IsNullOrWhiteSpace(fileName) && oldFileName != fileName)
			{
				return $"{folderName}/{fileName}";
			}
			return fileName;
		}

		/// <summary>
		/// Upload a file to a temp storage folder
		/// </summary>
		/// <param name="file"></param>
		/// <returns>file path</returns>
		public async Task<string> UploadAsync(IFormFile file)
		{
			// file validation
			var fileExtension = Path.GetExtension(file.FileName);
			if (!storageOptions.PermittedExtensions.Contains(fileExtension.ToLower()))
			{
				throw new DomainException(ErrorCodes.InvalidFile, $"File extension {fileExtension} is not allowed!");
			}

			var fileName = Path.GetRandomFileName() + fileExtension;
			var tempStoragePath = CombinePath("Temp");
			var filePath = CombinePath("Temp", fileName);

			if (!Directory.Exists(tempStoragePath))
				Directory.CreateDirectory(tempStoragePath);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			return fileName;
		}

		public string GetPublicUrl()
		{
			return storageOptions.PublicUri;
		}

		public string ContextualizeFileName(string context, string fileName)
		{
			return $"{context}/{fileName}";
		}
	}
}
