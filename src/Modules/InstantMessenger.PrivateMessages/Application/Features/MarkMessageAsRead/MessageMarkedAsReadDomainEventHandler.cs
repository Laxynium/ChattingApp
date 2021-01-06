using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Application.Hubs;
using InstantMessenger.PrivateMessages.Application.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.PrivateMessages.Application.Features.MarkMessageAsRead
{
    public class MessageMarkedAsReadIntegrationEventHandler : IIntegrationEventHandler<MessageMarkedAsReadIntegrationEvent>
    {
        private readonly IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> _hubContext;

        public MessageMarkedAsReadIntegrationEventHandler(IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(MessageMarkedAsReadIntegrationEvent @event)
        {
            await _hubContext.Clients.Users(@event.SenderId.ToString("N"),@event.ReceiverId.ToString("N")).OnMessageRead(
                new MessageMarkedAsReadDto(
                    @event.MessageId,
                    @event.ConversationId,
                    @event.SenderId,
                    @event.ReceiverId,
                    @event.ReadAt
                )
            );
        }
    }
}