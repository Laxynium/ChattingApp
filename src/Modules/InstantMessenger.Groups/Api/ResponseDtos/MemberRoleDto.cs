using System;
using InstantMessenger.Groups.Api.Queries;

namespace InstantMessenger.Groups.Api.ResponseDtos
{
    public class MemberRoleDto
    {
        public Guid UserId { get; }
        public RoleDto Role { get; }

        public MemberRoleDto(Guid userId, RoleDto role)
        {
            UserId = userId;
            Role = role;
        }
    }
}