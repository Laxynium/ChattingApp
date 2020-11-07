using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Friendships.Api.Features.RejectInvitation;
using InstantMessenger.Friendships.Api.Features.SendInvitation;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using Xunit;

namespace InstantMessenger.IntegrationTests.Tests
{
    [Collection("Server collection")]

    public class RejectInvitationTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;

        public RejectInvitationTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Reject_Invitation()
        {
            var userA = await _fixture.LoginAsUser("test1@test.com");
            var userB = await _fixture.LoginAsUser("test2@test.com");

            var sut = _fixture.GetClient<IFriendshipsApi>();

            await sut.SendFriendshipInvitation($"Bearer {userA.Token}", new SendFriendshipInvitationApiRequest(Guid.Parse(userB.Subject)));

            var pendingInvitation = await sut.GetPendingInvitations($"Bearer {userB.Token}");
            pendingInvitation.Should().SatisfyRespectively(x =>
            {
                x.InvitationId.Should().NotBeEmpty();
                x.SenderId.Should().Be(Guid.Parse(userA.Subject));
                x.ReceiverId.Should().Be(Guid.Parse(userB.Subject));
                x.Status.Should().Be(InvitationStatus.Pending.ToString());
                x.CreatedAt.Should().NotBe(default);
            });

            await sut.RejectInvitation($"Bearer {userB.Token}", new RejectFriendshipInvitationApiRequest(pendingInvitation.First().InvitationId));

            var friends = await sut.GetFriendships($"Bearer {userA.Token}");
            friends.Should().BeEmpty();
        }

    }
}