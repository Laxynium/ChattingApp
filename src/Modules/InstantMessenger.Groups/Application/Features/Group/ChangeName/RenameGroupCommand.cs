using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Group.ChangeName
{
    public class RenameGroupCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public string Name { get; }

        public RenameGroupCommand(Guid userId, Guid groupId, string name)
        {
            UserId = userId;
            GroupId = groupId;
            Name = name;
        }
    }

    internal sealed class RenameGroupCommandHandler : ICommandHandler<RenameGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public RenameGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(RenameGroupCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException(GroupId.From(command.GroupId));

            group.Rename(UserId.From(command.UserId), GroupName.Create(command.Name));
        }
    }
}