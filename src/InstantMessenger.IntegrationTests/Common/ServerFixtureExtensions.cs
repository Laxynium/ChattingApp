using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Identity.Api.Features.SignIn;
using InstantMessenger.Identity.Api.Features.SignUp;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.Shared.MailKit;

namespace InstantMessenger.IntegrationTests.Common
{
    internal static class ServerFixtureExtensions
    {
        internal static async Task<AuthDto> LoginWithDefaultUser(this ServerFixture fixture)
            => await LoginAsUser(fixture,"test@test.com");

        internal static async Task<AuthDto> LoginAsUser(this ServerFixture fixture, string email)
        {
            var password = "TEest12!@";
            var identityApi = fixture.GetClient<IIdentityApi>();
            await identityApi.SignUp(new SignUpCommand(email, password));

            var mailService = (FakeMailService)fixture.GetService<IMailService>();
            var link = LinkExtractor.FromMail(mailService.Messages.Last());
            var (userId, token) = LinkExtractor.GetQueryParams(link);
            await identityApi.ActivateAccount(userId, token);

            var authDto = await identityApi.SignIn(new SignInCommand(email, password));
            return authDto;
        }
    }
}