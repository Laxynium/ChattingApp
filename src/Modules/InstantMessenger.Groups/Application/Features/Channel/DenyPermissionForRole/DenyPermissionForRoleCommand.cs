using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Channel.DenyPermissionForRole
{
    public class DenyPermissionForRoleCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid RoleId { get; }
        public string Permission { get; }

        public DenyPermissionForRoleCommand(Guid userId, Guid groupId, Guid channelId, Guid roleId, string permission)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            RoleId = roleId;
            Permission = permission;
        }

    }
}