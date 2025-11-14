using InstaCore.Core.Dtos.Users;

namespace InstaCore.Core.Services.Contracts
{
    public interface IUserService
    {
        Task<UserResponse> GetByUsernameAsync(string username);

        Task<UserResponse> GetMeAsync(Guid userId);

        Task<UserResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);

        Task FollowAsync(Guid followerId, string followeeUsername);

        Task UnfollowAsync(Guid followerId, string followeeUsername);
    }
}
