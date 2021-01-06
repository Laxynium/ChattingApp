using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Identity.Application.Features.SignIn;
using InstantMessenger.Identity.Application.Features.SignUp;
using InstantMessenger.Identity.Application.Features.VerifyUser;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using InstantMessenger.Shared.MailKit;
using Xunit;

namespace InstantMessenger.IntegrationTests.Tests
{
    [Collection("Server collection")]
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
                .HaveSingleMailWithProperReceiverAndSender(@from: "appmailbox@test.com", to:command.Email).And
                .ContainProperLink();


            var link = EmailContentExtractor.GetUrlFromActivationMail(mailService.Messages.First());
            var (userId, token) = EmailContentExtractor.GetQueryParams(link);

            var result2 = await sut.ActivateAccount(new ActivateCommand(Guid.Parse(userId), token,"test_nickname"));

            result2.IsSuccessStatusCode.Should().BeTrue();


            var user = await sut.SignIn(new SignInCommand(command.Email, command.Password));

            user.Token.Should().NotBeEmpty();
            

            var meResult = await sut.Me(user.BearerToken());

            meResult.Id.Should().NotBeEmpty();
            meResult.Email.Should().Be(command.Email);
        }
    }
}
