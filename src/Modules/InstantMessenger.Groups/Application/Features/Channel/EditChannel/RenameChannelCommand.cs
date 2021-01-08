using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Channel.EditChannel
{
    public class RenameChannelCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public string Name { get; }

        public RenameChannelCommand(Guid userId, Guid groupId, Guid channelId, string name)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            Name = name;
        }
    }

    internal sealed class RenameChannelCommandHandler : ICommandHandler<RenameChannelCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;

        public RenameChannelCommandHandler(IGroupRepository groupRepository, IChannelRepository channelRepository)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
        }
        public async Task HandleAsync(RenameChannelCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ??
                        throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) ??
                          throw new ChannelNotFoundException(ChannelId.From(command.ChannelId));
            var name = ChannelName.Create(command.Name);
            group.RenameChannel(UserId.From(command.UserId), channel, name);
        }
    }
}