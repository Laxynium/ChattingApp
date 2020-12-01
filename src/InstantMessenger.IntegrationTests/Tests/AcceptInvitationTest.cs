using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Friendships.Api.Features.AcceptInvitation;
using InstantMessenger.Friendships.Api.Features.SendInvitation;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using Xunit;

namespace InstantMessenger.IntegrationTests.Tests
{
    [Collection("Server collection")]

    public class AcceptInvitationTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;

        public AcceptInvitationTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Accept_Invitation()
        {
            var userA = await _fixture.LoginAsUser("test1@test.com","test1");
            var userB = await _fixture.LoginAsUser("test2@test.com", "test2");
            
            var sut =  _fixture.GetClient<IFriendshipsApi>();

            await sut.SendFriendshipInvitation(userA.BearerToken(), new SendFriendshipInvitationApiRequest(userB.Nickname));

            var pendingInvitation = await sut.GetPendingInvitations(userB.BearerToken());
            pendingInvitation.Should().SatisfyRespectively(x =>
            {
                x.InvitationId.Should().NotBeEmpty();
                x.SenderId.Should().Be(userA.UserId);
                x.ReceiverId.Should().Be(userB.UserId);
                x.Status.Should().Be(InvitationStatus.Pending.ToString());
                x.CreatedAt.Should().NotBe(default);
            });

            await sut.AcceptInvitation(userB.BearerToken(), new AcceptFriendshipInvitationApiRequest(pendingInvitation.First().InvitationId));

            var friends = await sut.GetFriendships(userA.BearerToken());
            friends.Should().SatisfyRespectively(x =>
            {
                x.FriendshipId.Should().NotBeEmpty();
                x.Me.Should().Be(userA.UserId);
                x.Friend.Should().Be(userB.UserId);
                x.CreatedAt.Should().NotBe(default);
            });

            var privateMessagesApi = _fixture.GetClient<IPrivateMessagesApi>();
            var conversations = await privateMessagesApi.GetConversations(userA.BearerToken());
            conversations.Should().SatisfyRespectively(x =>
            {
                x.ConversationId.Should().NotBeEmpty();
                x.FirstParticipant.Should().Be(userA.UserId);
                x.SecondParticipant.Should().Be(userB.UserId);
            });
        }

    }
}