using System.Threading.Tasks;
using InstaCore.Core.Services.Contracts;
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
        private readonly IUploadsService uploadsService;

        public UploadsController(IWebHostEnvironment env, IUploadsService uploadsService)
        {
            this.env = env;
            this.uploadsService = uploadsService;
        }


        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
        {
            string url = await uploadsService.UploadImageAsync(file, env.WebRootPath, "avatars");

            return Ok(new { url });
        }

        [HttpPost("post-image")]
        public async Task<IActionResult> UploadPostImage([FromForm] IFormFile file)
        {
            string url = await uploadsService.UploadImageAsync(file, env.WebRootPath, "posts");

            return Ok(new { url });
        }
    }
}
