using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Application.ResponseDtos;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Application.IntegrationEvents
{
    public class DomainEventsMapper : IDomainEventMapper
    {
        public Maybe<IIntegrationEvent> Map(IDomainEvent @event)
        {
            return @event switch
            {
                Domain.Events.GroupRemovedEvent e =>new GroupRemovedEvent(e.GroupId, e.GroupName, e.CreatedAt,e.AllowedMembers.Select(m => m.UserId.Value)),
                Domain.Events.InvitationCreatedEvent e =>new InvitationCreatedEvent(e.GroupId, e.InvitationId, e.InvitationCode,
                    e.ExpirationTime.ToDto(),e.UsageCounter.ToDto()),
                Domain.Events.InvitationRevokedEvent e =>new InvitationRevokedEvent(e.GroupId, e.InvitationId, e.InvitationCode, 
                    e.ExpirationTime.ToDto(), e.UsageCounter.ToDto()),
                Domain.Events.MemberAddedToGroupEvent e =>new MemberAddedToGroupEvent(),
                Domain.Events.MemberKickedFromGroupEvent e =>new MemberKickedFromGroupEvent(),
                Domain.Events.MemberLeftGroupEvent e =>new MemberLeftGroupEvent(),
                Domain.Events.RoleCreatedEvent e =>new RoleCreatedEvent(),
                Domain.Events.RoleRemovedEvent e =>new RoleRemovedEvent(),
                Domain.Events.RoleAddedToMemberEvent e =>new RoleAddedToMemberEvent(e.GroupId, e.UserId, e.RoleId,e.RoleName, e.RolePriority),
                Domain.Events.RoleRemovedFromMemberEvent e => new RoleRemovedFromMemberEvent(e.GroupId, e.UserId, e.RoleId, e.RoleName, e.RolePriority),
                Domain.Events.PermissionAddedToRoleEvent e => new PermissionAddedToRoleEvent(e.GroupId, e.RoleId, e.PermissionName, e.PermissionValue),
                Domain.Events.PermissionRemovedFromRoleEvent e =>new PermissionRemovedFromRoleEvent(e.GroupId, e.RoleId, e.PermissionName, e.PermissionValue),
                Domain.Events.ChannelCreatedEvent e =>new ChannelCreatedEvent(e.ChannelId, e.GroupId, e.ChannelName),
                Domain.Events.ChannelRemovedEvent e =>new ChannelRemovedEvent(e.ChannelId, e.GroupId, e.ChannelName),
                Domain.Events.ChannelMemberPermissionOverridesChanged e => new ChannelMemberPermissionOverridesChangedEvent(
                        e.GroupId,
                        e.ChannelId,
                        e.UserId,
                        e.Overrides.Select(o => new PermissionOverrideDto {Permission = o.Permission.Name, Type = (OverrideTypeDto) o.Type})
                    ),
                Domain.Events.ChannelRolePermissionOverridesChangedEvent e => new ChannelRolePermissionOverridesChangedEvent(
                    e.GroupId,
                    e.ChannelId,
                    e.RoleId,
                    e.Overrides.Select(
                        o => new PermissionOverrideDto{Permission = o.Permission.Name,Type = (OverrideTypeDto) o.Type})
                    ),
                Domain.Events.MessageCreatedEvent e => new MessageCreatedEvent(e.MessageId, e.GroupId, e.ChannelId, e.SenderId, e.Content.Value, e.CreatedAt),
                _ => Maybe<IIntegrationEvent>.None
            };
        }

        public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> events)
        {
            return events.SelectMany(e=>Map(e).ToList());
        }
    }
}