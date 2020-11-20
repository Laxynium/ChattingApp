using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.MoveDownRoleInHierarchy
{
    public class MoveRoleDownInHierarchyCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public MoveRoleDownInHierarchyCommand(Guid userId, Guid groupId, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
        }
    }

    internal sealed class MoveRoleDownInHierarchyHandler : ICommandHandler<MoveRoleDownInHierarchyCommand>
    {
        private readonly IGroupRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MoveRoleDownInHierarchyHandler(IGroupRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(MoveRoleDownInHierarchyCommand command)
        {
            var group = await _repository.GetAsync(GroupId.From(command.GroupId));

            group.MoveDownRole(UserId.From(command.UserId), RoleId.From(command.RoleId));

            await _unitOfWork.Commit();
        }
    }
}