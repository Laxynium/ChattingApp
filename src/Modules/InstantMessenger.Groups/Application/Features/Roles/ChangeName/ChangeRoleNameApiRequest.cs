using System;

namespace InstantMessenger.Groups.Application.Features.Roles.ChangeName
{
    public class ChangeRoleNameApiRequest
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string Name { get; }

        public ChangeRoleNameApiRequest(Guid groupId, Guid roleId, string name)
        {
            GroupId = groupId;
            RoleId = roleId;
            Name = name;
        }
    }
}