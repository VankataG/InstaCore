using InstaCore.Core.Dtos;
using InstaCore.Core.Models;

namespace InstaCore.Core.Mapping
{
    public static class AuthMapper
    {
        public static AuthResponse ToResponse(string token, User user) => new()
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username
        };
    }
}
