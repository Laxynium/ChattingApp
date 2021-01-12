using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Application.Features.MarkMessageAsRead;
using InstantMessenger.PrivateMessages.Application.Features.SendMessage;
using InstantMessenger.PrivateMessages.Application.Queries;
using RestEase;

namespace InstantMessenger.EndToEndTests.Api
{
    [Header("User-Agent", "RestEase")]
    public interface IPrivateMessagesApi
    {
        [Get("/api/privateMessages/conversations")]
        Task<List<ConversationDto>> GetConversations(
            [Header("Authorization")] string token);

        [Post("/api/privateMessages")]
        Task<HttpResponseMessage> SendMessage([Header("Authorization")] string token, [Body]SendMessageApiRequest request);

        [Post("/api/privateMessages/mark-as-read")]
        Task<HttpResponseMessage> MarkMessageAsRead([Header("Authorization")] string token, [Body]MarkMessageAsReadApiRequest request);

        [Get("/api/privateMessages/{conversationId}")]
        Task<List<MessageDto>> GetMessages([Header("Authorization")] string token, [Path]Guid conversationId);
    }
}