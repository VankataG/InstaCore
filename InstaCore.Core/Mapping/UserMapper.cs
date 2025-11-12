using InstaCore.Core.Dtos.Users;
using InstaCore.Core.Models;

namespace InstaCore.Core.Mapping
{
    public static class UserMapper
    {
        public static UserResponse ToResponse(User user) => new()
        {
            Id = user.Id,
            Username = user.Username,
            Bio = user.Bio,
            AvatarUrl = user.AvatarUrl
        };
    }
}
