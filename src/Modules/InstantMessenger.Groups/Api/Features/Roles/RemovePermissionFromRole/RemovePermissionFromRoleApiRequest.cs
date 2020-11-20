using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.RemovePermissionFromRole
{
    public class RemovePermissionFromRoleApiRequest : ICommand
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string PermissionName { get; }
        public RemovePermissionFromRoleApiRequest(Guid groupId, Guid roleId, string permissionName)
        {
            GroupId = groupId;
            RoleId = roleId;
            PermissionName = permissionName;
        }
    }
}