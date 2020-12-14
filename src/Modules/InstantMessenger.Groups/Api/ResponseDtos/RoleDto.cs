using System;

namespace InstantMessenger.Groups.Api.ResponseDtos
{
    public class RoleDto
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string Name { get; }
        public int Priority { get; }

        public RoleDto(Guid groupId, Guid roleId, string name, int priority)
        {
            RoleId = roleId;
            Name = name;
            Priority = priority;
            GroupId = groupId;
        }
    }
}