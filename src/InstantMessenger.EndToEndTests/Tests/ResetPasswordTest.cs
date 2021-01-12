using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.EndToEndTests.Api;
using InstantMessenger.EndToEndTests.Common;
using InstantMessenger.Identity.Application.Features.PasswordReset;
using InstantMessenger.Identity.Application.Features.SendPasswordReset;
using InstantMessenger.Identity.Application.Features.SignIn;
using InstantMessenger.Shared.MailKit;
using Xunit;

namespace InstantMessenger.EndToEndTests.Tests
{
    [Collection("Server collection")]
    public class ResetPasswordTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;
        private readonly FakeMailService _mailService;

        public ResetPasswordTest(ServerFixture fixture)
        {
            _fixture = fixture;
            _mailService = (FakeMailService)_fixture.GetService<IMailService>();
        }

        [Fact]
        public async Task HappyPath()
        {
            var email = "test@test.com";
            var user = await _fixture.LoginAsUser(email,"test");
            _mailService.Reset();
            var sut = _fixture.GetClient<IIdentityApi>();


            var result = await sut.ForgotPassword(new SendPasswordResetCommand(email));

            result.IsSuccessStatusCode.Should().BeTrue();
            _mailService.Messages.Should()
                .HaveSingleMailWithProperReceiverAndSender(@from: "appmailbox@test.com", to: email)
                .And.ContainProperLink();
            var link = EmailContentExtractor.GetTokenFromForgotPasswordEmail(_mailService.Messages.First());
            var (_, token) = EmailContentExtractor.GetQueryParams(link);

            var newPassword = "TEestt12!@";

            result = await sut.ResetPassword(new ResetPasswordCommand(user.UserId, token, newPassword));

            result.IsSuccessStatusCode.Should().BeTrue();
            await sut.SignIn(new SignInCommand(email, newPassword));
        }
    }
}