using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Api.ResponseDtos;
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
                Domain.Events.GroupRemovedEvent e =>new Api.IntegrationEvents.GroupRemovedEvent(e.GroupId, e.GroupName, e.CreatedAt,e.AllowedMembers.Select(m => m.UserId.Value)),
                Domain.Events.InvitationCreatedEvent e =>new Api.IntegrationEvents.InvitationCreatedEvent(e.GroupId, e.InvitationId, e.InvitationCode,
                    e.ExpirationTime.ToDto(),e.UsageCounter.ToDto()),
                Domain.Events.InvitationRevokedEvent e =>new Api.IntegrationEvents.InvitationRevokedEvent(e.GroupId, e.InvitationId, e.InvitationCode, 
                    e.ExpirationTime.ToDto(), e.UsageCounter.ToDto()),
                Domain.Events.DomainEvents e =>new Api.IntegrationEvents.MemberAddedToGroupEvent(),
                Domain.Events.MemberKickedFromGroupEvent e =>new Api.IntegrationEvents.MemberKickedFromGroupEvent(),
                Domain.Events.MemberLeftGroupEvent e =>new Api.IntegrationEvents.MemberLeftGroupEvent(),
                Domain.Events.RoleCreatedEvent e =>new Api.IntegrationEvents.RoleCreatedEvent(),
                Domain.Events.RoleRemovedEvent e =>new Api.IntegrationEvents.RoleRemovedEvent(),
                Domain.Events.RoleAddedToMemberEvent e =>new Api.IntegrationEvents.RoleAddedToMemberEvent(e.GroupId, e.UserId, e.RoleId,e.RoleName, e.RolePriority),
                Domain.Events.RoleRemovedFromMemberEvent e => new Api.IntegrationEvents.RoleRemovedFromMemberEvent(e.GroupId, e.UserId, e.RoleId, e.RoleName, e.RolePriority),
                Domain.Events.PermissionAddedToRoleEvent e => new Api.IntegrationEvents.PermissionAddedToRoleEvent(e.GroupId, e.RoleId, e.PermissionName, e.PermissionValue),
                Domain.Events.PermissionRemovedFromRoleEvent e =>new Api.IntegrationEvents.PermissionRemovedFromRoleEvent(e.GroupId, e.RoleId, e.PermissionName, e.PermissionValue),
                Domain.Events.ChannelCreatedEvent e =>new Api.IntegrationEvents.ChannelCreatedEvent(e.ChannelId, e.GroupId, e.ChannelName),
                Domain.Events.ChannelRemovedEvent e =>new Api.IntegrationEvents.ChannelRemovedEvent(e.ChannelId, e.GroupId, e.ChannelName),
                Domain.Events.MessageCreatedEvent e => new Api.IntegrationEvents.MessageCreatedEvent(e.MessageId, e.GroupId, e.ChannelId, e.SenderId, e.Content.Value, e.CreatedAt),
                _ => Maybe<IIntegrationEvent>.None
            };
        }

        public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events)
        {
            return events.SelectMany(e=>Map(e).ToList());
        }
    }
}