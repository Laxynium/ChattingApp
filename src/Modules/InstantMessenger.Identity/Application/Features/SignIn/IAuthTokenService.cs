using System;

namespace InstantMessenger.Identity.Application.Features.SignIn
{
    public interface IAuthTokenService
    {
        AuthDto Create(Guid userId);
        string Create(Guid userId, string secret);
        bool Verify(string token, string secret, out Guid userId);
    }
}