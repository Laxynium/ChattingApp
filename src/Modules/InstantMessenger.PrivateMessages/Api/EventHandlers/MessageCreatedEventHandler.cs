using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Hubs;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.PrivateMessages.Api.EventHandlers
{
    public class MessageCreatedEventHandler:IEventHandler<MessageCreatedEvent>
    {
        private readonly IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> _hubContext;

        public MessageCreatedEventHandler(IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(MessageCreatedEvent @event)
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