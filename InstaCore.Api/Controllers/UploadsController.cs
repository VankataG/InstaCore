using System.Threading.Tasks;
using InstaCore.Core.Dtos;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadFileRequest request)
        {
            string url = await uploadsService.UploadImageAsync(request.File, env.WebRootPath, "avatars");

            return Ok(new { url });
        }

        [HttpPost("post-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPostImage([FromForm] UploadFileRequest request)
        {
            string url = await uploadsService.UploadImageAsync(request.File, env.WebRootPath, "posts");

            return Ok(new { url });
        }
    }
}
