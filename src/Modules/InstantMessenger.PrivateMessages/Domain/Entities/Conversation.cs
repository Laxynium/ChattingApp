using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.PrivateMessages.Domain.Exceptions;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Domain.Entities
{
    public class Conversation : Entity<ConversationId>
    {
        public Participant FirstParticipant { get; }
        public Participant SecondParticipant { get; }

        private Conversation(){}
        private Conversation(ConversationId id, Participant firstParticipant, Participant secondParticipant):base(id)
        {
            FirstParticipant = firstParticipant;
            SecondParticipant = secondParticipant;
        }

        public static Conversation Create(ConversationId conversationId, Participant firstParticipant, Participant secondParticipant) 
            => new(conversationId, firstParticipant,secondParticipant);

        public Message Send(MessageId messageId, MessageBody message, Participant from, IClock clock)
        {
            if (from != FirstParticipant && from != SecondParticipant)
                throw new InvalidParticipantException();

            return Message.Create(messageId,message, Id, from, GetAnotherParticipant(from), clock);
        }

        public void Remove()
        {
            Apply(new ConversationRemovedDomainEvent(Id, FirstParticipant, SecondParticipant));
        }

        private Participant GetAnotherParticipant(Participant participant) =>
            participant == FirstParticipant ? SecondParticipant : FirstParticipant;
    }
}