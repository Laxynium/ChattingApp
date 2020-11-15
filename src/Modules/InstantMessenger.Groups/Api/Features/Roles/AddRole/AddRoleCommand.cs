using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.AddRole
{
    public class AddRoleCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public string Name { get; }

        public AddRoleCommand(Guid userId, Guid groupId, string name)
        {
            UserId = userId;
            Name = name;
            GroupId = groupId;
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
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException();

            //TODO check if user can perform this action
            group.AddRole(UserId.From(command.UserId),RoleName.Create(command.Name));

            await _unitOfWork.Commit();
        }
    }
}