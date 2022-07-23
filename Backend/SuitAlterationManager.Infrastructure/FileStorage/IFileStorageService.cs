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
	public interface IFileStorageService
	{
		Task<string> UploadAsync(IFormFile file);
		Task Save(string fileName, string folderName);
		Task SaveBatch(List<string> fileNames, string folderName);
		string SaveIfChanged(string fileName, string oldFileName, string folderName);
		string FileNameIfChanged(string fileName, string oldFileName, string folderName);
		Task Delete(string fileName, string folderName);
		string GetPublicUrl();
		string ContextualizeFileName(string context, string fileName);
	}
}
