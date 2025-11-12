using InstaCore.Core.Dtos.Users;

namespace InstaCore.Core.Services.Contracts
{
    public interface IUserService
    {
        Task<UserResponse> GetByUsernameAsync(string username);

        Task<UserResponse> GetMeAsync(string userId);

        Task<UserResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    }
}
