using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Members.RemoveRole
{
    public class RemoveRoleFromMemberCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid UserIdOfMember { get; }
        public Guid RoleId { get; }

        public RemoveRoleFromMemberCommand(Guid userId, Guid groupId, Guid userIdOfMember, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            UserIdOfMember = userIdOfMember;
            RoleId = roleId;
        }
    }

    internal sealed class RemoveRoleFromMemberHandler : ICommandHandler<RemoveRoleFromMemberCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public RemoveRoleFromMemberHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(RemoveRoleFromMemberCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.RemoveRoleFromMember(UserId.From(command.UserId), UserId.From(command.UserIdOfMember), RoleId.From(command.RoleId));
        }
    }
}