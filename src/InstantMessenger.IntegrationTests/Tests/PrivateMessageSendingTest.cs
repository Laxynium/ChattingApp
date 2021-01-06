using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using InstantMessenger.PrivateMessages.Application.Features.SendMessage;
using Xunit;

namespace InstantMessenger.IntegrationTests.Tests
{
    [Collection("Server collection")]

    public class PrivateMessageSendingTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;

        public PrivateMessageSendingTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Message_Is_Sent()
        {
            var userA = await _fixture.LoginAsUser("test1@test.com", "test1");
            var userB = await _fixture.LoginAsUser("test2@test.com", "test2");
            var conversation = await _fixture.CreateConversation(userA, userB);

            var sut = _fixture.GetClient<IPrivateMessagesApi>();

            var result = await sut.SendMessage(userA.BearerToken(), new SendMessageApiRequest(conversation.ConversationId, "test"));
            result.EnsureSuccessStatusCode();

            var messages = await sut.GetMessages(userB.BearerToken(), conversation.ConversationId);
            messages.Should().SatisfyRespectively(
                x =>
                {
                    x.MessageId.Should().NotBeEmpty();
                    x.ConversationId.Should().Be(conversation.ConversationId);
                    x.CreatedAt.Should().NotBe(default);
                    x.From.Should().Be(conversation.FirstParticipant);
                    x.To.Should().Be(conversation.SecondParticipant);
                    x.Text.Should().Be("test");
                }
            );
        }

    }
}