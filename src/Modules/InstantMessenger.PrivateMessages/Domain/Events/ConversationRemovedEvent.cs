using System;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.PrivateMessages.Domain.Events
{
    [ProccessAlsoInternally]
    public class ConversationRemovedEvent : IEvent
    {
        public Guid ConversationId { get; }
        public Guid FirstParticipantId { get; }
        public Guid SecondParticipantId { get; }

        public ConversationRemovedEvent(Guid conversationId, Guid firstParticipantId, Guid secondParticipantId)
        {
            ConversationId = conversationId;
            FirstParticipantId = firstParticipantId;
            SecondParticipantId = secondParticipantId;
        }
    }
}