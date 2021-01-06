using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.PrivateMessages.Application.IntegrationEvents
{
    public class DomainEventMapper : IDomainEventMapper
    {
        public Maybe<IIntegrationEvent> Map(IDomainEvent @event) => @event switch
        {
            ConversationRemovedDomainEvent e => new ConversationRemovedIntegrationEvent(e.ConversationId,e.FirstParticipant,e.SecondParticipant),
            MessageCreatedDomainEvent e => new MessageCreatedIntegrationEvent(e.MessageId, e.ConversationId, e.Sender, e.Receiver, e.Content.TextContent, e.CreatedAt),
            MessageMarkedAsReadDomainEvent e => new MessageMarkedAsReadIntegrationEvent(e.MessageId, e.ConversationId, e.Sender, e.Receiver, e.ReadAt),
            _ => Maybe<IIntegrationEvent>.None
        };

        public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events)
        {
            return events.Select(Map).Where(x=>x.HasValue).Select(x=>x.Value);
        }
    }
}