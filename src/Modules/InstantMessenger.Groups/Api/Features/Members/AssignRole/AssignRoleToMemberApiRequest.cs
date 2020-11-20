using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Members.AssignRole
{
    public class RemoveRoleFromMemberApiRequest : ICommand
    {
        public Guid GroupId { get; }
        public Guid MemberUserId { get; }
        public Guid RoleId { get; }

        public RemoveRoleFromMemberApiRequest(Guid groupId, Guid memberUserId, Guid roleId)
        {
            GroupId = groupId;
            MemberUserId = memberUserId;
            RoleId = roleId;
        }
    }
}