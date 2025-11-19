using System.Reflection;
using InstaCore.Core.Dtos;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Services.Contracts;
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
            AuthResponse response = await authService.RegisterAsync(request);
            return Created(string.Empty, response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            AuthResponse response = await authService.LoginAsync(request);
            return Ok(response);
        }
    }
}
