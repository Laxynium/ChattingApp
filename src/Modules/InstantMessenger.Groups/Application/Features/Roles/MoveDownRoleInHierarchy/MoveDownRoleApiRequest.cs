using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Roles.MoveDownRoleInHierarchy
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