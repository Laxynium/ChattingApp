namespace InstantMessenger.Identity.Application.Features.SignIn
{
    public interface IAuthTokensCache
    {
        void Set(string email, AuthDto dto);
        AuthDto Get(string email);
    }
}