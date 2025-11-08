using InstaCore.Core.Dtos;

namespace InstaCore.Core.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        Task<AuthResponse> LoginAsync(LoginRequest request);

    }
}
