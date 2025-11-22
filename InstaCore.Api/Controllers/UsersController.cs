using System.IdentityModel.Tokens.Jwt;
using InstaCore.Api.Extensions;
using InstaCore.Core.Dtos.Users;
using InstaCore.Core.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }


        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            UserResponse response = await userService.GetMeAsync(userId.Value);

            return Ok(new { response.Id, response.Username, response.Bio, response.AvatarUrl, response.Followers, response.Following });
        }


        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicProfile(string username)
        {
            UserResponse response = await userService.GetByUsernameAsync(username);
            return Ok(new { response.Username, response.Bio, response.AvatarUrl, response.Followers, response.Following });
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            UserResponse response = await userService.UpdateProfileAsync(userId.Value, request);

            return Ok(new { response.Id, response.Username, response.Bio, response.AvatarUrl });
        }


        [HttpPost("{username}/follow")]
        public async Task<IActionResult> Follow(string username)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            bool followed = await userService.FollowAsync(userId.Value, username);

            if (followed) return Created();

            return NoContent();
        }

        [HttpDelete("{username}/follow")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var userId = this.GetUserId();
            if (userId == null)
                return Unauthorized();

            var unfollowed = await userService.UnfollowAsync(userId.Value, username);
            if (unfollowed) return Created();

            return NoContent();
        }
    }
}
