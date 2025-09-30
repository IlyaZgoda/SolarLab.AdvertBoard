using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using System.Security.Claims;
using System.Text;

namespace SolarLab.AdvertBoard.Infrastructure.Authentication
{
    public class TokenProvider(IOptions<JwtOptions> options) : ITokenProvider
    {
        private readonly JwtOptions _jwtOptions = options.Value;

        public string Create(string id, string email)
        {
            string secretKey = _jwtOptions.Secret;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Email, email)
                ]),
                
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
