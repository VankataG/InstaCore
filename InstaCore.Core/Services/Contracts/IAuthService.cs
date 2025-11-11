using InstaCore.Core.Dtos;

namespace InstaCore.Core.Services.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        Task<AuthResponse> LoginAsync(LoginRequest request);

    }
}
