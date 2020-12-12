using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Groups.Api.Hubs
{
    public class GroupDto
    {
        public Guid GroupId { get; }
        public string Name { get; }

        public GroupDto(Guid groupId, string name)
        {
            GroupId = groupId;
            Name = name;
        }
    }

    public class InvitationDto
    {
        public Guid GroupId { get; }
        public Guid InvitationId { get; }
        public string Code { get; }

        public InvitationDto(Guid groupId, Guid invitationId, string code)
        {
            GroupId = groupId;
            InvitationId = invitationId;
            Code = code;
        }
    }

    public class ChannelDto
    {
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public string ChannelName { get; }

        public ChannelDto(Guid groupId, Guid channelId, string channelName)
        {
            GroupId = groupId;
            ChannelId = channelId;
            ChannelName = channelName;
        }
    }
    public interface IGroupsHubContract
    {
        public Task OnGroupRemoved(GroupDto @event);
        public Task OnInvitationCreated(InvitationDto @event);
        public Task OnInvitationRevoked();
        public Task OnMemberAddedToGroup();
        public Task OnMemberKickedOutOfGroup();
        public Task OnMemberLeftGroup();
        public Task OnRoleCreated();
        public Task OnRoleRemoved();
        public Task OnRoleAddedToMember();
        public Task OnRoleRemovedFromMember();
        public Task OnPermissionAddedToRole();
        public Task OnPermissionRemovedFromRole();
        public Task OnChannelCreated(ChannelDto dto);
        public Task OnChannelRemoved(ChannelDto dto);
        public Task OnMessageCreated(GroupMessageDto dto);
    }

    [Authorize]
    public class GroupsHub: Hub<IGroupsHubContract>
    {
    }
}