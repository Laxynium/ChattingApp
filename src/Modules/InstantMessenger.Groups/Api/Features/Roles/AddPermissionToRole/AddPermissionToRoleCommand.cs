using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole
{
    public class AddPermissionToRoleCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string PermissionName { get; }
        public AddPermissionToRoleCommand(Guid userId, Guid groupId, Guid roleId, string permissionName)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
            PermissionName = permissionName;
        }
    }

    public class AddPermissionToRoleHandler : ICommandHandler<AddPermissionToRoleCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddPermissionToRoleHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(AddPermissionToRoleCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);
            
            group.AddPermissionToRole(UserId.From(command.UserId), RoleId.From(command.RoleId), Permission.FromName(command.PermissionName));

            await _unitOfWork.Commit();
        }
    }
}