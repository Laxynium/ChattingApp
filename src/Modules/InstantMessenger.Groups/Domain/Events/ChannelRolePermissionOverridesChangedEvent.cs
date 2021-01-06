using System.Collections.Generic;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class ChannelRolePermissionOverridesChangedEvent : IDomainEvent
    {
        public GroupId GroupId { get; }
        public ChannelId ChannelId { get; }
        public RoleId RoleId { get; }
        public IEnumerable<PermissionOverride> Overrides { get; }

        public ChannelRolePermissionOverridesChangedEvent(GroupId groupId, ChannelId channelId, RoleId roleId, IEnumerable<PermissionOverride> overrides)
        {
            GroupId = groupId;
            ChannelId = channelId;
            RoleId = roleId;
            Overrides = overrides;
        }
    }
}