using InstantMessenger.Identity.Api.Features.SignIn;

namespace InstantMessenger.IntegrationTests.Common
{
    public static class Extensions
    {
        public static string BearerToken(this AuthDto user)
        {
            return $"Bearer {user.Token}";
        }
    }
}