using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.PrivateMessages.Domain.Events
{
    public class ConversationRemovedDomainEvent : IDomainEvent
    {
        public ConversationId ConversationId { get; }
        public Participant FirstParticipant { get; }
        public Participant SecondParticipant { get; }

        public ConversationRemovedDomainEvent(ConversationId conversationId, Participant firstParticipant, Participant secondParticipant)
        {
            ConversationId = conversationId;
            FirstParticipant = firstParticipant;
            SecondParticipant = secondParticipant;
        }
    }
}