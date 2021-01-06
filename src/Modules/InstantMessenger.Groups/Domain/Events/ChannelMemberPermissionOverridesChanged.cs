using System.Collections.Generic;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class ChannelMemberPermissionOverridesChanged : IDomainEvent
    {
        public GroupId GroupId { get; }
        public ChannelId ChannelId { get; }
        public UserId UserId { get; }
        public IEnumerable<PermissionOverride> Overrides { get; }

        public ChannelMemberPermissionOverridesChanged(GroupId groupId, ChannelId channelId, UserId userId, IEnumerable<PermissionOverride> overrides)
        {
            GroupId = groupId;
            ChannelId = channelId;
            UserId = userId;
            Overrides = overrides;
        }
    }
}