using System.IdentityModel.Tokens.Jwt;
using InstaCore.Api.Extensions;
using InstaCore.Core.Dtos.Likes;
using InstaCore.Core.Services.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/likes")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService likeService;

        public LikesController(ILikeService likeService)
        {
            this.likeService = likeService;
        }


        [HttpPost("{postId}/like")]
        public async Task<IActionResult> Like(Guid postId)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            LikeResponse response = await likeService.LikeAsync(userId.Value, postId);

            return Ok(response);
        }

        [HttpDelete("{postId}/like")]
        public async Task<IActionResult> Unlike(Guid postId)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            LikeResponse response = await likeService.UnlikeAsync(userId.Value, postId);

            return Ok(response);
        }
    }
}
