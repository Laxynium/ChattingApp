using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Identity.Api.Features.PasswordReset;
using InstantMessenger.Identity.Api.Features.SendPasswordReset;
using InstantMessenger.Identity.Api.Features.SignIn;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using InstantMessenger.Shared.MailKit;
using Xunit;

namespace InstantMessenger.IntegrationTests.Tests
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
            var user = await _fixture.LoginAsUser(email);
            _mailService.Reset();
            var sut = _fixture.GetClient<IIdentityApi>();


            var result = await sut.ForgotPassword(new SendPasswordResetCommand(email));

            result.IsSuccessStatusCode.Should().BeTrue();
            _mailService.Messages.Should()
                .HaveSingleMailWithProperReceiverAndSender(@from: "appmailbox@test.com", to: email)
                .And.ContainProperToken();
            var token = EmailContentExtractor.GetTokenFromForgotPasswordEmail(_mailService.Messages.First());

            var newPassword = "TEestt12!@";

            result = await sut.ResetPassword(new ResetPasswordCommand(Guid.Parse(user.Subject), token, newPassword));

            result.IsSuccessStatusCode.Should().BeTrue();
            await sut.SignIn(new SignInCommand(email, newPassword));
        }
    }
}