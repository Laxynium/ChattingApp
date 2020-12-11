using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.RemoveChannel
{
    public class RemoveChannelCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }

        public RemoveChannelCommand(Guid userId, Guid groupId, Guid channelId)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
        }
    }

    internal sealed class RemoveChannelHandler : ICommandHandler<RemoveChannelCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;

        public RemoveChannelHandler(IGroupRepository groupRepository, IChannelRepository channelRepository)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
        }
        public async Task HandleAsync(RemoveChannelCommand command)
        {
            var group = await  _groupRepository.GetAsync(GroupId.From(command.GroupId)) 
                        ?? throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) 
                          ?? throw new ChannelNotFoundException(command.ChannelId);

            group.RemoveChannel(UserId.From(command.UserId), channel);

            await _channelRepository.RemoveAsync(channel);
        }
    }
}