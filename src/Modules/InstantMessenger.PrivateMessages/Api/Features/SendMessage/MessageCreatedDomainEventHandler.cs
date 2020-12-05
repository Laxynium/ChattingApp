using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Hubs;
using InstantMessenger.PrivateMessages.Api.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.PrivateMessages.Api.Features.SendMessage
{
    public class MessageCreatedIntegrationEventHandler:IIntegrationEventHandler<MessageCreatedIntegrationEvent>
    {
        private readonly IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> _hubContext;

        public MessageCreatedIntegrationEventHandler(IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(MessageCreatedIntegrationEvent @event)
        {
            await _hubContext.Clients.Users(@event.ReceiverId.ToString("N")).OnMessageCreated(
                new MessageDto(
                    @event.MessageId,
                    @event.ConversationId,
                    @event.SenderId,
                    @event.ReceiverId,
                    @event.Content,
                    @event.CreatedAt
                )
            );
        }
    }
}