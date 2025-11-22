using System.IdentityModel.Tokens.Jwt;

using InstaCore.Core.Dtos.Likes;
using InstaCore.Core.Dtos.Posts;
using InstaCore.Core.Services.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;

        private readonly ILikeService likeService;

        public PostsController(IPostService postService, ILikeService likeService)
        {
            this.postService = postService;
            this.likeService = likeService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            PostResponse response = await postService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            PostResponse response = await postService.GetByIdAsync(id);
            return Ok(response);
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
