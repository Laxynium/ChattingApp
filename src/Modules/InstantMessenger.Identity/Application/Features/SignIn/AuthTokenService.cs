using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace InstantMessenger.Identity.Application.Features.SignIn
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
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString("N")), 
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

        public string Create(Guid userId, string secret)
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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(securityToken);
        }

        public bool Verify(string token, string secret, out Guid userId)
        {
            userId = default;
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var result = handler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidIssuer = _options.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret))
                    },
                    out var validatedToken
                );
                userId = Guid.Parse(result.Identity.Name ?? string.Empty);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }
    }
}