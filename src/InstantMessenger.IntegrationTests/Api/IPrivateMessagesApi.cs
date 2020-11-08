using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Queries;
using RestEase;

namespace InstantMessenger.IntegrationTests.Api
{
    [Header("User-Agent", "RestEase")]
    public interface IPrivateMessagesApi
    {
        [Get("/api/privateMessages")]
        Task<List<ConversationDto>> GetConversations(
            [Header("Authorization")] string token);
    }
}