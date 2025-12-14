using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/uploads")]
    public class UploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public UploadsController(IWebHostEnvironment env)
        {
            this.env = env;
        }



        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file is null)
                return BadRequest("File is required");

            if (file.Length == 0)
                return BadRequest("Empty file");

            long maxBytes = 5 * 1024 * 1024;

            if (file.Length > maxBytes)
                return BadRequest("File is too large to upload");


            List<string> allowedExts = new() { ".jpg", ".jpeg", ".png", ".webp" };

            string? ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExts.Contains(ext))
                return BadRequest("Extension not allowed");



            string filename = Guid.NewGuid().ToString("N") + ext;

            string dirPath = Path.Combine(env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(dirPath);

            string filePath = Path.Combine(dirPath, filename);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            string url = $"/uploads/avatars/{filename}";

            return Ok(new { url });
        }

        [HttpPost("post-image")]
        public async Task<IActionResult> UploadPostImage([FromForm]IFormFile file)
        {
            if (file is null)
                return BadRequest("File is required");

            if (file.Length == 0)
                return BadRequest("Empty file");

            long maxBytes = 5 * 1024 * 1024;

            if (file.Length > maxBytes)
                return BadRequest("File is too large to upload");


            List<string> allowedExts = new() { ".jpg", ".jpeg", ".png", ".webp" };

            string? ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExts.Contains(ext))
                return BadRequest("Extension not allowed");



            string filename = Guid.NewGuid().ToString("N") + ext;

            string dirPath = Path.Combine(env.WebRootPath, "uploads", "posts");
            Directory.CreateDirectory(dirPath);

            string filePath = Path.Combine(dirPath, filename);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            string url = $"/uploads/posts/{filename}";

            return Ok(new { url });
        }
    }
}
