using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.MoveDownRoleInHierarchy
{
    public class MoveDownRoleInHierarchyCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public MoveDownRoleInHierarchyCommand(Guid userId, Guid groupId, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
        }
    }

    internal sealed class MoveDownRoleInHierarchyHandler : ICommandHandler<MoveDownRoleInHierarchyCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoveDownRoleInHierarchyHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(MoveDownRoleInHierarchyCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.MoveDownRole(UserId.From(command.UserId), RoleId.From(command.RoleId));

            await _unitOfWork.Commit();
        }
    }
}