using Microsoft.AspNetCore.Http;

namespace InstaCore.Core.Dtos
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
