using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace InstaCore.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static Guid? GetUserId(this ControllerBase controller)
        {
            var sub = controller.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(sub, out var userId))
                return userId;

            return null;
        }
    }
}
