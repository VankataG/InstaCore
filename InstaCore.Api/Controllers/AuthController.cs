using System.Reflection;
using InstaCore.Core.Contracts;
using InstaCore.Core.Dtos;
using InstaCore.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                AuthResponse response = await authService.RegisterAsync(request);
                return Created(string.Empty, response);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { title = "Conflict", detail = ex.Message});
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                AuthResponse response = await authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { title = "Unazuthorized", detail = ex.Message });
            }
        }
    }
}
