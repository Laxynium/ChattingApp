using CSharpFunctionalExtensions;
using InstantMessenger.PrivateMessages.Domain.Exceptions;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Domain
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

        public static Conversation Create(Participant firstParticipant, Participant secondParticipant) 
            => new Conversation(ConversationId.Create(), firstParticipant,secondParticipant);

        public Message Send(MessageBody message, Participant from, IClock clock)
        {
            if (from != FirstParticipant && from != SecondParticipant)
                throw new InvalidParticipantException();

            return Message.Create(message, Id, from, GetAnotherParticipant(from), clock);
        }

        private Participant GetAnotherParticipant(Participant participant) =>
            participant == FirstParticipant ? SecondParticipant : FirstParticipant;
    }
}