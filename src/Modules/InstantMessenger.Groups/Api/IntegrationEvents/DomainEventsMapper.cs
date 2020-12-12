using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Api.IntegrationEvents
{
    public class DomainEventsMapper : IDomainEventMapper
    {
        public Maybe<IIntegrationEvent> Map(IDomainEvent @event)
        {
            return @event switch
            {
                Domain.Events.GroupRemovedEvent e =>new IntegrationEvents.GroupRemovedEvent(e.GroupId, e.GroupName,e.AllowedMembers.Select(m=>m.UserId.Value)),
                Domain.Events.InvitationCreatedEvent e =>new IntegrationEvents.InvitationCreatedEvent(e.GroupId,e.InvitationId,e.InvitationCode),
                Domain.Events.InvitationRevokedEvent e =>new IntegrationEvents.InvitationRevokedEvent(e.GroupId, e.InvitationId, e.InvitationCode),
                Domain.Events.MemberAddedToGroupEvent e =>new IntegrationEvents.MemberAddedToGroupEvent(),
                Domain.Events.MemberKickedFromGroupEvent e =>new IntegrationEvents.MemberKickedFromGroupEvent(),
                Domain.Events.MemberLeftGroupEvent e =>new IntegrationEvents.MemberLeftGroupEvent(),
                Domain.Events.RoleCreatedEvent e =>new IntegrationEvents.RoleCreatedEvent(),
                Domain.Events.RoleRemovedEvent e =>new IntegrationEvents.RoleRemovedEvent(),
                Domain.Events.RoleAddedToMemberEvent e =>new IntegrationEvents.RoleAddedToMemberEvent(),
                Domain.Events.RoleRemovedFromMemberEvent e =>new IntegrationEvents.RoleRemovedFromMemberEvent(),
                Domain.Events.PermissionAddedToRoleEvent e =>new IntegrationEvents.PermissionAddedToRoleEvent(),
                Domain.Events.PermissionRemovedFromRoleEvent e =>new IntegrationEvents.PermissionRemovedFromRoleEvent(),
                Domain.Events.ChannelCreatedEvent e =>new IntegrationEvents.ChannelCreatedEvent(e.ChannelId, e.GroupId, e.ChannelName),
                Domain.Events.ChannelRemovedEvent e =>new IntegrationEvents.ChannelRemovedEvent(e.ChannelId, e.GroupId, e.ChannelName),
                Domain.Events.MessageCreatedEvent e => new IntegrationEvents.MessageCreatedEvent(e.MessageId,e.GroupId,e.ChannelId, e.SenderId,e.Content.Value, e.CreatedAt),
                _ => Maybe<IIntegrationEvent>.None
            };
        }

        public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events)
        {
            return events.SelectMany(e=>Map(e).ToList());
        }
    }
}