using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Friendships.Domain.Events;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Friendships.Api.IntegrationEvents
{
    public class DomainEventsMapper : IDomainEventMapper
    {
        public Maybe<IIntegrationEvent> Map(IDomainEvent @event) => @event switch
        {
            FriendshipCreatedDomainEvent e => new FriendshipCreatedIntegrationEvent(e.FriendshipId,e.FirstPerson,e.SecondPerson,e.CreatedAt),
            FriendshipInvitationCreatedDomainEvent e => new FriendshipInvitationCreatedIntegrationEvent(e.InvitationId, e.SenderId,e.ReceiverId,e.CreatedAt),
            FriendshipInvitationAcceptedDomainEvent e => new FriendshipInvitationAcceptedIntegrationEvent(e.InvitationId,e.SenderId,e.ReceiverId,e.CreatedAt),
            FriendshipInvitationRejectedDomainEvent e => new FriendshipInvitationRejectedIntegrationEvent(e.InvitationId,e.SenderId,e.ReceiverId,e.CreatedAt),
            FriendshipInvitationCanceledDomainEvent e => new FriendshipInvitationCanceledIntegrationEvent(e.InvitationId,e.SenderId,e.ReceiverId,e.CreatedAt),
            FriendshipRemovedDomainEvent e => new FriendshipRemovedIntegrationEvent(e.FriendshipId,e.FirstPerson,e.SecondPerson,e.CreatedAt),
            _=>Maybe<IIntegrationEvent>.None
        };

        public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events)
        {
            return events.SelectMany(x => Map(x).ToList());
        }
    }
}