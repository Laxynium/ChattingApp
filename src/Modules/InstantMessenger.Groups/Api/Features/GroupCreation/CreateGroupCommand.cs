using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.GroupCreation
{
    public class CreateGroupCommand : ICommand
    {
        public Guid GroupId { get; }
        public Guid OwnerId { get; }
        public string Name { get; }

        public CreateGroupCommand(Guid groupId, Guid ownerId, string name)
        {
            GroupId = groupId;
            OwnerId = ownerId;
            Name = name;
        }
    }

    internal sealed class CreateGroupHandler : ICommandHandler<CreateGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public CreateGroupHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(CreateGroupCommand command)
        {
            await _groupRepository.AddAsync(new Group(command.GroupId, command.Name));
            await _groupRepository.SaveAsync();
        }
    }
}