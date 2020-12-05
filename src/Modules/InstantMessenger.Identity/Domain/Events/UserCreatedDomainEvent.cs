using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Identity.Domain.Events
{
    public class UserCreatedDomainEvent : IDomainEvent
    {
        public UserId Id { get; }
        public Email Email { get; }
        public string PasswordHash { get; }

        public UserCreatedDomainEvent(UserId id, Email email, string passwordHash)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}