using System.Security.Claims;
using InstaCore.Core.Models;

namespace InstaCore.Core.Contracts
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(User user, IEnumerable<Claim>? extraClaims = null);
    }
}
