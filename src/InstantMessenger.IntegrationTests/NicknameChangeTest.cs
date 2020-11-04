using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.IntegrationTests.Identity;
using InstantMessenger.Profiles.Api.Features.NicknameChange;
using Xunit;

namespace InstantMessenger.IntegrationTests
{
    [Collection("Server collection")]
    public class NicknameChangeTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;

        public NicknameChangeTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task HappyPath()
        {
            var token = await _fixture.LoginWithDefaultUser();
            var sut = _fixture.GetClient<IProfilesApi>();

            var result = await sut.ChangeNickname($"Bearer {token.Token}", new ChangeNicknameApiRequest("new_nickname"));

            result.IsSuccessStatusCode.Should().BeTrue();
            var profile = await sut.Get($"Bearer {token.Token}");
            profile.Nickname.Should().Be("new_nickname");
        }
    }
}