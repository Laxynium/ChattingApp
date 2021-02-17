using System;

namespace InstantMessenger.Groups.Application.ResponseDtos
{
    public class MemberRoleDto
    {
        public Guid UserId { get; }
        public Guid MemberId { get; }
        public RoleDto Role { get; }

        public MemberRoleDto(Guid userId, Guid memberId, RoleDto role)
        {
            UserId = userId;
            MemberId = memberId;
            Role = role;
        }
    }
}