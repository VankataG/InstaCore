using System.IdentityModel.Tokens.Jwt;
using InstaCore.Api.Extensions;
using InstaCore.Core.Dtos.Posts;
using InstaCore.Core.Services.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            PostResponse response = await postService.CreateAsync(userId.Value, request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid postId)
        {
            PostResponse response = await postService.GetByIdAsync(postId);
            return Ok(response);
        }

    }
}
