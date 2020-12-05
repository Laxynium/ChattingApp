using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Events;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Rules;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;
using NodaTime;

namespace InstantMessenger.Friendships.Domain.Entities
{
    public class Invitation : Entity<InvitationId>
    {
        public PersonId SenderId { get; }
        public PersonId ReceiverId { get; }
        public DateTimeOffset CreatedAt { get; }
        public InvitationStatus Status { get; private set; }
        private Invitation(){}

        private Invitation(InvitationId id, PersonId senderId, PersonId receiverId, DateTimeOffset createdAt, InvitationStatus status): base(id)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            CreatedAt = createdAt;
            Status = status;
            Apply(new FriendshipInvitationCreatedDomainEvent(id,senderId,receiverId,createdAt));
        }

        public static async Task<Invitation> Create(PersonId sender, PersonId receiver, IUniquePendingInvitationRule rule, IClock clock)
        {
            if(sender == receiver)
                throw new InvalidInvitationException();

            if(!await rule.IsMet(sender, receiver))
                throw new InvitationAlreadyExistsException();

            return new Invitation(InvitationId.Create(), sender, receiver, clock.GetCurrentInstant().InUtc().ToDateTimeOffset(), InvitationStatus.Pending);
        }

        public Friendship AcceptInvitation(PersonId acceptor, IClock clock)
        {
            if (acceptor != ReceiverId)
                throw new PersonNotFoundException();

            if(Status != InvitationStatus.Pending)
                throw new InvalidInvitationStateException();

            Status = InvitationStatus.Accepted;

            Apply(new FriendshipInvitationAcceptedDomainEvent(Id, SenderId, ReceiverId, CreatedAt));

            return Friendship.Create(SenderId, ReceiverId, clock);
        }

        public void RejectInvitation(PersonId rejecter)
        {
            if (rejecter != ReceiverId)
                throw new PersonNotFoundException();

            if (Status != InvitationStatus.Pending)
                throw new InvalidInvitationStateException();

            Apply(new FriendshipInvitationRejectedDomainEvent(Id,SenderId,ReceiverId,CreatedAt));

            Status = InvitationStatus.Rejected;
        }

        public void CancelInvitation(PersonId sender)
        {
            if (sender != SenderId)
                throw new PersonNotFoundException();

            if (Status != InvitationStatus.Pending)
                throw new InvalidInvitationStateException();

            Apply(new FriendshipInvitationCanceledDomainEvent(Id, SenderId, ReceiverId, CreatedAt));

            Status = InvitationStatus.Canceled;
        }
    }
}