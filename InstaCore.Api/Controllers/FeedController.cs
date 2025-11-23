using InstaCore.Api.Extensions;
using InstaCore.Core.Dtos.Posts;
using InstaCore.Core.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/feed")]
    public class FeedController : ControllerBase
    {
        private readonly IPostService postService;

        public FeedController(IPostService postService)
        {
            this.postService = postService;
        }


        [HttpGet]
        public async Task<IActionResult> GetFeed([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            IReadOnlyList<PostResponse> feed = await postService.GetFeedAsync(userId.Value, page, pageSize);
            return Ok(feed);
        }
    }
}
