using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Identity.Domain.Events
{
    public class AccountActivatedDomainEvent : IDomainEvent
    {
        public UserId UserId { get; }
        public Email Email { get; }
        public Nickname Nickname { get; }

        public AccountActivatedDomainEvent(UserId userId, Email userEmail, Nickname userNickname)
        {
            UserId = userId;
            Email = userEmail;
            Nickname = userNickname;
        }
    }
}