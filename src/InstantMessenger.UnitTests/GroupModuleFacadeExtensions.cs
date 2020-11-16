using System;
using System.Threading.Tasks;
using InstantMessenger.Groups;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Members.Add;
using InstantMessenger.Groups.Api.Features.Members.AssignRole;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;

namespace InstantMessenger.UnitTests
{
    internal static class GroupModuleFacadeExtensions
    {
        internal static async Task CreateGroup(this GroupsModuleFacade facade, 
            Guid userId, 
            Guid groupId, 
            string groupName)
            => await facade.SendAsync(new CreateGroupCommand(userId, groupId, groupName));

        internal static async Task AddRole(this GroupsModuleFacade facade, 
            Guid userId, 
            Guid groupId,
            Guid roleId,
            string roleName
            )
            => await facade.SendAsync(new AddRoleCommand(userId, groupId, roleId, roleName));

        internal static async Task AddMember(this GroupsModuleFacade facade,
            Guid groupId,
            Guid userIdOfMember
        )
            => await facade.SendAsync(new AddGroupMember(groupId, userIdOfMember));

        internal static async Task AssignRole(this GroupsModuleFacade facade,
            Guid userId,
            Guid groupId,
            Guid userIdOfMember,
            Guid roleId
        )
            => await facade.SendAsync(new AssignRoleToMemberCommand(userId, groupId, userIdOfMember, roleId));

        internal static async Task AddPermission(this GroupsModuleFacade facade,
            Guid userId,
            Guid groupId,
            Guid roleId,
            string permissionName
        )
            => await facade.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId, permissionName));
    }
}