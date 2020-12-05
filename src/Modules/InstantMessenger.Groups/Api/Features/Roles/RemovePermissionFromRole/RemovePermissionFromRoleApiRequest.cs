using System;

namespace InstantMessenger.Groups.Api.Features.Roles.RemovePermissionFromRole
{
    public class RemovePermissionFromRoleApiRequest
    {
        public Guid GroupId { get; set; }
        public Guid RoleId { get; set; }
        public string PermissionName { get; set; }

        public RemovePermissionFromRoleApiRequest()
        {
            
        }
        public RemovePermissionFromRoleApiRequest(Guid groupId, Guid roleId, string permissionName)
        {
            GroupId = groupId;
            RoleId = roleId;
            PermissionName = permissionName;
        }
    }
}