using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InstaCore.Core;
using InstaCore.Core.Contracts;
using InstaCore.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InstaCore.Infrastructure.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions options;

        private readonly SigningCredentials credentials;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            this.options = options.Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.Key));
            this.credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public string CreateAccessToken(User user, IEnumerable<Claim>? extraClaims = null)
        {
            var now = DateTime.UtcNow; ;

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.Username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)
            };

            if (extraClaims != null)
                claims.AddRange(extraClaims);

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(options.AccessTokenMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
