using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Identity.Application.IntegrationEvents
{
    public class DomainEventMapper : IDomainEventMapper
    {
        public Maybe<IIntegrationEvent> Map(IDomainEvent @event)
        {
            return @event switch
            {
                Domain.Events.ActivationLinkCreatedDomainEvent x=> new ActivationLinkCreatedEvent(x.Id, x.Token,x.UserId,x.CreatedAt),
                Domain.Events.AccountActivatedDomainEvent x=> new AccountActivatedEvent(x.UserId, x.Email,x.Nickname),
                _ => Maybe<IIntegrationEvent>.None
            };
        }

        public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events)
        {
            return events.Select(Map).Where(x=>x.HasValue).Select(x=>x.Value);
        }
    }
}