using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Group.Delete
{
    public class RemoveGroupCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }

        public RemoveGroupCommand(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    internal sealed class RemoveGroupCommandHandler : ICommandHandler<RemoveGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public RemoveGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(RemoveGroupCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException(GroupId.From(command.GroupId));

            group.Remove(UserId.From(command.UserId));

            await _groupRepository.RemoveAsync(group);
        }
    }
}