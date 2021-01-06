using System;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.PrivateMessages.Application.IntegrationEvents
{
    public class ConversationRemovedIntegrationEvent : IIntegrationEvent
    {
        public Guid ConversationId { get; }
        public Guid FirstParticipantId { get; }
        public Guid SecondParticipantId { get; }

        public ConversationRemovedIntegrationEvent(Guid conversationId, Guid firstParticipantId, Guid secondParticipantId)
        {
            ConversationId = conversationId;
            FirstParticipantId = firstParticipantId;
            SecondParticipantId = secondParticipantId;
        }
    }
}