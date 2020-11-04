using System;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Identity.Api.Events
{
    public class AccountActivatedEvent : IEvent
    {
        public Guid UserId { get; }
        public string Email { get;}

        public AccountActivatedEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}