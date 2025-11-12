using InstaCore.Core.Contracts;
using InstaCore.Core.Dtos;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Models;
using InstaCore.Core.Services.Contracts;

namespace InstaCore.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;

        private readonly IPasswordHasher passwordHasher;

        private readonly IJwtTokenService jwtTokenService;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.jwtTokenService = jwtTokenService;
        }


        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);

            if (user == null || !passwordHasher.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedException();


            var token = jwtTokenService.CreateAccessToken(user);

            return new AuthResponse()
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
            };


        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await userRepository.ExistsByEmailAsync(request.Email) ||
                await userRepository.ExistsByUsernameAsync(request.Username))
            {
                throw new ConflictException("Email or Username already taken.");
            }

            string hashedPassword = passwordHasher.Hash(request.Password);

            User newUser = new User()
            {
                Username = request.Username.Trim(),
                Email = request.Email.ToLower(),
                PasswordHash = hashedPassword
            };

            await userRepository.AddAsync(newUser);

            var token = jwtTokenService.CreateAccessToken(newUser);

            return new AuthResponse()
            { 
                Token = token,
                UserId = newUser.Id,
                Username = newUser.Username,
            };
        }
    }
}
