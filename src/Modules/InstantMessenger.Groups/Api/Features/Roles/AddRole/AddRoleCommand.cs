using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.AddRole
{
    public class AddRoleCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string Name { get; }

        public AddRoleCommand(Guid userId, Guid groupId, Guid roleId, string name)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
            Name = name;
        }
    }

    internal sealed class AddRoleHandler : ICommandHandler<AddRoleCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddRoleHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(AddRoleCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.AddRole(UserId.From(command.UserId),RoleId.From(command.RoleId),RoleName.Create(command.Name));

            await _unitOfWork.Commit();
        }
    }
}