using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Application.Hubs;
using InstantMessenger.PrivateMessages.Application.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.PrivateMessages.Application.Features.SendMessage
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