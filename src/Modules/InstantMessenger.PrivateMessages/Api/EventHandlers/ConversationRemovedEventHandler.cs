using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Hubs;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.PrivateMessages.Api.EventHandlers
{
    public class ConversationRemovedEventHandler : IEventHandler<ConversationRemovedEvent>
    {
        private readonly IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> _hubContext;

        public ConversationRemovedEventHandler(IHubContext<PrivateMessagesHub,IPrivateMessagesHubContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(ConversationRemovedEvent @event)
        {
            await _hubContext.Clients.Users(@event.FirstParticipantId.ToString("N"), @event.SecondParticipantId.ToString("N"))
                .OnConversationRemoved(new ConversationDto(@event.ConversationId, @event.FirstParticipantId, @event.SecondParticipantId));
        }
    }
}