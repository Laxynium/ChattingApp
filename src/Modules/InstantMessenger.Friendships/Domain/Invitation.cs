using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using InstantMessenger.Friendships.Api;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Rules;
using NodaTime;

namespace InstantMessenger.Friendships.Domain
{
    public class Invitation : Entity<Guid>
    {
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public DateTimeOffset CreatedAt { get; }
        public InvitationStatus Status { get; private set; }
        private Invitation(){}

        private Invitation(Guid id, Guid senderId, Guid receiverId, DateTimeOffset createdAt, InvitationStatus status): base(id)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            CreatedAt = createdAt;
            Status = status;
        }

        public static async Task<Invitation> Create(Person sender, Person receiver, IUniquePendingInvitationRule rule, IClock clock)
        {
            if(sender == receiver)
                throw new InvalidInvitationException();

            if(!await rule.IsMet(sender, receiver))
                throw new InvitationAlreadyExistsException();

            return new Invitation(Guid.NewGuid(), sender.Id, receiver.Id, clock.GetCurrentInstant().InUtc().ToDateTimeOffset(), InvitationStatus.Pending);
        }

        public Friendship AcceptInvitation(Person acceptor, IClock clock)
        {
            if (acceptor.Id != ReceiverId)
            {
                throw new PersonNotFoundException();
            }

            Status = InvitationStatus.Accepted;

            return Friendship.Create(SenderId, ReceiverId, clock);
        }

        public void RejectInvitation(Person rejecter)
        {
            if (rejecter.Id != ReceiverId)
            {
                throw new PersonNotFoundException();
            }

            Status = InvitationStatus.Rejected;
        }
    }
}