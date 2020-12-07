using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Members.AssignRole
{
    public class AssignRoleToMemberApiRequest : ICommand
    {
        public Guid GroupId { get; }
        public Guid MemberUserId { get; }
        public Guid RoleId { get; }

        public AssignRoleToMemberApiRequest(Guid groupId, Guid memberUserId, Guid roleId)
        {
            GroupId = groupId;
            MemberUserId = memberUserId;
            RoleId = roleId;
        }
    }
}