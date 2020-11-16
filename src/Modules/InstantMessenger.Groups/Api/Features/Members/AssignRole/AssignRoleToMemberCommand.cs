using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Members.AssignRole
{
    public class AssignRoleToMemberCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid UserIdOfMember { get; }
        public Guid RoleId { get; }

        public AssignRoleToMemberCommand(Guid userId, Guid groupId, Guid userIdOfMember, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            UserIdOfMember = userIdOfMember;
            RoleId = roleId;
        }
    }

    internal sealed class AssignRoleToMemberHandler : ICommandHandler<AssignRoleToMemberCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignRoleToMemberHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(AssignRoleToMemberCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException();

            group.AssignRole(UserId.From(command.UserId), UserId.From(command.UserIdOfMember), RoleId.From(command.RoleId));

            await _unitOfWork.Commit();
        }
    }
}