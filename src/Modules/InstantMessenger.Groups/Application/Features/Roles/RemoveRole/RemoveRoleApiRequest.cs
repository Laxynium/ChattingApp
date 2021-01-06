using System;

namespace InstantMessenger.Groups.Application.Features.Roles.RemoveRole
{
    public class RemoveRoleApiRequest
    {
        public Guid GroupId { get; set; }
        public Guid RoleId { get; set; }

        public RemoveRoleApiRequest()
        {
            
        }
        public RemoveRoleApiRequest(Guid groupId, Guid roleId)
        {
            GroupId = groupId;
            RoleId = roleId;
        }
    }
}