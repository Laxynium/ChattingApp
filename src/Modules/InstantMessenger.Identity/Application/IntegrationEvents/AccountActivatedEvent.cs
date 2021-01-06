using System;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Identity.Application.IntegrationEvents
{
    public class AccountActivatedEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Nickname { get; }

        public AccountActivatedEvent(Guid userId, string userEmail, string userNickname)
        {
            UserId = userId;
            Email = userEmail;
            Nickname = userNickname;
        }
    }
}