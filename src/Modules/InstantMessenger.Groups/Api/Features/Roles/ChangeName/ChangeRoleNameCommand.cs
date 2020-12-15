using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.ChangeName
{
    public class ChangeRoleNameCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string Name { get; }

        public ChangeRoleNameCommand(Guid userId, Guid groupId, Guid roleId, string name)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
            Name = name;
        }
    }

    internal sealed class ChangeRoleNameCommandHandler : ICommandHandler<ChangeRoleNameCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public ChangeRoleNameCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task HandleAsync(ChangeRoleNameCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ??
                        throw new GroupNotFoundException(GroupId.From(command.GroupId));

            group.RenameRole(UserId.From(command.UserId), RoleId.From(command.RoleId), RoleName.Create(command.Name));
        }
    }
}