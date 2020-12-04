using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Hubs;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.PrivateMessages.Api.EventHandlers
{
    public class MessageMarkedAsReadEventHandler : IEventHandler<MessageMarkedAsReadEvent>
    {
        private readonly IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> _hubContext;

        public MessageMarkedAsReadEventHandler(IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(MessageMarkedAsReadEvent @event)
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