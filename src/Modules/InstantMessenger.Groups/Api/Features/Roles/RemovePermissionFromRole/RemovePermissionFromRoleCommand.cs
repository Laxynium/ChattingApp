using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.RemovePermissionFromRole
{
    public class RemovePermissionFromRoleCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string PermissionName { get; }
        public RemovePermissionFromRoleCommand(Guid userId, Guid groupId, Guid roleId, string permissionName)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
            PermissionName = permissionName;
        }
    }

    public class RemovePermissionFromRoleHandler : ICommandHandler<RemovePermissionFromRoleCommand>
    {
        private readonly IGroupRepository _groupRepository;
        public RemovePermissionFromRoleHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(RemovePermissionFromRoleCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.RemovePermissionFromRole(UserId.From(command.UserId), RoleId.From(command.RoleId), Permission.FromName(command.PermissionName));
        }
    }

}