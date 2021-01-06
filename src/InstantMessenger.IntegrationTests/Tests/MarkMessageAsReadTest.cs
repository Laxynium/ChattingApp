using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using InstantMessenger.PrivateMessages.Application.Features.MarkMessageAsRead;
using InstantMessenger.PrivateMessages.Application.Features.SendMessage;
using Xunit;

namespace InstantMessenger.IntegrationTests.Tests
{
    [Collection("Server collection")]
    public class MarkMessageAsReadTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;

        public MarkMessageAsReadTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Message_Is_Sent()
        {
            var userA = await _fixture.LoginAsUser("test1@test.com","test1");
            var userB = await _fixture.LoginAsUser("test2@test.com", "test2");
            var conversation = await _fixture.CreateConversation(userA, userB);
            var sut = _fixture.GetClient<IPrivateMessagesApi>();
            await sut.SendMessage(userA.BearerToken(), new SendMessageApiRequest(conversation.ConversationId, "test"));
            var message = (await sut.GetMessages(userB.BearerToken(), conversation.ConversationId)).Last();

            var result = await sut.MarkMessageAsRead(userB.BearerToken(), new MarkMessageAsReadApiRequest(message.MessageId));

            result.IsSuccessStatusCode.Should().BeTrue();
            var afterRead = (await sut.GetMessages(userA.BearerToken(), conversation.ConversationId)).Last();
            afterRead.ReadAt.Should().NotBeNull();
        }
    }
}