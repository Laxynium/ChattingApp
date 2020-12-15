using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.IntegrationEvents;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Api.ResponseDtos;
using InstantMessenger.Groups.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Groups.Api.Hubs
{
    public class AllowedActionDto
    {
        public Guid GroupId { get; }

        public AllowedActionDto(Guid groupId)
        {
            GroupId = groupId;
        }
    }
    public interface IGroupsHubContract
    {
        public Task OnGroupRemoved(GroupDto data);
        public Task OnInvitationCreated(InvitationDto data);
        public Task OnInvitationRevoked(InvitationDto data);
        public Task OnMemberAddedToGroup(MemberDto data);
        public Task OnMemberKickedOutOfGroup(MemberDto data);
        public Task OnMemberLeftGroup(MemberDto data);
        public Task OnRoleCreated(RoleDto data);
        public Task OnRoleRemoved(RoleDto data);
        public Task OnRoleAddedToMember(MemberRoleDto data);
        public Task OnRoleRemovedFromMember(MemberRoleDto data);
        public Task OnPermissionAddedToRole(RolePermissionDto data);
        public Task OnPermissionRemovedFromRole(RolePermissionDto data);
        public Task OnChannelCreated(ChannelDto dto);
        public Task OnChannelRemoved(ChannelDto dto);
        public Task OnMessageCreated(GroupMessageView data);
        public Task OnRolePermissionOverridesChanged(ChannelRolePermissionOverridesChangedEvent data);
        public Task OnMemberPermissionOverridesChanged(ChannelMemberPermissionOverridesChangedEvent data);
        public Task OnAllowedActionsChanged(AllowedActionDto data);
    }

    [Authorize]
    public class GroupsHub: Hub<IGroupsHubContract>
    {
    }
}