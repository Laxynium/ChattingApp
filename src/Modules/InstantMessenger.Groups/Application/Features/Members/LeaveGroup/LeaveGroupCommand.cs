using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Members.LeaveGroup
{
    public class LeaveGroupCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }

        public LeaveGroupCommand(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    internal sealed class LeaveGroupHandler : ICommandHandler<LeaveGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public LeaveGroupHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(LeaveGroupCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.LeaveGroup(UserId.From(command.UserId));
        }
    }
}