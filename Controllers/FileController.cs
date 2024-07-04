using EFTraining.Entities;
using EFTraining.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<S3Response> UploadFile(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileExt = Path.GetExtension(file.FileName);
            var objName = $"{Guid.NewGuid()}.{fileExt}";

            var s3Obj = new S3Object()
            {
                Name = objName,
                File = memoryStream
            };

            return await _fileService.UploadFile(s3Obj);
        }


        [HttpGet]
        public async Task<S3Response> DownloadFile(string name, string filepath = "imgs")
        {
            return await _fileService.DownloadFile(name, filepath);
        }

        [HttpDelete]
        public async Task<S3Response> DeleteFile(string name)
        {
            return await _fileService.DeleteFile(name);
        }
    }
}
