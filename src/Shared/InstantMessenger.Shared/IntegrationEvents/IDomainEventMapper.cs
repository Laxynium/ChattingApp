using System.Collections.Generic;
using CSharpFunctionalExtensions;
using InstantMessenger.Shared.BuildingBlocks;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Shared.IntegrationEvents
{
    public interface IDomainEventMapper
    {
        Maybe<IIntegrationEvent> Map(IDomainEvent @event);
        IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events);
    }
}