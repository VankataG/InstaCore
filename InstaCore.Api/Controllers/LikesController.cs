using System.IdentityModel.Tokens.Jwt;
using InstaCore.Core.Dtos.Likes;
using InstaCore.Core.Services;
using InstaCore.Core.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            LikeResponse response = await likeService.LikeAsync(userId, postId);

            return Ok(response);
        }

        [HttpDelete("{postId}/like")]
        public async Task<IActionResult> Unlike(Guid postId)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            LikeResponse response = await likeService.UnlikeAsync(userId, postId);

            return Ok(response);
        }
    }
}
