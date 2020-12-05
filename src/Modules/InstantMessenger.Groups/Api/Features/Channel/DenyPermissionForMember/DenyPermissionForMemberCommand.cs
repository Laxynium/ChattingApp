using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.DenyPermissionForMember
{
    public class DenyPermissionForMemberCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid MemberUserId { get; }
        public string Permission { get; }

        public DenyPermissionForMemberCommand(Guid userId, Guid groupId, Guid channelId, Guid memberUserId, string permission)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            MemberUserId = memberUserId;
            Permission = permission;
        }

    }
}