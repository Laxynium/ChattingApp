using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using InstantMessenger.PrivateMessages.Api.Features.MarkMessageAsRead;
using InstantMessenger.PrivateMessages.Api.Features.SendMessage;
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
            var userA = await _fixture.LoginAsUser("test1@test.com");
            var userB = await _fixture.LoginAsUser("test2@test.com");
            var conversation = await _fixture.CreateConversation(userA, userB);
            var sut = _fixture.GetClient<IPrivateMessagesApi>();
            await sut.SendMessage($"Bearer {userA.Token}", new SendMessageApiRequest(conversation.ConversationId, "test"));
            var message = (await sut.GetMessages($"Bearer {userB.Token}", conversation.ConversationId)).Last();

            var result = await sut.MarkMessageAsRead($"Bearer {userB.Token}", new MarkMessageAsReadApiRequest(message.MessageId));

            result.IsSuccessStatusCode.Should().BeTrue();
            var afterRead = (await sut.GetMessages($"Bearer {userA.Token}", conversation.ConversationId)).Last();
            afterRead.ReadAt.Should().NotBeNull();
        }
    }
}