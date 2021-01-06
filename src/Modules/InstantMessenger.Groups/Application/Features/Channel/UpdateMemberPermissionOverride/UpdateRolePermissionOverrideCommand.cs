using System;
using System.Collections.Generic;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Channel.UpdateMemberPermissionOverride
{
    public class UpdateMemberPermissionOverrideCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid MemberUserId { get; }
        public List<PermissionOverrideCommandItem> Overrides { get; }

        public UpdateMemberPermissionOverrideCommand(Guid userId, Guid groupId, Guid channelId, Guid memberUserId, List<PermissionOverrideCommandItem> overrides)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            MemberUserId = memberUserId;
            Overrides = overrides;
        }
    }

    public class PermissionOverrideCommandItem
    {
        public string Permission { get; }
        public PermissionOverrideTypeCommandItem Type { get; }

        public PermissionOverrideCommandItem(string permission, PermissionOverrideTypeCommandItem type)
        {
            Permission = permission;
            Type = type;
        }
    }

    public enum PermissionOverrideTypeCommandItem
    {
        Allow = 1,
        Deny = -1,
        Neutral = 0
    }
}