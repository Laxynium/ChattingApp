using System;

namespace InstantMessenger.Identity.Api.Features.SignIn
{
    public interface IAuthTokenService
    {
        AuthDto Create(Guid userId);
    }
}