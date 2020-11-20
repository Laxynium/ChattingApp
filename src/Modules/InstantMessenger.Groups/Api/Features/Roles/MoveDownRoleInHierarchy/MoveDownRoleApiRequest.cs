using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.MoveDownRoleInHierarchy
{
    public class MoveDownRoleApiRequest : ICommand
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public MoveDownRoleApiRequest(Guid groupId, Guid roleId)
        {
            GroupId = groupId;
            RoleId = roleId;
        }
    }
}