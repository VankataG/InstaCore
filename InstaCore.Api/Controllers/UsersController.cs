using System.IdentityModel.Tokens.Jwt;
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
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();


            UserResponse response = await userService.GetMeAsync(userId);

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
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            UserResponse response = await userService.UpdateProfileAsync(userId, request);

            return Ok(new { response.Id, response.Username, response.Bio, response.AvatarUrl });
        }


        [HttpPost("{username}/follow")]
        public async Task<IActionResult> Follow(string username)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            bool followed = await userService.FollowAsync(userId, username);

            if (followed) return Created();

            return NoContent();
        }

        [HttpDelete("{username}/follow")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();


            var unfollowed = await userService.UnfollowAsync(userId, username);
            if (unfollowed) return Created();

            return NoContent();
        }
    }
}
