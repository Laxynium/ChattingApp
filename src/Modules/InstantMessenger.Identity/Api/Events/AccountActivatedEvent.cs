using System;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Identity.Api.Events
{
    public class AccountActivatedEvent : IEvent
    {
        public Guid UserId { get; }
        public string Email { get;}
        public string Nickname { get; }

        public AccountActivatedEvent(Guid userId, string userEmail, string userNickname)
        {
            UserId = userId;
            Email = userEmail;
            Nickname = userNickname;
        }
    }
}