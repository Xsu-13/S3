using EFTraining.Entities;

namespace EFTraining.Services
{
    public interface IFileService
    {
        Task<S3Response> UploadFile(S3Object s3obj);
        Task<S3Response> DownloadFile(string keyName, string filepath);
        Task<S3Response> DeleteFile(string keyName);
    }
}
