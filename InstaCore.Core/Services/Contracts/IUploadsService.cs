using Microsoft.AspNetCore.Http;

namespace InstaCore.Core.Services.Contracts
{
    public interface IUploadsService
    {
        Task<string> UploadImageAsync(IFormFile file, string rootPath, string saveFolderPath);
    }
}
