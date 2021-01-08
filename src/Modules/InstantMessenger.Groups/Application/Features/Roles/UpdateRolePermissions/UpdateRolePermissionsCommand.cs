using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Roles.UpdateRolePermissions
{
    public class PermissionCommandItem
    {
        public string Permission { get; }
        public bool IsOn { get; }

        public PermissionCommandItem(string permission, bool isOn)
        {
            Permission = permission;
            IsOn = isOn;
        }
    }
    public class UpdateRolePermissionsCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public List<PermissionCommandItem> Permissions { get; }

        public UpdateRolePermissionsCommand(Guid userId, Guid groupId, Guid roleId, List<PermissionCommandItem> permissions)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
            Permissions = permissions ?? new List<PermissionCommandItem>();
        }
    }

    public class UpdateRolePermissionsCommandHandler : ICommandHandler<UpdateRolePermissionsCommand>
    {
        private readonly IGroupRepository _groupRepository;
        public UpdateRolePermissionsCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(UpdateRolePermissionsCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            var actions =  command.Permissions.Select(
                x => x.IsOn
                    ? (Action)(()=>group.AddPermissionToRole(UserId.From(command.UserId), RoleId.From(command.RoleId), Permission.FromName(x.Permission)))
                    : ()=>group.RemovePermissionFromRole(UserId.From(command.UserId), RoleId.From(command.RoleId), Permission.FromName(x.Permission))
            );

            foreach (var action in actions)
            {
                action.Invoke();
            }
        }
    }
}