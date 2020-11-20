using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.RemoveRole
{
    public class RemoveRoleApiRequest : ICommand
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public RemoveRoleApiRequest(Guid groupId, Guid roleId)
        {
            GroupId = groupId;
            RoleId = roleId;
        }
    }
}