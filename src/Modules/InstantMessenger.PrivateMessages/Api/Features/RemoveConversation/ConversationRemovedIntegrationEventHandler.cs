using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Hubs;
using InstantMessenger.PrivateMessages.Api.IntegrationEvents;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.PrivateMessages.Api.Features.RemoveConversation
{
    public class ConversationRemovedIntegrationEventHandler : IIntegrationEventHandler<ConversationRemovedIntegrationEvent>
    {
        private readonly IHubContext<PrivateMessagesHub, IPrivateMessagesHubContract> _hubContext;

        public ConversationRemovedIntegrationEventHandler(IHubContext<PrivateMessagesHub,IPrivateMessagesHubContract> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task HandleAsync(ConversationRemovedIntegrationEvent @event)
        {
            await _hubContext.Clients.Users(@event.FirstParticipantId.ToString("N"), @event.SecondParticipantId.ToString("N"))
                .OnConversationRemoved(new ConversationDto(@event.ConversationId, @event.FirstParticipantId, @event.SecondParticipantId));
        }
    }
}