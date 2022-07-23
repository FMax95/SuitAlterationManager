using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Extensions;
using SuitAlterationManager.Extensions.Attributes;
using SuitAlterationManager.Infrastructure.FileStorage;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.CMS.SystemManagement
{
    [Route("api/files")]
    [TypeFilter(typeof(AllowExceptionAttribute))]
    public class FileController : BaseController
    {
        private readonly IFileStorageService fileStorageService;

        public FileController(IFileStorageService fileStorageService)
        {
            this.fileStorageService = fileStorageService;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file is null)
                throw new DomainException(ErrorCodes.InvalidFile, "File is empty!");

            var fileName = await fileStorageService.UploadAsync(file);

            return Ok(fileName);
        }


        [HttpPost("save/{fileName}")]
        public IActionResult Save([FromRoute] string fileName)
        {
            fileStorageService.Save(fileName, "");
            return Ok();
        }
    }
}