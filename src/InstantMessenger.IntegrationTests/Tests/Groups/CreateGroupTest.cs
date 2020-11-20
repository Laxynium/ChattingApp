using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using Xunit;

namespace InstantMessenger.IntegrationTests.Tests.Groups
{
    [Collection("Server collection")]
    public class CreateGroupTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;

        public CreateGroupTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateGroup()
        {
            var user = await _fixture.LoginAsUser("user@test.com","user");
            var sut = _fixture.GetClient<IGroupsApi>();
            var command = new CreateGroupApiRequest(Guid.NewGuid(), Guid.NewGuid(), "test");

            var result = await sut.CreateGroup(user.BearerToken(), command);

            result.IsSuccessStatusCode.Should().BeTrue();
            var group = await sut.GetGroup(user.BearerToken(), command.GroupId);
            group.GroupId.Should().Be(command.GroupId);
            group.Name.Should().Be(command.GroupName);
            group.CreatedAt.Should().NotBe(default);
            var owner = await sut.GetOwner(user.BearerToken(), group.GroupId);
            owner.Should().NotBeNull();
            owner.CreatedAt.Should().NotBe(default);
            owner.IsOwner.Should().BeTrue();
            owner.UserId.Should().Be(Guid.Parse(user.Subject));
            owner.MemberId.Should().NotBeEmpty();
            owner.Name.Should().Be("user");
        }
    }
}