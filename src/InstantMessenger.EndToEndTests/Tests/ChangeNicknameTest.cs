using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.EndToEndTests.Api;
using InstantMessenger.EndToEndTests.Common;
using InstantMessenger.Identity.Application.Features.ChangeNickname;
using Xunit;

namespace InstantMessenger.EndToEndTests.Tests
{
    [Collection("Server collection")]
    public class ChangeNicknameTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;

        public ChangeNicknameTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Change_nickname()
        {
            var user = await _fixture.LoginWithDefaultUser();
            var sut = _fixture.GetClient<IIdentityApi>();

            var result = await sut.ChangeNickname(user.BearerToken(), new ChangeNicknameApiRequest("new_nickname"));

            result.IsSuccessStatusCode.Should().BeTrue();
            var me = await sut.Me(user.BearerToken());
            me.Nickname.Should().Be("new_nickname");
        }
    }
}