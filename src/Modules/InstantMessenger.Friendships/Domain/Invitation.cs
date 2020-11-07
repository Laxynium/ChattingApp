using System;
using CSharpFunctionalExtensions;
using InstantMessenger.Friendships.Api;
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
        public static Invitation Create(Guid senderId, Guid receiverId, IClock clock) => new Invitation(Guid.NewGuid(), senderId, receiverId, clock.GetCurrentInstant().InUtc().ToDateTimeOffset(), InvitationStatus.Pending);

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