using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.MoveUpRoleInHierarchy
{
    public class MoveUpRoleInHierarchyCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public MoveUpRoleInHierarchyCommand(Guid userId, Guid groupId, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
        }
    }

    internal sealed class MoveUpRoleInHierarchyHandler : ICommandHandler<MoveUpRoleInHierarchyCommand>
    {
        private readonly IGroupRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MoveUpRoleInHierarchyHandler(IGroupRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(MoveUpRoleInHierarchyCommand command)
        {
            var group = await _repository.GetAsync(GroupId.From(command.GroupId));

            group.MoveUpRole(UserId.From(command.UserId), RoleId.From(command.RoleId));

            await _unitOfWork.Commit();
        }
    }
}