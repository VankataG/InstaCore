using InstaCore.Core.Exceptions;
using InstaCore.Core.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace InstaCore.Core.Services
{
    public class UploadsService : IUploadsService
    {
        public async Task<string> UploadImageAsync(IFormFile file, string rootPath, string saveFolderPath)
        {
            if (file is null)
                throw new BadRequestException("File is required");

            if (file.Length == 0)
                throw new BadRequestException("Empty file");

            long maxBytes = 5 * 1024 * 1024;

            if (file.Length > maxBytes)
                throw new BadRequestException("File is too large to upload");


            List<string> allowedExts = new() { ".jpg", ".jpeg", ".png", ".webp" };

            string? ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExts.Contains(ext))
                throw new BadRequestException("Extension not allowed");

            string filename = Guid.NewGuid().ToString("N") + ext;

            string dirPath = Path.Combine(rootPath, "uploads", saveFolderPath);
            Directory.CreateDirectory(dirPath);

            string filePath = Path.Combine(dirPath, filename);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            string url = $"/uploads/{saveFolderPath}/{filename}";

            return url;
        }
    }
}
