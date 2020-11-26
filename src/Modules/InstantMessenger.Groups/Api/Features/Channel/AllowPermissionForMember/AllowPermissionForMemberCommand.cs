using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.AllowPermissionForMember
{
    public class AllowPermissionForMemberCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid MemberUserId { get; }
        public string Permission { get; }

        public AllowPermissionForMemberCommand(Guid userId, Guid groupId, Guid channelId, Guid memberUserId, string permission)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            MemberUserId = memberUserId;
            Permission = permission;
        }

    }
}