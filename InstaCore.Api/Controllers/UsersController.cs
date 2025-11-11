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
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;   
        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            UserResponse response = await userService.GetMeAsync(userId);

            return Ok(new { userId, response.Username, response.Bio, response.AvatarUrl});
        }


        [HttpGet("{username}")]
        public async Task<IActionResult> GetPublicProfile(string username)
        {
            try
            {
                UserResponse response = await userService.GetByUsernameAsync(username);
                return Ok(new { response.Username, response.Bio, response.AvatarUrl });
            }
            catch (ConflictException ex)
            {
                return NotFound(new { title = "Not found", detail = ex.Message });
            }
            

            
        }
    }
}
