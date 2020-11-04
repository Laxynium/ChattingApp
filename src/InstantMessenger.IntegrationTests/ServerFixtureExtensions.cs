using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Identity.Api.Features.SignIn;
using InstantMessenger.Identity.Api.Features.SignUp;
using InstantMessenger.IntegrationTests.Identity;
using InstantMessenger.Shared.MailKit;

namespace InstantMessenger.IntegrationTests
{
    internal static class ServerFixtureExtensions
    {
        internal static async Task<AuthDto> LoginWithDefaultUser(this ServerFixture fixture)
        {
            var (email, password) = ("test@test.com", "TEest12!@");
            var identityApi = fixture.GetClient<IIdentityApi>();
            await identityApi.SignUp(new SignUpCommand(email, password));

            var mailService = (FakeMailService)fixture.GetService<IMailService>();
            var link = LinkExtractor.FromMail(mailService.Messages.First());
            var (userId, token) = LinkExtractor.GetQueryParams(link);
            await identityApi.ActivateAccount(userId, token);

            var authDto = await identityApi.SignIn(new SignInCommand(email, password));
            return authDto;
        }
    }
}