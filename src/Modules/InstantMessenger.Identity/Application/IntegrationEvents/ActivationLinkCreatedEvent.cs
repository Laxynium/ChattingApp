using System;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Identity.Application.IntegrationEvents
{
    public class ActivationLinkCreatedEvent : IIntegrationEvent
    {
        public Guid Id { get; }
        public string Token { get; }
        public Guid UserId { get; }
        public DateTimeOffset CreatedAt { get; }

        public ActivationLinkCreatedEvent(Guid id, string token, Guid userId, DateTimeOffset createdAt)
        {
            Id = id;
            Token = token;
            UserId = userId;
            CreatedAt = createdAt;
        }

    }

    public class ActivationLinkCreatedEventHandler: IIntegrationEventHandler<ActivationLinkCreatedEvent>
    {
        public Task HandleAsync(ActivationLinkCreatedEvent @event)
        {
            return Task.CompletedTask;   
        }
    }
}