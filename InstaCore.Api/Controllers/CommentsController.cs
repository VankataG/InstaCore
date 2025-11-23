using InstaCore.Api.Extensions;
using InstaCore.Core.Dtos.Comments;
using InstaCore.Core.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/{postId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }



        [HttpPost]
        public async Task<IActionResult> AddComment(Guid postId, [FromBody] CreateCommentRequest request)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            CreateCommentResponse response = await commentService.AddCommentAsync(userId.Value, postId, request);
            return CreatedAtAction(nameof(ViewComments), new { postId }, response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewComments(Guid postId)
        {
            var commentsResponse = await commentService.ViewAllCommentsAsync(postId);

            return Ok(commentsResponse);
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid postId, Guid commentId)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            await commentService.DeleteCommentAsync(userId.Value, commentId);
            return Ok();
        }
    }
}
