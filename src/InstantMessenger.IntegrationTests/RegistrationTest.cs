using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using InstantMessenger.Identity.Api.Features.SignIn;
using InstantMessenger.Identity.Api.Features.SignUp;
using InstantMessenger.IntegrationTests.Identity;
using InstantMessenger.Shared.MailKit;
using Xunit;

namespace InstantMessenger.IntegrationTests
{
    public class RegistrationTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;
        public RegistrationTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task HappyPath()
        {
            var sut = _fixture.GetClient<IIdentityApi>();
            var command = new SignUpCommand("test@test.com", "TEest12!@");

            var result = await sut.SignUp(command);

            result.IsSuccessStatusCode.Should().BeTrue();
            var mailService = (FakeMailService)_fixture.GetService<IMailService>();
            mailService.Messages.Should()
                .HaveSingleMailWithProperReceiverAndSender(from: "appmailbox@test.com", to:command.Email).And
                .ContainProperLink();


            var link = LinkExtractor.FromMail(mailService.Messages.First());
            var (userId, token) = GetQueryParams(link);

            var result2 = await sut.ActivateAccount(userId, token);

            result2.IsSuccessStatusCode.Should().BeTrue();


            var signInResult = await sut.SignIn(new SignInCommand(command.Email, command.Password));

            signInResult.Token.Should().NotBeEmpty();
            

            var meResult = await sut.Me($"Bearer {signInResult.Token}");

            meResult.Id.Should().NotBeEmpty();
            meResult.Email.Should().Be(command.Email);
        }


        private (string userId, string token) GetQueryParams(string link)
        {
            var queryParams = HttpUtility.ParseQueryString(new UriBuilder(link).Query);
            return (queryParams.Get("userId"), queryParams.Get("token"));
        }
    }
}
