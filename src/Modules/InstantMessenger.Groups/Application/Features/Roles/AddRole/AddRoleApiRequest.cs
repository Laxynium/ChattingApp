using System;

namespace InstantMessenger.Groups.Application.Features.Roles.AddRole
{
    public class AddRoleApiRequest
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string Name { get; }

        public AddRoleApiRequest(Guid groupId, Guid roleId, string name)
        {
            GroupId = groupId;
            RoleId = roleId;
            Name = name;
        }
    }
}