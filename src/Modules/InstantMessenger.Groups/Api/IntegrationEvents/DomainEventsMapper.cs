using System.Collections.Generic;
using CSharpFunctionalExtensions;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Api.IntegrationEvents
{
    public class DomainEventsMapper : IDomainEventMapper
    {
        public Maybe<IIntegrationEvent> Map(IDomainEvent @event)
        {
            return Maybe<IIntegrationEvent>.None;
        }

        public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events)
        {
            yield break;
        }
    }
}