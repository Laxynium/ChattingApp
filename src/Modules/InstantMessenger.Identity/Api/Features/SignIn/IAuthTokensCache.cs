namespace InstantMessenger.Identity.Api.Features.SignIn
{
    public interface IAuthTokensCache
    {
        void Set(string email, AuthDto dto);
        AuthDto Get(string email);
    }
}