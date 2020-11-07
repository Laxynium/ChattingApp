using System;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Friendships.ExternalEvents
{
    internal class AccountActivatedEvent : IEvent
    {
        public Guid UserId { get; }

        public AccountActivatedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
