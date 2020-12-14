using System;

namespace InstantMessenger.Groups.Api.ResponseDtos
{
    public class RolePermissionDto
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public PermissionDto Permission { get; }

        public RolePermissionDto(Guid groupId, Guid roleId, PermissionDto permission)
        {
            GroupId = groupId;
            RoleId = roleId;
            Permission = permission;
        }
    }
}