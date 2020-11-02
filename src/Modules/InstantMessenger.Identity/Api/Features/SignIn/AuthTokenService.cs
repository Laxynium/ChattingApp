using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace InstantMessenger.Identity.Api.Features.SignIn
{
    internal sealed class AuthTokenService : IAuthTokenService
    {
        private readonly IdentityOptions _options;

        public AuthTokenService(IdentityOptions options)
        {
            _options = options;
        }
        public AuthDto Create(Guid userId)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString("N")),
                    new Claim(ClaimTypes.Role, "user"),
                }),
                Expires = DateTime.UtcNow.AddDays(_options.ExpirationDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_options.Key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = handler.CreateToken(tokenDescriptor);

            var token = new AuthDto
            {
                Token = handler.WriteToken(securityToken),
                Issuer = securityToken.Issuer,
                Subject = userId.ToString("N"),
                ValidFrom = securityToken.ValidFrom,
                ValidTo = securityToken.ValidTo
            };

            return token;
        }
    }
}