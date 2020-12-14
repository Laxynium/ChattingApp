using System;
using System.Collections.Generic;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.UpdateRolePermissions
{
    public class UpdateRolePermissionsApiRequest : ICommand
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public List<PermissionCommandItem> Permissions { get; }

        public UpdateRolePermissionsApiRequest(Guid groupId, Guid roleId, List<PermissionCommandItem> permissions)
        {
            GroupId = groupId;
            RoleId = roleId;
            Permissions = permissions;
        }
    }
}