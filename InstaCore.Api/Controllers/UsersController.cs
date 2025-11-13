using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using InstaCore.Core.Dtos.Users;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Models;
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

            try
            {
                UserResponse response = await userService.GetMeAsync(userId);

                return Ok(new { response.Id, response.Username, response.Bio, response.AvatarUrl, response.Followers, response.Following });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { title = "Not found", detail = ex.Message });
            }
        }


        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicProfile(string username)
        {
            try
            {
                UserResponse response = await userService.GetByUsernameAsync(username);
                return Ok(new { response.Username, response.Bio, response.AvatarUrl, response.Followers, response.Following });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { title = "Not found", detail = ex.Message });
            }
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            try
            {
                UserResponse response = await userService.UpdateProfileAsync(userId, request);

                return Ok(new { response.Id, response.Username, response.Bio, response.AvatarUrl });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { title = "Not found", detail = ex.Message });
            }
        }
    }
}
